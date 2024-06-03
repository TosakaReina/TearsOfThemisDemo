using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovementController : MonoBehaviour
{

    [Header("Camera")]
    public Camera mainCamera;
    public float zoomOutSize;
    public GameObject mapView;

    [HideInInspector]
    public Vector3 originalCameraPosition;
    [HideInInspector]
    public Quaternion originalCameraRotation;
    public float originalCameraSize;

    public float transitionDuration = 1f; // duration of the transition animation

    private bool isMapOpen = false;
    private Coroutine transitionCoroutine;

    void Start()
    {
        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;
        originalCameraSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            Debug.Log("Toggle Map");
            ToggleMap();
        }
    }

    private void ToggleMap()
    {
        isMapOpen = !isMapOpen;

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        if (isMapOpen)
        {
            transitionCoroutine = StartCoroutine(TransitionCamera(mapView.transform.position, mapView.transform.rotation, zoomOutSize));
        }
        else
        {
            transitionCoroutine = StartCoroutine(TransitionCamera(originalCameraPosition, originalCameraRotation, originalCameraSize));
        }
    }

    private IEnumerator TransitionCamera(Vector3 targetPosition, Quaternion targetRotation, float targetSize)
    {
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        float startSize = mainCamera.orthographicSize;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
        mainCamera.orthographicSize = targetSize;
    }
}
