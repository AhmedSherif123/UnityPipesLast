using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Esri.HPFramework;
using System.Collections;

public class GyroCompassSplit : MonoBehaviour
{
    [Header("References")]
    public Camera arCamera;
    public HPTransform mypos;
    public TMP_Text debugText;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float compassFilterStrength = 0.2f;
    public bool useLandscapeLeft = true;

    [Header("Jitter Filter")]
    public float rotationThreshold; // ⭐ minimum degrees to apply rotation

    private Quaternion lastGyroRotation = Quaternion.identity;
    private bool sensorsReady = false;
    private float smoothedCompassHeading = 0f;

    // FPS calculation
    private int frames = 0;
    private float timePassed = 0f;
    private float currentFPS = 0f;

    private Quaternion lastAppliedRotation = Quaternion.identity; // ⭐ for threshold check

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        if (arCamera == null) arCamera = Camera.main;
        if (mypos == null) mypos = GetComponent<HPTransform>();
    }

    private void OnEnable()
    {
        Application.onBeforeRender += OnBeforeRenderUpdate;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= OnBeforeRenderUpdate;
    }

    private void Start()
    {
        StartCoroutine(InitializeSensors());
    }

    private IEnumerator InitializeSensors()
    {
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        yield return new WaitForSeconds(1f);

        if (!Input.gyro.enabled || !SystemInfo.supportsGyroscope)
        {
            Debug.LogError("Gyroscope not available!");
            yield break;
        }

        lastGyroRotation = GyroToUnity(Input.gyro.attitude);
        smoothedCompassHeading = Input.compass.trueHeading;
        lastAppliedRotation = arCamera.transform.localRotation;
        sensorsReady = true;
    }

    private void OnBeforeRenderUpdate()
    {
        if (!sensorsReady) return;

        try
        {
            // Gyro gives pitch/roll
            Quaternion rawGyro = GyroToUnity(Input.gyro.attitude);
            lastGyroRotation = rawGyro;

            Vector3 gyroEuler = lastGyroRotation.eulerAngles;
            float pitch = NormalizeAngle(gyroEuler.x);
            float roll = NormalizeAngle(gyroEuler.z);

            // Compass yaw (absolute north)
            float rawCompassHeading = GetCompassHeading();

            // Smooth only the compass yaw
            smoothedCompassHeading = Mathf.LerpAngle(
                smoothedCompassHeading,
                rawCompassHeading,
                1f - compassFilterStrength
            );

            float correctedYaw = smoothedCompassHeading;
            Quaternion fusedRotation = Quaternion.Euler(pitch, correctedYaw, roll);

            // ⭐ Threshold check
         float angleDiff = Quaternion.Angle(lastAppliedRotation, fusedRotation);
            if (angleDiff >= rotationThreshold) {
            arCamera.transform.localRotation = fusedRotation;
            lastAppliedRotation = fusedRotation;
            }


            UpdateDebugInfo(rawCompassHeading, smoothedCompassHeading);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Sensor error: {e.Message}");
            sensorsReady = false;
        }
    }

    private void LateUpdate()
    {
        UpdateFPS();
    }

    private float GetCompassHeading()
    {
        if (Input.compass.trueHeading == 0f && Input.compass.headingAccuracy < 0)
            return smoothedCompassHeading;

        return Input.compass.trueHeading;
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        Quaternion converted = new Quaternion(q.x, q.y, -q.z, -q.w);
        return useLandscapeLeft
            ? Quaternion.Euler(90f, 0f, -90f) * converted
            : Quaternion.Euler(90f, 0f, 90f) * converted;
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    private void UpdateFPS()
    {
        frames++;
        timePassed += Time.unscaledDeltaTime;

        if (timePassed >= 1f)
        {
            currentFPS = frames / timePassed;
            frames = 0;
            timePassed = 0f;
        }
    }

    private void UpdateDebugInfo(float rawCompass, float smoothedCompass)
    {
        if (debugText != null)
        {
            debugText.text =
                $"Compass Raw: {rawCompass:F1}°\n" +
                $"FPS: {currentFPS:F1}";
        }
    }

    public void Recalibrate()
    {
        if (sensorsReady)
        {
            lastGyroRotation = GyroToUnity(Input.gyro.attitude);
            smoothedCompassHeading = Input.compass.trueHeading;
            lastAppliedRotation = arCamera.transform.localRotation;
        }
    }
}
