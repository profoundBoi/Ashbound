using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InputUnlockTrigger : MonoBehaviour
{
    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        var inputLock = other.GetComponentInParent<PlayerInputLock>();
        if (inputLock != null && inputLock.IsLocked)
        {
            inputLock.Unlock();
        }
    }
}