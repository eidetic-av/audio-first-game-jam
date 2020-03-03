using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge_Wind_Sound : MonoBehaviour
{
    public AudioSource windAudioSource;
    public AudioClip ropeBreak;
    public int replayVal;

    private int currentReplayVal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentReplayVal < replayVal)
        {
           // windAudioSource.PlayOneShot(ropeBreak);
           /// currentReplayVal++;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //windAudioSource.Stop();
        }
    }
}
