using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlockController : MonoBehaviour
{
    public static MapBlockController instance;

    private MapBlock currentSelectedBlock;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanSelectBlock(MapBlock block)
    {
        return currentSelectedBlock == null || currentSelectedBlock == block;
    }

    public void SelectBlock(MapBlock block)
    {
        currentSelectedBlock = block;
    }

    public void DeselectBlock(MapBlock block)
    {
        if (currentSelectedBlock == block)
        {
            currentSelectedBlock = null;
        }
    }
}
