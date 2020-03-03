using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameMaster gameMaster;
    private LevelManager levelManager;
    public GameObject playerObj;
    public AudioClip boulderDeathSound;
    public AudioClip doorDeathSound;
    public AudioClip bridgeDeathSound;
    public AudioClip bladesDeathSound;
    public AudioClip genericDeathSound;
    private AudioSource soundForPlayer;


    // Start is called before the first frame update
    void Start()
    {
        levelManager = this;
        gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        soundForPlayer = playerObj.AddComponent<AudioSource>();
        soundForPlayer.volume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //Condition to respawn the player
        if (playerObj.transform.position.y < 0)
        {
            soundForPlayer.clip = genericDeathSound;
            soundForPlayer.Play();
            Respawn();
        }
    }

    public void Respawn()
    {
        playerObj.transform.position = gameMaster.lastCheckpointPosition;
    }
}
