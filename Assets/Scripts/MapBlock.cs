using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TilemapEditor25D;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MapBlock : MonoBehaviour
{
    public bool isMovable;
    public Color emissionColor = Color.white; // 要设置的Emission颜色
    private Color originalColor;

    private List<Transform> targetTransforms = new List<Transform>();
    private bool isHighLighted = false;



    private void Start()
    {
        GetTargetTransform(transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMovable = !isMovable;
            if (isHighLighted) 
            { 
                stopHighlight();
                isHighLighted = false;
            }
            Debug.Log(isHighLighted);
        }

        if (isMovable)
        {
            if (!isHighLighted)
            {
                HighlightSelectedBlock();
                isHighLighted = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gameObject.transform.position += Vector3.down;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gameObject.transform.position += Vector3.up;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gameObject.transform.position += Vector3.left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                gameObject.transform.position += Vector3.right;
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

    public void HighlightSelectedBlock()
    {
        if (targetTransforms == null)
        {
            return;
        }

        foreach (Transform child in targetTransforms)
        {
            Material material = child.GetComponent<MeshRenderer>().material;
            // 确保材质具有Emission属性
            if (material.HasProperty("_EmissionColor"))
            {
                // 设置Emission颜色
                material.SetColor("_EmissionColor", emissionColor);

                // 启用Emission
                material.EnableKeyword("_EMISSION");
            }
            else
            {
                Debug.LogWarning("材质不支持Emission属性");
            }
        }
    }

    public void stopHighlight()
    {
        foreach (Transform child in targetTransforms)
        {
            Material material = child.GetComponent<MeshRenderer>().material;
            // 确保材质具有Emission属性
            if (material.HasProperty("_EmissionColor"))
            {

                material.DisableKeyword("_EMISSION");
            }
            else
            {
                Debug.LogWarning("材质不支持Emission属性");
            }
        }
    }
}

