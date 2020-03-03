using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTriggerScript : MonoBehaviour
{
    public BoulderRollingScript boulderRollingScript;
    public AudioSource audioSource;
    public bool isTriggered = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //TODO RESET ON PLAYER DEATH

    private void OnTriggerEnter(Collider other)
    {
        //Detect player when they touch the pressure plate
        if (other.tag == "Player" && isTriggered == false)
        {
            isTriggered = true;
            audioSource.Play();
            StartCoroutine(boulderRollingScript.StartRolling(transform.position.x, transform.position.z));
        }
    }

}
