using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge_Primary_Sound : MonoBehaviour
{
    public GameObject[] wind_Sources;
    public AudioSource primary_Source;
    public AudioClip ropeBreak;
    public int replayVal;

    private int currentReplayVal;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(GameObject wind in wind_Sources)
            {
                wind.transform.position = new Vector3(wind.transform.position.x, wind.transform.position.y, other.transform.position.z);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && currentReplayVal < replayVal)
        {
            primary_Source.PlayOneShot(ropeBreak);
            //currentReplayVal++;
        }
    }
}
