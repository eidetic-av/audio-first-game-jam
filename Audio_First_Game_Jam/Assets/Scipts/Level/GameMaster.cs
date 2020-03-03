using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public Vector3 lastCheckpointPosition; 

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        else
            Destroy(gameObject);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
