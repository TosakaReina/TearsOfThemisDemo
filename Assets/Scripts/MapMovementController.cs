using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovementController : MonoBehaviour
{

    [Header("Camera")]
    public Camera mainCamera;
    public float zoomOutSize;
    public Vector3 mapViewPosition; // view position after map toggle(Zoom out)
    [HideInInspector]
    public Vector3 originalCameraPosition;
    public float originalCameraSize;

    private bool isMapOpen = false;
    private bool isBlockSelected = false;
    private Vector3Int selectedBlockPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalCameraPosition = mainCamera.transform.position;
        originalCameraSize = mainCamera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleMap()
    {
        isMapOpen = !isMapOpen;
        if (isMapOpen)
        {
            mainCamera.transform.position = mapViewPosition;
            mainCamera.orthographicSize = zoomOutSize;
        }
        else // reset Camera perameters 
        {
            mainCamera.transform.position = originalCameraPosition;
            mainCamera.orthographicSize = originalCameraSize;
            isBlockSelected = false;
        }
    }
}
