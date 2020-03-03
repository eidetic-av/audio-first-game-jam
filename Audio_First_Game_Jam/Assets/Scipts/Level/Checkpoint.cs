using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameMaster currentGameMaster;

    void Start()
    {
        currentGameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("THIS IS WORKING");
            currentGameMaster.lastCheckpointPosition = transform.position;
        }
    }
}
