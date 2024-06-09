using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemDeactive : MonoBehaviour
{
    public GameObject[] objectsToDisable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
            Destroy(gameObject);
        }
    }
}
