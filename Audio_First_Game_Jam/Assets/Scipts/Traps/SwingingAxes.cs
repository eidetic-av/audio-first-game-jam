using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxes : MonoBehaviour
{
    private bool trapIsActive;
    public GameObject axeToTrigger;
    private Animator animationToTrigger;


    // Start is called before the first frame update
    void Start()
    {
        animationToTrigger = axeToTrigger.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(trapIsActive);
        Debug.Log(animationToTrigger.GetBool("isTriggered"));
        if (trapIsActive)
        {
            animationToTrigger.SetBool("isTriggered", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            trapIsActive = true;
        }
    }
}
