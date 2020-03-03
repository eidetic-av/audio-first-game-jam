using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingDoorScript : MonoBehaviour
{
    public Transform doorTransform;
    public Vector3 doorStartPos;
    public Rigidbody doorRigidbody;
    public BoxCollider doorCollider;

    //These values can be changed in the inspector
    public float doorRaiseSpeed;
    public float doorMaxHeight;
    public float dropWaitTime;

    private float yDifference;

    public enum DoorState { Rising, DeadlyFalling, Falling, Stopped };
    public DoorState doorState = DoorState.Rising;


    // Start is called before the first frame update
    void Start()
    {
        doorTransform = GetComponent<Transform>();
        doorRigidbody = GetComponent<Rigidbody>();
        doorCollider = GetComponent<BoxCollider>();
        doorStartPos = doorTransform.position;
    }

    void FixedUpdate()
    {
        RaiseDoor();
    }

    public void RaiseDoor()
    {
        //Start raising the door and drop door once it's at max height
        if(doorTransform.position.y < doorMaxHeight && doorState == DoorState.Rising)
        {
            doorTransform.position = new Vector3(doorTransform.position.x,
                                               doorTransform.position.y + doorRaiseSpeed,
                                               doorTransform.position.z);
        }
        else
        {
            DropDoor();
        }
    }

    public void DropDoor()
    {
        //Change door to falling and enable triggers
        if (doorState == DoorState.Rising)
        {
            doorState = DoorState.DeadlyFalling;
            doorRigidbody.useGravity = true;
            doorCollider.isTrigger = true;
        }

        //If the door has fallen a certain point, it shouldn't kill the player
        //This turns the door into a collider to stop the player from walking past as opposed to killing them
        if(doorState == DoorState.DeadlyFalling && doorMaxHeight - doorTransform.position.y >= (doorMaxHeight - doorStartPos.y)*0.6)
        {
            doorCollider.isTrigger = false;
            doorState = DoorState.Falling;
        }

        //Check if door has reached the floor
        if (doorTransform.position.y <= doorStartPos.y && doorState == DoorState.Falling)
        {
            //Stop door falling and start rising after a bit
            doorTransform.position = doorStartPos;
            doorState = DoorState.Stopped;
            Invoke("StartRising", dropWaitTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Kill the player and reset on trigger enter
        if (other.tag == "Player")
        {
            Debug.Log("Kill player"); //Player dies call here
            doorTransform.position = doorStartPos;
            doorCollider.isTrigger = false;
            StartRising();
        }  
    }

    //Initiates the raising of the door
    public void StartRising()
    {
        doorRigidbody.useGravity = false;
        doorState = DoorState.Rising;
    }

}
