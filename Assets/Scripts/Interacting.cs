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