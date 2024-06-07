using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class CameraSwitchController : MonoBehaviour
{
    public GameObject frontCamera;
    public GameObject backCamera;
    [HideInInspector]
    public GameObject currentCamera;
    public bool useFront = true;

    public void Start()
    {
        currentCamera = useFront ? frontCamera : backCamera;
    }

    private void SwitchCamera()
    {
        if (useFront)
        {
            frontCamera.SetActive(true);
            backCamera.SetActive(false);
        }
        else
        {
            frontCamera.SetActive(false);
            backCamera.SetActive(true);
        }
    }

    // switch cameras' state
    public void ToggleCamera(Transform transform)
    {
        currentCamera = useFront ? backCamera : frontCamera;
        useFront = !useFront;
        currentCamera.GetComponent<CameraFollow>().SetTarget(transform);
        SwitchCamera();
    }
}
