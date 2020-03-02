using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTriggerScript : MonoBehaviour
{
    public BoulderRollingScript boulderRollingScript;

    private void OnTriggerEnter(Collider other)
    {
        //Detect player when they touch the pressure plate
        if (other.tag == "Player")
        {
            boulderRollingScript.StartRolling(transform.position.x, transform.position.z);
        }
    }

}
