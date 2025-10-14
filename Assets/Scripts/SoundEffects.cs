using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public string folder;
    [SerializeField] public AudioClip[] audiolist;
    private AudioSource sound;


    void Start()
    {
        audiolist = Resources.LoadAll<AudioClip>(folder);

        if (audiolist.Length == 0)
        {
            Debug.Log($"No audio found");
        }
        else
        {
            foreach (var ad in audiolist)
            {
                Debug.Log($" Found Audio clip with name {ad.name}");
            }

            sound = GetComponent<AudioSource>();
        }
    }
}
