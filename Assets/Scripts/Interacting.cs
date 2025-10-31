using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Esri.Unity;
using Esri.GameEngine.Layers;
public class ArcGISRaycast : MonoBehaviour
{
    [Header("References")]
    public ArcGISMapComponent arcGISMapComponent;
    public ArcGISDataFetcher dataFetcher;
    public Camera cam;
    public GameObject markerGO;
    public TextMeshProUGUI locationText;

    [Header("Settings")]
    public float maxDistance = 500f;
    public float debugDuration = 4f;
    public LayerMask raycastLayers = Physics.DefaultRaycastLayers;

    private int featureId = -1;
    private string position;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;

        if (arcGISMapComponent == null)
            arcGISMapComponent = FindFirstObjectByType<ArcGISMapComponent>();

        if (dataFetcher == null)
            dataFetcher = FindFirstObjectByType<ArcGISDataFetcher>();
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            HandleClick(Mouse.current.position.ReadValue());
#elif UNITY_IOS || UNITY_ANDROID
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            HandleClick(Touchscreen.current.primaryTouch.position.ReadValue());
#endif
    }

    private void HandleClick(Vector2 screenPos)
    {
        if (cam == null)
        {
            Debug.LogError("[ArcGISRaycast] No camera assigned!");
            return;
        }

        // ✅ Start ray from the camera position toward the clicked screen point
        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, raycastLayers))
        {
            // ---- Draw ray to exact hit position ----
            Debug.DrawLine(ray.origin,  hit.point, Color.yellow, debugDuration);

            // ---- Try to get ArcGIS feature ID ----
            featureId = -1;
            var arcHit = arcGISMapComponent?.GetArcGISRaycastHit(hit);
            if (arcHit.HasValue)
            {
                featureId = arcHit.Value.featureId;
                Debug.LogWarning($"[ArcGISRaycast] 🎯 Hit ArcGIS feature {featureId}");
            }
            else
            {
                Debug.Log($"[ArcGISRaycast] Hit collider: {hit.collider.name}");
            }

            // ---- UI feedback ----
            if (locationText)
                locationText.text = $"FeatureID: {featureId}";

            // ---- Marker placement ----
            UpdateMarkerAndPosition(hit.point);
            DrawDebugSphere(hit.point, 2f, Color.green);

            // ---- Send to data fetcher ----
            dataFetcher?.FetchFeatureData(featureId, position);
        }
        else
        {
            // ---- Missed hit ----
            Vector3 missEnd = ray.origin + ray.direction * maxDistance;
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red, debugDuration);
            DrawDebugSphere(missEnd, 0.1f, Color.red);
            Debug.Log("[ArcGISRaycast] ❌ No collider hit.");
        }
    }

    private void UpdateMarkerAndPosition(Vector3 hitPoint)
    {
        if (arcGISMapComponent == null || markerGO == null)
            return;

        var geo = arcGISMapComponent.EngineToGeographic(hitPoint);
        var markerLoc = markerGO.GetComponent<ArcGISLocationComponent>();

        if (markerLoc != null)
        {
            markerLoc.Position = new ArcGISPoint(geo.X, geo.Y, geo.Z, geo.SpatialReference);
            var wgs = ArcGISGeometryEngine.Project(geo, ArcGISSpatialReference.WGS84()) as ArcGISPoint;
            position = $"Lat: {wgs.Y:0.#####}, Lon: {wgs.X:0.#####}";
        }
    }

    private void DrawDebugSphere(Vector3 pos, float radius, Color color)
    {
        GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        s.transform.position = pos;
        s.transform.localScale = Vector3.one * radius ;
        s.GetComponent<Collider>().enabled = false;
        s.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Color")) { color = color };
        s.layer = 3;
        Destroy(s, debugDuration);
    }
}
