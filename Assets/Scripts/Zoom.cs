using Esri.HPFramework;
using UnityEngine;

public class PinchZoomDetector : MonoBehaviour
{
    [SerializeField] private HPTransform CamPos;

    public  const double MinAltitude = 5.0;
    public const double MaxAltitude = 8880955.0;

    // Speed settings
    [SerializeField] private float zoomSpeed = 0.5f;   // Zoom responsiveness
    [SerializeField] private float smoothSpeed = 5f;   // Interpolation speed

    private double targetAltitude;

    void Start()
    {
        if (CamPos == null)
        {
            CamPos = GetComponent<HPTransform>();
        }

        // Set starting target altitude to current y position
        targetAltitude = CamPos.UniversePosition.y;

        
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            float prevDistance = Vector2.Distance(touch0Prev, touch1Prev);
            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            float zoomDelta = currentDistance - prevDistance;

            if (Mathf.Abs(zoomDelta) > 0.01f)
            {
                AdjustTargetAltitude(zoomDelta);
            }
        }

        // Smoothly interpolate towards target altitude
        var pos = CamPos.UniversePosition;
        pos.y = Mathf.Lerp((float)pos.y, (float)targetAltitude, Time.deltaTime * smoothSpeed);
        CamPos.UniversePosition = pos;
    }

    private void AdjustTargetAltitude(float zoomDelta)
    {
        double currentAltitude = CamPos.UniversePosition.y;

        // Scale zoom change based on altitude
        double zoomChange = zoomDelta * zoomSpeed * (currentAltitude * 0.001);

        // Increase or decrease target altitude
        targetAltitude = Mathf.Clamp(
            (float)(targetAltitude - zoomChange),
            (float)MinAltitude,
            (float)MaxAltitude
        );
    }
}
