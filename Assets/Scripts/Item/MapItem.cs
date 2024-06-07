using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem : MonoBehaviour
{
    public GameObject[] objectsToEnable; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true); 
            }
            Destroy(gameObject);
        }
    }
}
