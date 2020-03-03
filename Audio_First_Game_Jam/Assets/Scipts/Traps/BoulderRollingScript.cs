using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRollingScript : MonoBehaviour
{
    public Transform boulderTransform;
    public SphereCollider sphereCollider;
    public Vector3 boulderStartPos;
    public AudioSource audioSource;
    public AudioClip crashSound;

    public bool boulderRolling = false;
    private float movementX = 0.0f;
    private float movementZ = 0.0f;
    public float boulderSpeed = 0.2f;
    public float boulderDelay = 0.8f;

    private void Start()
    {
        boulderTransform = GetComponent<Transform>();
        sphereCollider = GetComponent<SphereCollider>();
        audioSource = GetComponent<AudioSource>();
        boulderStartPos = boulderTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBoulder();
    }


    private void MoveBoulder()
    {
        if (boulderRolling)
        {
            //Moves the boulder's position once triggered
            boulderTransform.position = new Vector3(boulderTransform.position.x + movementX,
                                                    boulderTransform.position.y,
                                                    boulderTransform.position.z + movementZ);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        boulderRolling = false;
        movementX = 0.0f;
        movementZ = 0.0f;

        //Kill player if hit and reset boulder position
        if (other.tag == "Player")
        {
            boulderTransform.position = boulderStartPos;
            //LevelManager.death("BoulderDeath");
            //LevelManager.respawn();
            audioSource.Stop();
        }

        //Stop the boulder from rolling once the stop point is hit
        else if (other.tag == "BoulderStopPoint")
        {
            sphereCollider.isTrigger = false;
            audioSource.Stop();
            audioSource.clip = crashSound;
            audioSource.Play();

        }

    }

    public IEnumerator StartRolling(float triggerPosX, float triggerPosZ)
    {
        yield return new WaitForSeconds(boulderDelay);
        audioSource.Play();
        sphereCollider.isTrigger = true;
        boulderRolling = true;

        //Use the pressure plate's position to determine the direction the boulder will roll
        if (triggerPosX > boulderTransform.position.x)
        {
            movementX = boulderSpeed;
        }
        else if (triggerPosX < boulderTransform.position.x)
        {
            movementX = -boulderSpeed;
        }
        else if (triggerPosZ > boulderTransform.position.z)
        {
            movementZ = boulderSpeed;
        }
        else
        {
            movementZ = -boulderSpeed;
        }
    }
}
