using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderRollingScript : MonoBehaviour
{
    public Transform boulderTransform;
    public SphereCollider sphereCollider;
    public Vector3 boulderStartPos;

    public bool boulderRolling = false;
    public float movementX = 0.0f;
    public float movementZ = 0.0f;
    public float boulderSpeed = 0.2f;

    private void Start()
    {
        boulderTransform = GetComponent<Transform>();
        sphereCollider = GetComponent<SphereCollider>();
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
            //PLAY BOULDER ROLLING SOUND CLIP HERE (maybe in a co-routine?)
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
            Debug.Log("Player Dies"); //KILL PLAYER CALL HERE
            boulderTransform.position = boulderStartPos;
            Debug.Log("Death sound plays"); //PLAY DEATH SOUND CLIP HERE
        }

        //Stop the boulder from rolling once the stop point is hit
        else if (other.tag == "BoulderStopPoint")
        {
            Debug.Log("Boulder is stopped and stationary");
            sphereCollider.isTrigger = false;
        }

    }

    public void StartRolling(float triggerPosX, float triggerPosZ)
    {
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
        //PLAY START ROLLING SOUND CLIP HERE
    }
}
