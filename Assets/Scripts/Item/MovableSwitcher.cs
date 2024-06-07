using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableSwitcher : MonoBehaviour
{
    public MapBlock mapBlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || mapBlock != null)
        {
            mapBlock.SwitchMaterial();
            mapBlock.movableSwitched = true;
            Destroy(gameObject);
        }
    }
}
