using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighting : MonoBehaviour
{
    public GameObject highlightedGameobject;
    public Material newMaterial;

    private List<Transform> targetTransforms = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        GetTargetTransform(highlightedGameobject.transform); // get all tilemap block children
    }

    private void GetTargetTransform(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("Tilemap"))
            {
                targetTransforms.Add(child);
            }
        }
    }

    // change to movable block material
    public void SwitchMaterial()
    {
        foreach (Transform child in targetTransforms)
        {
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();
            renderer.material = newMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchMaterial();
        }
    }
}
