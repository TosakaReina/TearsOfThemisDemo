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
    private bool isMapOpened = false;


    // maximum move distance respectively
    [Header("Movement Limitation")]
    public float upMaxMoveDistance = 5f; 
    public float downMaxMoveDistance = 5f; 
    public float leftMaxMoveDistance = 5f; 
    public float rightMaxMoveDistance = 5f; 
    private Vector3 originalPosition; 

    private void Start()
    {
        GetTargetTransform(transform);
        originalPosition = transform.position; // record original position
    }

    private void Update()
    {
        // detect map opened
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isHighLighted)
            {
                isHighLighted = false;
                isMovable = false;
                stopHighlight();
            }

            isMapOpened = !isMapOpened;
        }

        // detect block selected
        if (Input.GetKeyDown(mapCode))
        {
            isMovable = !isMovable;
            if (isHighLighted)
            {
                stopHighlight();
                isHighLighted = false;
            }
            Debug.Log(isHighLighted);
        }

        if (isMovable && isMapOpened)
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

            transform.position = targetPosition;
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

    public void stopHighlight()
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
}
