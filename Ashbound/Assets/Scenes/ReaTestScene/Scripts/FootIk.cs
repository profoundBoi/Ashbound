using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootIK : MonoBehaviour
{
    Animator animator;

    [Header("Raycast settings")]
    public LayerMask groundLayer;
    public float raycastHeight = 1f;
    public float raycastDistance = 2f;
    public float footOffset = 0.05f;

    [Header("Weight falloff")]
    public float maxHeightForFullWeight = 0.05f;  // foot height above ground where IK = full weight
    public float heightForZeroWeight = 0.3f;      // foot height above ground where IK = 0 (fully in animation's control)
    public float weightSmoothSpeed = 10f;

    float leftWeight = 0f;
    float rightWeight = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;
        SolveFoot(AvatarIKGoal.LeftFoot, ref leftWeight);
        SolveFoot(AvatarIKGoal.RightFoot, ref rightWeight);
    }

    void SolveFoot(AvatarIKGoal foot, ref float currentWeight)
    {
        Vector3 animatedFootPos = animator.GetIKPosition(foot);
        Vector3 rayOrigin = animatedFootPos + Vector3.up * raycastHeight;

        float targetWeight = 0f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight + raycastDistance, groundLayer))
        {
            // how far is the ANIMATED foot currently above the actual ground?
            float heightAboveGround = animatedFootPos.y - hit.point.y;

            // map that height to a 0–1 weight: 
            // near ground (small height) -> weight near 1
            // lifted high (swing phase)  -> weight near 0
            targetWeight = 1f - Mathf.InverseLerp(maxHeightForFullWeight, heightForZeroWeight, heightAboveGround);
            targetWeight = Mathf.Clamp01(targetWeight);

            currentWeight = Mathf.Lerp(currentWeight, targetWeight, Time.deltaTime * weightSmoothSpeed);

            animator.SetIKPositionWeight(foot, currentWeight);
            animator.SetIKRotationWeight(foot, currentWeight);

            if (currentWeight > 0.01f)
            {
                Vector3 targetPos = hit.point + Vector3.up * footOffset;
                animator.SetIKPosition(foot, targetPos);

                Quaternion groundRot = Quaternion.FromToRotation(Vector3.up, hit.normal) * animator.GetIKRotation(foot);
                animator.SetIKRotation(foot, groundRot);
            }
        }
        else
        {
            currentWeight = Mathf.Lerp(currentWeight, 0f, Time.deltaTime * weightSmoothSpeed);
            animator.SetIKPositionWeight(foot, currentWeight);
            animator.SetIKRotationWeight(foot, currentWeight);
        }
    }
}