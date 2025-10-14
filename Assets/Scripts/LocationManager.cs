using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using Esri.HPFramework;

public class UnityLocationManager : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject locationProvider;
    private AndroidJavaClass unityPlayerClass;
    private AndroidJavaObject currentActivity;
#endif

    [Header("UI References")]
   
    //public TMP_Text stateText;

    private float lastUpdateTime = 0f;
    private const float freezeThreshold = 2f;

    // Queue for main thread actions
    private readonly Queue<Action> mainThreadActions = new Queue<Action>();

    // Stores the last location
    public struct LocationData
    {
        public double Latitude;
        public double Longitude;
        public double Altitude;
        public float Accuracy;
        public long Timestamp;
    }
    public LocationData CurrentLocation { get; private set; }

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
           // if (stateText != null) stateText.text = "Waiting for permission...";
        }
        else
        {
            InitLocationProvider();
        }
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void InitLocationProvider()
    {
        locationProvider = new AndroidJavaObject("com.example.mylibrary.LocationProvider", currentActivity);

        var listenerProxy = new LocationListenerProxy(this);
        locationProvider.Call("startLocationUpdates", listenerProxy);

        // Start foreground service for background tracking
        currentActivity.Call("startService",
            new AndroidJavaObject("android.content.Intent", currentActivity,
                new AndroidJavaClass("com.example.mylibrary.LocationForegroundService")));
    }

    private class LocationListenerProxy : AndroidJavaProxy
    {
        private readonly UnityLocationManager manager;

        public LocationListenerProxy(UnityLocationManager mgr)
            : base("com.example.mylibrary.LocationProvider$LocationListener")
        {
            manager = mgr;
        }

        void onLocationChanged(double latitude, double longitude, double altitude, float accuracy, long timestamp)
        {
            manager.EnqueueAction(() => manager.HandleLocation(latitude, longitude, altitude, accuracy, timestamp));
        }

        void onLocationError(string message)
        {
            manager.EnqueueAction(() => manager.HandleError(message));
        }

        void onLocationState(string message)
        {
            manager.EnqueueAction(() => manager.HandleState(message));
        }
    }

    // Called from the background service via UnitySendMessage
    public void OnBackgroundLocation(string message)
    {
        EnqueueAction(() =>
        {
            var parts = message.Split(',');
            if (parts.Length == 5 &&
                double.TryParse(parts[0], out double lat) &&
                double.TryParse(parts[1], out double lon) &&
                double.TryParse(parts[2], out double alt) &&
                float.TryParse(parts[3], out float acc) &&
                long.TryParse(parts[4], out long ts))
            {
                HandleLocation(lat, lon, alt, acc, ts);
            }
        });
    }

    private void EnqueueAction(Action action)
    {
        lock (mainThreadActions)
        {
            mainThreadActions.Enqueue(action);
        }
    }
#endif

    private void HandleLocation(double lat, double lon, double alt, float acc, long timestamp)
    {
        // Store current location
        CurrentLocation = new LocationData
        {
            Latitude = lat,
            Longitude = lon,
            Altitude = alt,
            Accuracy = acc,
            Timestamp = timestamp
        };

        // Update last time
        lastUpdateTime = Time.time;

        // Update UI
        string msg = $"Lat: {lat:F6}\nLon: {lon:F6}\nAlt: {alt:F1} m\nAcc: {acc:F1} m\nTime: {timestamp}";
       
       // if (stateText != null && stateText.text.Contains("Frozen")) stateText.text = "";

       
        Debug.Log(msg);
    }

    private void HandleState(string message)
    {
        Debug.Log("[LocationState] " + message);
        //if (stateText != null) stateText.text = message;
    }

    private void HandleError(string message)
    {
        Debug.LogError("[LocationError] " + message);
        //if (stateText != null) stateText.text = "Error: " + message;
    }

    void Update()
    {
        // Execute queued actions on main thread
        lock (mainThreadActions)
        {
            while (mainThreadActions.Count > 0)
            {
                mainThreadActions.Dequeue()?.Invoke();
            }
        }

        // Check for frozen updates
        /*if (Time.time - lastUpdateTime > freezeThreshold)
        {
            if (stateText != null)
                stateText.text = "Location updates frozen, waiting for GPS fix...";
        }*/

    }

    void OnDestroy()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (locationProvider != null)
        {
            try { locationProvider.Call("stopLocationUpdates"); } catch { }
        }

        if (currentActivity != null)
        {
            try
            {
                currentActivity.Call("stopService",
                    new AndroidJavaObject("android.content.Intent", currentActivity,
                        new AndroidJavaClass("com.example.mylibrary.LocationForegroundService")));
            }
            catch { }
        }
#endif
    }
}
