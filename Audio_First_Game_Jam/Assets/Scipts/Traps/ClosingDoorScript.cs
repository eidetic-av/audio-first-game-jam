using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingDoorScript : MonoBehaviour
{
    public Transform doorTransform;
    public Vector3 doorStartPos;
    public Rigidbody doorRigidbody;
    public BoxCollider doorCollider;

    //These values can be changed in the inspector, recommended values listed
    public float doorRaiseSpeed; //0.02
    public float doorMaxHeight; //8
    public float dropWaitTime; //1

    public AudioSource audioSource;
    public AudioClip riseSound;
    public AudioClip fallSound;

    public enum DoorState { Rising, Suspended, DeadlyFalling, Falling, Stopped };
    public DoorState doorState = DoorState.Stopped;


    // Start is called before the first frame update
    void Start()
    {
        doorTransform = GetComponent<Transform>();
        doorRigidbody = GetComponent<Rigidbody>();
        doorCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        doorStartPos = doorTransform.position;
        StartRising();
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
            SuspendDoor();
            CheckDoorPosition();
        }
    }

    public void SuspendDoor()
    {
        //Hold door in the air
        if (doorState == DoorState.Rising)
        {
            doorState = DoorState.Suspended;
            Invoke("DropDoor", dropWaitTime);
        }
    }

    public void DropDoor()
    {
        //Change door to falling and enable triggers
        audioSource.clip = fallSound;
        audioSource.Play();
        doorState = DoorState.DeadlyFalling;
        doorRigidbody.useGravity = true;
        doorCollider.isTrigger = true;

    }

    public void CheckDoorPosition()
    {
        //If the door has fallen a certain point, it shouldn't kill the player
        //This turns the door into a collider to stop the player from walking past as opposed to killing them
        if (doorState == DoorState.DeadlyFalling && doorMaxHeight - doorTransform.position.y >= (doorMaxHeight - doorStartPos.y) * 0.6)
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
            //LevelManager.death("DoorDeath");
            //LevelManager.respawn();
            doorTransform.position = doorStartPos;
            doorCollider.isTrigger = false;
            StartRising();
        }  
    }

    //Initiates the raising of the door
    public void StartRising()
    {
        audioSource.clip = riseSound;
        audioSource.Play();
        doorRigidbody.useGravity = false;
        doorState = DoorState.Rising;
    }

}
