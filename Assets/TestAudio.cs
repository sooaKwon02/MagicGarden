using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudio : MonoBehaviour
{
    public AudioSource source;

    void Start()
    {
        if (source != null)
        {
            source.Play();
            Debug.Log("Audio Started!");
        }
        else
        {
            Debug.LogError("AudioSource is null!");
        }
    }
}
