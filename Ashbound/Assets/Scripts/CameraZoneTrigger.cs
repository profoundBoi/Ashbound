using Unity.Cinemachine;
using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    public CinemachineCamera cameraToActivate;
    public CinemachineCamera defaultCamera;

    public int activePriority = 20;
    public int defaultPriority = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraToActivate.Priority = activePriority;
            defaultCamera.Priority = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraToActivate.Priority = 0;
            defaultCamera.Priority = defaultPriority;
        }
    }
}
