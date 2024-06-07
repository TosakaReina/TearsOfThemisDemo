using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBlock : MonoBehaviour
{
    public KeyCode mapCode;
    public Color emissionColor;
    private Color originalColor;

    private List<Transform> targetTransforms = new List<Transform>();
    private bool isMovable;
    private bool isHighLighted = false;

    // maximum move distance respectively
    [Header("Movement Limitation")]
    public float upMaxMoveDistance = 5f; 
    public float downMaxMoveDistance = 5f; 
    public float leftMaxMoveDistance = 5f; 
    public float rightMaxMoveDistance = 5f; 

    private Vector3 originalPosition;
    private List<Transform> itemsTransforms = new List<Transform>(); // track all players entering

    [Header("Switch Material")]
    public Material newMaterial;
    public bool movableSwitched = true;

    private void Start()
    {
        GetTargetTransform(transform); // get all tilemap block children
        originalPosition = transform.position; // record original position
    }

    private void Update()
    {
        // detect whether have switched to movable map block
        if (movableSwitched)
        {
            // detect map opened
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (isHighLighted)
                {
                    isHighLighted = false;
                    isMovable = false;
                    StopHighlight();
                    MapBlockController.instance.DeselectBlock(this);
                }
            }

            // detect block selected
            if (Input.GetKeyDown(mapCode) && MapBlockController.instance.CanSelectBlock(this))
            {
                isMovable = !isMovable;
                if (isHighLighted)
                {
                    StopHighlight();
                    isHighLighted = false;
                    MapBlockController.instance.DeselectBlock(this);
                }
                else
                {
                    MapBlockController.instance.SelectBlock(this);
                }
            }

            if (isMovable && MapMovementController.IsMapOpened)
            {
                if (!isHighLighted)
                {
                    HighlightSelectedBlock();
                    isHighLighted = true;
                }

                Vector3 targetPosition = transform.position;

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    targetPosition += Vector3.down;
                    if (originalPosition.y - targetPosition.y > downMaxMoveDistance)
                    {
                        targetPosition = transform.position; // if exceeding the limit, restore to original position
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    targetPosition += Vector3.up;
                    if (targetPosition.y - originalPosition.y > upMaxMoveDistance)
                    {
                        targetPosition = transform.position;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    targetPosition += Vector3.left;
                    if (originalPosition.x - targetPosition.x > leftMaxMoveDistance)
                    {
                        targetPosition = transform.position;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    targetPosition += Vector3.right;
                    if (targetPosition.x - originalPosition.x > rightMaxMoveDistance)
                    {
                        targetPosition = transform.position;
                    }
                }

                Vector3 moveDelta = targetPosition - transform.position;

                // move MapBlock, items and players simultaneously
                transform.position = targetPosition;
                foreach (var item in itemsTransforms)
                {
                    item.position += moveDelta;
                }
            }
        }
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

    // selected
    public void HighlightSelectedBlock()
    {
        if (targetTransforms == null)
        {
            return;
        }

        foreach (Transform child in targetTransforms)
        {
            Material material = child.GetComponent<MeshRenderer>().material;
            // ensure emission attribute
            if (material.HasProperty("_EmissionColor"))
            {
                material.SetColor("_EmissionColor", emissionColor); // set color
                material.EnableKeyword("_EMISSION"); // set active
            }
            else
            {
                Debug.LogWarning("Not Support Emission");
            }
        }
    }

    // deselected
    public void StopHighlight()
    {
        foreach (Transform child in targetTransforms)
        {
            Material material = child.GetComponent<MeshRenderer>().material;
            if (material.HasProperty("_EmissionColor"))
            {
                material.DisableKeyword("_EMISSION");
            }
            else
            {
                Debug.LogWarning("Not Support Emission");
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
        if (other.CompareTag("Player") || other.CompareTag("Item"))
        {
            if (!itemsTransforms.Contains(other.transform))
            {
                itemsTransforms.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Item"))
        {
            if (itemsTransforms.Contains(other.transform))
            {
                itemsTransforms.Remove(other.transform);
            }
        }
    }
}
