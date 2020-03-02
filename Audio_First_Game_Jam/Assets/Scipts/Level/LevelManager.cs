using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameMaster gameMaster;
    private LevelManager levelManager;
    public GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = this;
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        //Condition to respawn the player
        if (playerObj.transform.position.y < 0)
            Respawn();
    }

    public void Respawn()
    {
        playerObj.transform.position = gameMaster.lastCheckpointPosition;
    }
}
