using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovementController : MonoBehaviour
{
    public static bool IsMapOpened { get; private set; } = false; // detect whether map is opened

    [Header("Cameras")]
    public Camera mainCamera1;
    public Camera mainCamera2;
    public float zoomOutSize;
    public Transform mapFrontView;
    public Transform mapBackView;
    private Transform currentView;

    public float transitionDuration = 1f; // duration of the transition animation
    private Coroutine transitionCoroutine;

    private Camera activeCamera;
    private CameraFollow activeCameraFollow;

    void Start()
    {
        UpdateActiveCamera();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    private void UpdateActiveCamera()
    {
        // front camera
        if (mainCamera1.isActiveAndEnabled)
        {
            activeCamera = mainCamera1;
            activeCameraFollow = mainCamera1.GetComponent<CameraFollow>();
            currentView = mapFrontView;
        }
        // back camera
        else if (mainCamera2.isActiveAndEnabled)
        {
            activeCamera = mainCamera2;
            activeCameraFollow = mainCamera2.GetComponent<CameraFollow>();
            currentView = mapBackView;
        }
    }

    private void ToggleMap()
    {
        IsMapOpened = !IsMapOpened;

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        if (IsMapOpened)
        {
            UpdateActiveCamera();
            activeCameraFollow.enabled = false;
            transitionCoroutine = StartCoroutine(TransitionCamera(currentView.position, currentView.rotation, zoomOutSize));
        }
        else
        {
            activeCameraFollow.enabled = true;
            //transitionCoroutine = StartCoroutine(TransitionCamera(activeCameraFollow.target.position + activeCameraFollow.offset, 
            //    activeCameraFollow.target.rotation, activeCameraFollow.GetComponent<Camera>().orthographicSize));

            Quaternion targetRotation = activeCameraFollow.target.rotation;
            // if back camera is active, rotate 180 degrees.
            if (mainCamera2.isActiveAndEnabled)
            {
                Debug.Log("99999");
                targetRotation *= Quaternion.Euler(0, 180, 0);
            }
            transitionCoroutine = StartCoroutine(TransitionCamera(activeCameraFollow.target.position + activeCameraFollow.offset,
               targetRotation, activeCameraFollow.GetComponent<Camera>().orthographicSize));
            //transitionCoroutine = StartCoroutine(TransitionCamera(targetPosition, targetRotation, 
            //    activeCameraFollow.GetComponent<Camera>().orthographicSize));
        }
    }

    private IEnumerator TransitionCamera(Vector3 targetPosition, Quaternion targetRotation, float targetSize)
    {
        Vector3 startPosition = activeCamera.transform.position;
        Quaternion startRotation = activeCamera.transform.rotation;
        float startSize = activeCamera.orthographicSize;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            activeCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            activeCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            activeCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        activeCamera.transform.position = targetPosition;
        activeCamera.transform.rotation = targetRotation;
        activeCamera.orthographicSize = targetSize;
    }
}
