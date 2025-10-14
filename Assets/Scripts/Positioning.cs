using TMPro;
using UnityEngine;
using Esri.HPFramework;
using Unity.Mathematics;
using System.Collections;
using Esri.ArcGISMapsSDK.Components;

public class Positioning : MonoBehaviour
{
    private const float EarthRadius = 6378137f;
    private const float UIUpdateInterval = 0.2f;
    public UnityLocationManager locationManager;

    [Header("UI")]
    [SerializeField] private TMP_Text mylocation;

    [Header("Transforms")]
    [SerializeField] private HPTransform XRCamPos;
    [SerializeField] private HPTransform MapCamPos;
   
    [SerializeField] public HPTransform PluginPos;

    [Header("Debug / Fake Location")]
    public float fakeLat = 30.089610f;
    public float fakeLon = 31.70033f;

    public bool gpsReady = false;
    public bool usingFake = false;

    private float lastUIUpdateTime;

    private void Start()
    {
        mylocation.text = "Loading...";

#if UNITY_EDITOR
        UseFakeLocation();
#else
        if (!Input.location.isEnabledByUser)
        {
            UseFakeLocation();
        }
        else
        {
            StartCoroutine(StartLocationService());
        }
#endif
    }

    private IEnumerator StartLocationService()
    {
        Input.location.Start(0.5f, 0.5f); // accuracy & distance
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0 || Input.location.status == LocationServiceStatus.Failed)
        {
            UseFakeLocation();
            yield break;
        }

        gpsReady = true;

        
    }

    private void Update()
    {
        if (!gpsReady && !usingFake)
        {
            if (Time.time - lastUIUpdateTime > UIUpdateInterval)
            {
                mylocation.text = "Waiting for GPS...";
                lastUIUpdateTime = Time.time;
            }
            return;
        }
        else
        {
            double currentLat = usingFake ? fakeLat : locationManager.CurrentLocation.Latitude;
            double currentLon = usingFake ? fakeLon : locationManager.CurrentLocation.Longitude;
            double currentAlt = usingFake ? (float)XRCamPos.UniversePosition.y : locationManager.CurrentLocation.Altitude;

            Vector2 mercatorPos = LatLonToWebMercator((float)currentLat, (float)currentLon);

          

          UpdateTransforms(mercatorPos);

            PluginPosition();



            // Update UI
            if (Time.time - lastUIUpdateTime > UIUpdateInterval)
            {
                UpdateUIText((float)currentLat, (float)currentLon, (float)currentAlt, mercatorPos);
                lastUIUpdateTime = Time.time;
            }
        }
        
    }
    private void UpdateTransforms(Vector2 mercatorPos)
    {
        // Update XR camera
        XRCamPos.UniversePosition = new double3(
            mercatorPos.x,
            XRCamPos.UniversePosition.y,
            mercatorPos.y
        );


        // Map camera
        MapCamPos.UniversePosition = new double3(
            mercatorPos.x,
           MapCamPos.UniversePosition.y,
            mercatorPos.y
        );



    }

    private void UpdateUIText(float lat, float lon, float alt, Vector2 mercatorPos)
    {
        string source = usingFake ? "(FakeData)" : "(GPS)";
        mylocation.text = $"{source}\nLat: {lat:F6}\nLon: {lon:F6}\nAlt: {alt:F1} m" +
                   $"\nX: {XRCamPos.UniversePosition.x:F3} m\nY: {XRCamPos.UniversePosition.z:F3} m";

    }


    private void UseFakeLocation()
    {
        gpsReady = false;
        usingFake = true;
    }
    public void PluginPosition ()
    {
        double x;
        double y;
        if (!usingFake)
        {
            x = locationManager.CurrentLocation.Latitude;
            y = locationManager.CurrentLocation.Longitude;
        }
        else
        {
            x = fakeLat;
            y = fakeLon;
        }

        Vector2 LatLong =  LatLonToWebMercator((float)x, (float)y);
        PluginPos.UniversePosition = new double3(LatLong.x, PluginPos.UniversePosition.y, LatLong.y);
         }

    private Vector2 LatLonToWebMercator(float lat, float lon)
    {
        float x = EarthRadius * Mathf.Deg2Rad * lon;
        float y = EarthRadius * Mathf.Log(Mathf.Tan(Mathf.PI / 4f + Mathf.Deg2Rad * lat / 2f));
        return new Vector2(x, y);
    }
}
