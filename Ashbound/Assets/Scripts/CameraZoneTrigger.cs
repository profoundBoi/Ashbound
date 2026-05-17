using Unity.Cinemachine;
using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera cameraToActivate;
    public CinemachineCamera defaultCamera;

    [Header("Priority Settings")]
    public int activePriority = 20;
    public int defaultPriority = 10;

    private bool playerInside = false;

    private void Start()
    {
        cameraToActivate.Priority = 0;
        defaultCamera.Priority = defaultPriority;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || playerInside)
            return;

        playerInside = true;

        cameraToActivate.Priority = activePriority;

        defaultCamera.Priority = defaultPriority - 1;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        cameraToActivate.Priority = 0;
        defaultCamera.Priority = defaultPriority;
    }
}
