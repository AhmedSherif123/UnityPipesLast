using UnityEngine;
using UnityEngine.Android;
using TMPro;

public class LocationPermissionRequester : MonoBehaviour
{
   

    void Start()
    {
        RequestPermissions();
    }

    void RequestPermissions()
    {
#if UNITY_ANDROID
        // Fine Location
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            Permission.RequestUserPermission(Permission.FineLocation);

        // Coarse Location
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation))
            Permission.RequestUserPermission(Permission.CoarseLocation);

        // Camera
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            Permission.RequestUserPermission(Permission.Camera);

        // Background Location (Android 10+)
        string backgroundLocation = "android.permission.ACCESS_BACKGROUND_LOCATION";
        if (!Permission.HasUserAuthorizedPermission(backgroundLocation))
            Permission.RequestUserPermission(backgroundLocation);

        // Foreground Service
        string foregroundService = "android.permission.FOREGROUND_SERVICE";
        if (!Permission.HasUserAuthorizedPermission(foregroundService))
            Permission.RequestUserPermission(foregroundService);
#endif
    }


    
}
