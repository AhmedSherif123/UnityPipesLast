// Copyright 2022 Esri.
// Licensed under the Apache License, Version 2.0

using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArcGISRaycast : MonoBehaviour
{
    [Header("References")]
    public ArcGISMapComponent arcGISMapComponent;
    [SerializeField] private TextMeshProUGUI locationText;
    [SerializeField] private GameObject markerGO;
    [SerializeField] private ArcGISDataFetcher dataFetcher; //- - - > the script responsible for fetching from feature layer .

    [Header("Sphere Raycast Settings")]
    [SerializeField] private float sphereRadius = 0.3f;
    [SerializeField] private float rayDistance = 200f;
    [SerializeField] private LayerMask raycastLayers = Physics.DefaultRaycastLayers;

    private int featureId;
    private string position;

    private void Awake()
    {
        if (arcGISMapComponent == null)
            arcGISMapComponent = FindFirstObjectByType<ArcGISMapComponent>();

        if (dataFetcher == null)
            dataFetcher = FindFirstObjectByType<ArcGISDataFetcher>();
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            StartSphereRaycast(Mouse.current.position.ReadValue());
#elif UNITY_IOS || UNITY_ANDROID
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            StartSphereRaycast(Touchscreen.current.primaryTouch.position.ReadValue());
#endif
    }

    private void StartSphereRaycast(Vector2 screenPosition)
    {
        if (Camera.main == null)
        {
            Debug.LogError("[ArcGISRaycast] Main camera not found!");
            return;
        }

        Camera cam = Camera.main;
        screenPosition.x = Mathf.Clamp(screenPosition.x, 0, Screen.width);
        screenPosition.y = Mathf.Clamp(screenPosition.y, 0, Screen.height);

        Ray ray = cam.ScreenPointToRay(screenPosition);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow, 5f);

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, rayDistance, raycastLayers))
        {
            var arcGISRaycastHit = arcGISMapComponent.GetArcGISRaycastHit(hit);
            featureId = arcGISRaycastHit.featureId;
           

            if (locationText != null)
                locationText.text = $"FeatureID: {featureId} ";

            if (featureId == -1)
            {
                Debug.LogWarning($"[ArcGISRaycast] Invalid hit or no feature ID. ==> ");
                return;
            }

            Debug.Log($"[SphereRaycast] Hit feature {featureId} on layer ");
            UpdateMarkerAndPosition(hit.point);
            DrawDebugSphere(hit.point, sphereRadius, Color.green, 6f);

            // ✅ Send OBJECTID_1 to the data fetcher
            if (dataFetcher != null)
            {
                dataFetcher.FetchFeatureData(featureId, position);
            }
            else
            {
                Debug.LogError("[ArcGISRaycast] No ArcGISDataFetcher assigned!");
            }
        }
        else
        {
            Vector3 endpoint = ray.origin + ray.direction * rayDistance;
            Debug.LogWarning($"[SphereRaycast] No collider hit — endpoint: {endpoint}");
            DrawDebugSphere(endpoint, sphereRadius, Color.red, 3f);
        }
    }

    private void UpdateMarkerAndPosition(Vector3 hitPoint)
    {
        var geoPosition = arcGISMapComponent.EngineToGeographic(hitPoint);
        var location = markerGO.GetComponent<ArcGISLocationComponent>();

        if (location == null)
        {
            Debug.LogError("[ArcGISRaycast] Marker GameObject is missing ArcGISLocationComponent!");
            return;
        }

        location.Position = new ArcGISPoint(geoPosition.X, geoPosition.Y, geoPosition.Z, geoPosition.SpatialReference);

        var point = ArcGISGeometryEngine.Project(geoPosition, ArcGISSpatialReference.WGS84()) as ArcGISPoint;
        position = $"Lat: {point.Y:0.##}  Long: {point.X:0.##}";
    }

    private void DrawDebugSphere(Vector3 position, float radius, Color color, float duration)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = Vector3.one * radius * 2;
        sphere.GetComponent<Collider>().enabled = false;

        var mat = new Material(Shader.Find("Unlit/Color")) { color = color };
        sphere.GetComponent<MeshRenderer>().material = mat;
        Destroy(sphere, duration);
    }
}





/*using UnityEngine;
using TMPro;

public class Interacting : MonoBehaviour
{
    [SerializeField] private ButtonScripts bs; // Assign in Inspector
    public float length = 100f;
    [SerializeField] private SoundEffects effects; // Should contain AudioClip[] audiolist
    [SerializeField] private AudioSource source;

    void Start()
    {
        if (bs == null)
        {
            bs = GetComponent<ButtonScripts>();
            if (bs == null)
                Debug.LogWarning("ButtonScripts reference is missing! Assign it in the Inspector.");
        }

        if (effects == null)
        {
            effects = GetComponent<SoundEffects>();
            if (effects == null)
                Debug.LogWarning("SoundEffects reference is missing! Assign it in the Inspector.");
        }

        if (source == null)
        {
            source = GetComponent<AudioSource>();
            if (source == null)
                Debug.LogWarning("AudioSource is missing! Add one to this GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MOUSE CLICKED");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, length))
            {
                string hitName = hit.transform.name;

                if (bs != null && bs.text != null)
                    bs.text.text = hitName;

                AudioClip clip = System.Array.Find(effects.audiolist, c => c.name == hitName);

                if (clip != null && source != null)
                {
                    source.clip = clip;
                    source.Play();
                    Debug.Log($"Playing sound: {clip.name}");
                }
                else
                {
                    Debug.LogWarning($"No audio clip found with name '{hitName}'");
                }

                Debug.Log($"HIT: {hitName} | Hit origin: {ray.origin}");
            }
            else
            {
                Debug.Log("Hit Nothing");
            }
        }
    }
}
*/