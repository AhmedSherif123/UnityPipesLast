using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    public ARTrackedImageManager imagetracker;
    public GameObject[] ArPrefabs;
    List<GameObject> ARobjects = new List<GameObject>();

    void Awake()
    {
        imagetracker = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        imagetracker.trackedImagesChanged += onTrackImageChanged;
    }

    void OnDisable()
    {
        imagetracker.trackedImagesChanged -= onTrackImageChanged;
    }

    private void onTrackImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedimage in eventArgs.added)
        {
            foreach (var arprefab in ArPrefabs)
            {
                if (trackedimage.referenceImage.name == arprefab.name)
                {
                    var newprefab = Instantiate(arprefab, trackedimage.transform);
                    newprefab.name = arprefab.name; // Removes (Clone)
                    ARobjects.Add(newprefab);
                }
            }
        }

        foreach (var trackedimage in eventArgs.updated)
        {
            foreach (var gameobj in ARobjects)
            {
                if (gameobj.name == trackedimage.referenceImage.name) // match prefab name
                {
                    gameobj.SetActive(trackedimage.trackingState == TrackingState.Tracking);
                }
            }
        }
    }
}
