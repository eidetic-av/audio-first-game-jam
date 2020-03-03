using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyFloor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player_Movement.inWeb = true;
    }
}
