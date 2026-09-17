using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WallRunHandIK : MonoBehaviour
{
    Animator animator;
    Transform spineBone;

    [Header("Wall Detection")]
    public LayerMask wallLayer;
    public float reachDistance = 1f;     // how far out to check for a wall
    public float shoulderHeight = 1.3f;  // approx height to cast from (local up offset)

    [Header("Hand Blend")]
    [Range(0, 1)] public float maxReachWeight = 0.85f;
    public float handWeightSmoothSpeed = 8f;

    [Header("Hand Offset")]
    public float palmOffset = 0.03f; // pushes hand slightly off the wall surface to avoid clipping

    [Header("Spine Lean")]
    public HumanBodyBones leanBone = HumanBodyBones.Spine;
    public float maxLeanAngle = 20f;      // degrees to lean when wall running
    public float leanSmoothSpeed = 8f;    // how fast the lean transitions

    float currentHandWeight = 0f;
    float currentLean = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spineBone = animator.GetBoneTransform(leanBone);

        if (spineBone == null)
        {
            Debug.LogWarning($"WallRunHandIK: no bone found for {leanBone} — check Avatar is Humanoid and correctly mapped.");
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        Vector3 origin = transform.position + Vector3.up * shoulderHeight;
        bool wallDetected = Physics.Raycast(origin, -transform.right, out RaycastHit hit, reachDistance, wallLayer);

        // --- Hand IK ---
        float targetHandWeight = wallDetected ? maxReachWeight : 0f;
        currentHandWeight = Mathf.Lerp(currentHandWeight, targetHandWeight, Time.deltaTime * handWeightSmoothSpeed);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, currentHandWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, currentHandWeight);

        if (wallDetected)
        {
            Vector3 targetPos = hit.point + hit.normal * palmOffset;
            animator.SetIKPosition(AvatarIKGoal.LeftHand, targetPos);

            Quaternion palmRotation = Quaternion.LookRotation(-hit.normal, transform.up);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, palmRotation);
        }

        // --- Spine Lean ---
        float targetLean = wallDetected ? -maxLeanAngle : 0f; // negative = lean toward left wall, flip sign if it leans the wrong way
        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSmoothSpeed);
    }

    void LateUpdate()
    {
        // applied in LateUpdate, after Animator has posed the spine this frame
        if (spineBone == null) return;
        spineBone.localRotation *= Quaternion.Euler(0f, 0f, currentLean);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * shoulderHeight;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(origin, -transform.right * reachDistance);
    }
}