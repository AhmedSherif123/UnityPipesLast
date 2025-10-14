
/*    Add layer
using System;
using UnityEngine;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Layers;
using Esri.GameEngine.Layers.Base;
using Esri.Unity;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class AddLayer : MonoBehaviour
{
    [SerializeField] private ArcGISMapComponent arcGISMapComponent;
    [SerializeField] public Material PipesColors;

    [Serializable]
    public enum LayerType
    {
        ArcGISMapImageLayer,
        ArcGISFeatureLayer,
        ArcGIS3DObjectSceneLayer,
        ArcGISTiledLayer
    };

    [Header("Layer Settings")]
    [SerializeField] private LayerType type;
    [SerializeField] private string url;
    [SerializeField] private string layerName;
    [SerializeField] private bool setVisible = true;
    [SerializeField][Range(0, 1)] private float opacity = 1.0f;

    [Header("Registry Settings")]
    public int LayersID;
    private bool addedThisSession = false;

    // Track if we've tried to add in Edit Mode
    private bool attemptedEditModeAdd = false;

    private void OnEnable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorApplication.update += EditorUpdate;
        }
#endif
        TryAddLayer();
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorApplication.update -= EditorUpdate;
        }
#endif
    }

    private void Start()
    {
        TryAddLayer();
    }

#if UNITY_EDITOR
    private void EditorUpdate()
    {
        if (!Application.isPlaying && !attemptedEditModeAdd)
        {
            TryAddLayerWithDelay();
        }
    }

    private void TryAddLayerWithDelay()
    {
        attemptedEditModeAdd = true;
        EditorApplication.delayCall += () =>
        {
            if (this != null)
            {
                TryAddLayer();
            }
        };
    }
#endif

    public void TryAddLayer()
    {
        if (addedThisSession) return;

        if (!Application.isPlaying && arcGISMapComponent == null)
        {
            arcGISMapComponent = FindObjectOfType<ArcGISMapComponent>();
            if (arcGISMapComponent == null)
            {
                Debug.LogWarning("ArcGISMapComponent not found, waiting...");
                return;
            }
        }

        AddLayerToMap();
        addedThisSession = true;
    }

    public void AddLayerToMap()
    {
        if (arcGISMapComponent?.View?.Map == null)
        {
            Debug.LogError("❌ ArcGIS Map is null or not initialized");
            return;
        }

        var arcGISMap = arcGISMapComponent.View.Map;
        ArcGISLayer layer = null;

        try
        {
            switch (type)
            {
                case LayerType.ArcGIS3DObjectSceneLayer:
                    layer = new ArcGIS3DObjectSceneLayer(url, arcGISMapComponent.APIKey);
                    break;

                case LayerType.ArcGISMapImageLayer:
                    layer = new ArcGISImageLayer(url, arcGISMapComponent.APIKey);
                    break;

                case LayerType.ArcGISTiledLayer:
                    layer = new ArcGISVectorTileLayer(url, arcGISMapComponent.APIKey);
                    break;

                default:
                    Debug.LogError($"❌ Unsupported layer type: {type}");
                    return;
            }

            layer.IsVisible = setVisible;
            layer.Opacity = opacity;

            if (!string.IsNullOrEmpty(layerName))
            {
                layer.Name = layerName;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Failed to construct layer {layerName}: {ex.Message}");
            return;
        }

        // Check if layer already exists
        ulong layerCount = arcGISMap.Layers.GetSize();
        for (ulong i = 0; i < layerCount; i++)
        {
            var existingLayer = arcGISMap.Layers.At(i);
            if (existingLayer != null)
            {
                if (existingLayer.Name == layer.Name ||
                    (existingLayer is ArcGIS3DObjectSceneLayer sceneLayer && sceneLayer.Source == url))
                {
                    Debug.Log($"Layer {layer.Name} already exists in the map");
                    return;
                }
            }
        }

        // Add to map
        arcGISMap.Layers.Add(layer);
        LayersID = arcGISMap.Layers.IndexOf(layer);

        Debug.Log($"✅ Added layer '{layer.Name}' (type {type}) with index: {LayersID}");

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorUtility.SetDirty(this);
            EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
#endif
    }

    [ContextMenu("Add Layer Now")]
    public void AddLayerNow()
    {
        addedThisSession = false;
        attemptedEditModeAdd = false;
        TryAddLayer();
    }
}*/