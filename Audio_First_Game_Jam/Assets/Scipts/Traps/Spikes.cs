using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public GameObject spikes;
    public float movementSpeed = 10f;
    private Vector3 originalSpikeLocation;
    private Vector3 endSpikeLocation;
    public float delayTime;
    public float moveSpikeUpBy = 1.0f;
    private bool isTrapActive = false;
    private bool isTrapUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        originalSpikeLocation = spikes.transform.position;
        endSpikeLocation = new Vector3 (originalSpikeLocation.x, (originalSpikeLocation.y + moveSpikeUpBy), originalSpikeLocation.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(isTrapActive && !isTrapUsed)
        {
            if (spikes.transform.position != endSpikeLocation)
                ChangeSpikePosition(endSpikeLocation);
        }
        else
        {
            if (spikes.transform.position != originalSpikeLocation)
            {
                Debug.Log(originalSpikeLocation);
                StartCoroutine(DelaySpikeDown());
                isTrapUsed = true;
            }
            else if (spikes.transform.position != endSpikeLocation && spikes.transform.position != originalSpikeLocation && !isTrapUsed)
            {
                Debug.Log(endSpikeLocation);
                ChangeSpikePosition(endSpikeLocation);

            }

        }
            
    }

    public IEnumerator DelaySpikeDown()
    {
        yield return new WaitForSeconds(delayTime);
        ChangeSpikePosition(originalSpikeLocation);
    }

    private void ChangeSpikePosition(Vector3 destinationPosition)
    {
        spikes.transform.position = Vector3.MoveTowards(spikes.transform.position, destinationPosition, movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTrapActive = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        isTrapActive = false;
    }
}
