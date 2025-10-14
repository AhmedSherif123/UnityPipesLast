using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    public Camera ARCamera;      // AR camera
    public Camera ArcGISCamera;  // Map camera

    [Header("UI")]
    public Button switchButton;
    public TMP_Text btnText;

    private bool showingAR = true;

    void Start()
    {
        if (switchButton != null)
            switchButton.onClick.AddListener(SwitchCamera);

        // Start with AR mode active
        SetARMode();
    }

    public void SwitchCamera()
    {
        if (showingAR)
            SetMapMode();
        else
            SetARMode();

        showingAR = !showingAR;
    }

    private void SetARMode()
    {
        // Show AR on top
        if (ARCamera != null && ArcGISCamera != null)
        {
            ARCamera.enabled = true;
            ArcGISCamera.enabled = true;

            // AR on top
            ARCamera.depth = 1;
            ArcGISCamera.depth = 0;

            // Keep AR clear normally, Map can be background
            ArcGISCamera.clearFlags = CameraClearFlags.Skybox;
            ARCamera.clearFlags = CameraClearFlags.Nothing;
        }

        if (btnText != null)
            btnText.text = "Switch to MAP";
    }

    private void SetMapMode()
    {
        // Show Map on top
        if (ARCamera != null && ArcGISCamera != null)
        {
            ARCamera.enabled = true;
            ArcGISCamera.enabled = true;

            // Map on top
            ARCamera.depth = 0;
            ArcGISCamera.depth = 1;

            ArcGISCamera.clearFlags = CameraClearFlags.Skybox;
            ARCamera.clearFlags = CameraClearFlags.Nothing;
        }

        if (btnText != null)
            btnText.text = "Switch to AR";
    }
}
