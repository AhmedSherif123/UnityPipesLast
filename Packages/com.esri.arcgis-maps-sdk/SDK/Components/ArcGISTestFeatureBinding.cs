using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Layers;

[ExecuteAlways]
public class AttributeList : MonoBehaviour
{
    [Header("ArcGIS Components")]
    public ArcGISMapComponent mapComponent;

    [Header("Custom Shader Material")]
    [Tooltip("Assign your Shader Graph material here (must contain _OBJECTID_1 property)")]
    public Material baseMaterial;

    // Each GameObject has its own FeatureID and Material instance
    public Dictionary<GameObject, int> featureIdLookup = new();

    private Transform mapParent;
    private HashSet<string> trackedNames = new();
    private int lastChildCount = 0;

    private void Awake()
    {
        if (mapComponent == null)
            mapComponent = FindFirstObjectByType<ArcGISMapComponent>();
    }

    private async void Start()
    {
        await WaitForMapLoaded();
        InitializeTracking();
    }

    private async Task WaitForMapLoaded()
    {
        while (mapComponent == null ||
               mapComponent.Map == null ||
               mapComponent.Map.LoadStatus != Esri.GameEngine.ArcGISLoadStatus.Loaded)
        {
            await Task.Delay(5000);
        }
    }

    private void InitializeTracking()
    {
        if (mapComponent.transform.childCount == 0)
        {
            Debug.LogWarning("ArcGIS map has no child renderers yet.");
            return;
        }

        // Typically last child holds renderer GameObjects
        mapParent = mapComponent.transform.GetChild(mapComponent.transform.childCount - 1);
        lastChildCount = mapParent.childCount;
        ScanChildren();
    }

    private void Update()
    {
        if (mapParent == null) return;

        // Detect if new GameObjects were streamed in
        if (mapParent.childCount != lastChildCount)
        {
            ScanChildren();
            lastChildCount = mapParent.childCount;
        }
    }

    private void ScanChildren()
    {
        for (int i = 0; i < mapParent.childCount; i++)
        {
            var child = mapParent.GetChild(i);
            var name = child.name;

            if (name.Contains("UnusedPoolGOs") || name.Contains("Layer_0_"))
                continue;

            if (trackedNames.Contains(name))
                continue;

            trackedNames.Add(name);

            int featureId = TryExtractFeatureID(child.gameObject);
            featureIdLookup[child.gameObject] = featureId;

            ApplyIndependentMaterial(child.gameObject, featureId);

            if (featureId != -1)
                Debug.Log($"🧩 '{name}' FeatureID: {featureId}");
            else
                Debug.Log($"⚠️ '{name}' has no valid FeatureID");
        }
    }

    private int TryExtractFeatureID(GameObject go)
    {
        try
        {
            var rendererComponent = go.GetComponentInParent<ArcGISRendererComponent>();
            if (rendererComponent == null)
                return -1;

            var renderable = rendererComponent.GetRenderableByGameObject(go);
            if (renderable == null)
                return -1;

            // Check for the _FeatureIds texture
            if (renderable.Material.NativeMaterial.HasTexture("_FeatureIds"))
            {
                var featureIds = (Texture2D)renderable.Material.NativeMaterial.GetTexture("_FeatureIds");

                // Take the first pixel (assuming 1 feature per mesh)
                var color = featureIds.GetPixel(0, 0);

                // Decode RGBA → integer FeatureID
                var scaledColor = new Vector4(255f * color.r, 255f * color.g, 255f * color.b, 255f * color.a);
                var shift = new Vector4(1, 0x100, 0x10000, 0x1000000);
                scaledColor.Scale(shift);
                int featureId = (int)(scaledColor.x + scaledColor.y + scaledColor.z + scaledColor.w);

                return featureId;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Error extracting FeatureID from {go.name}: {ex.Message}");
        }

        return -1;
    }

    private void ApplyIndependentMaterial(GameObject go, int featureId)
    {
        if (baseMaterial == null)
        {
            Debug.LogWarning("⚠️ No base material assigned — skipping material application.");
            return;
        }

        var renderer = go.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            Debug.LogWarning($"⚠️ '{go.name}' has no MeshRenderer.");
            return;
        }

        // ✅ Create a unique material instance for this GameObject
        Material instance = new(baseMaterial);
        renderer.material = instance;

        // ✅ Set shader property for this specific feature
        instance.SetFloat("_OBJECTID_1", featureId);

        Debug.Log($"🎨 Applied independent material to '{go.name}' (FeatureID: {featureId})");
    }
}
