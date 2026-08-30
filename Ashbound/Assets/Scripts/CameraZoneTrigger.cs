using Unity.Cinemachine;
using UnityEngine;

public class CameraZoneTrigger : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera cameraToActivate;   // the zone's Cinemachine camera
    public GameObject defaultCamera;             // can be a CinemachineCamera OR a plain Camera

    [Header("Priority Settings")]
    public int activePriority = 20;
    public int defaultPriority = 10;

    private bool playerInside = false;
    private CinemachineCamera defaultCineCam;
    private Camera defaultPlainCam;

    private void Start()
    {
        cameraToActivate.Priority = 0;

        // Figure out what kind of default camera we were given
        defaultCineCam = defaultCamera.GetComponent<CinemachineCamera>();
        defaultPlainCam = defaultCamera.GetComponent<Camera>();

        if (defaultCineCam != null)
        {
            defaultCineCam.Priority = defaultPriority;
        }
        else if (defaultPlainCam != null)
        {
            defaultPlainCam.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || playerInside)
            return;

        playerInside = true;
        cameraToActivate.Priority = activePriority;

        if (defaultCineCam != null)
        {
            defaultCineCam.Priority = defaultPriority - 1;
        }
        else if (defaultPlainCam != null)
        {
            defaultPlainCam.enabled = false; // let Cinemachine's brain-driven camera take over
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;
        cameraToActivate.Priority = 0;

        if (defaultCineCam != null)
        {
            defaultCineCam.Priority = defaultPriority;
        }
        else if (defaultPlainCam != null)
        {
            defaultPlainCam.enabled = true;
        }
    }
}