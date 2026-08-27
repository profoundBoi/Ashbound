using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpineLean : MonoBehaviour
{
    Animator animator;
    Transform spineBone;

    [Header("Lean settings")]
    public float leanAngle = 5f;          // degrees added each time OnLean is called
    public float maxLeanAngle = 50f;      // cap so it can't lean infinitely
    public float leanSmoothSpeed = 5f;    // how fast the lean transitions
    [SerializeField]
    private Animation RunClip;

    [Header("Bone")]
    public HumanBodyBones leanBone = HumanBodyBones.Spine;

    float currentLean = 0f;
    float targetLean = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        spineBone = animator.GetBoneTransform(leanBone);

        if (spineBone == null)
        {
            Debug.LogWarning($"SpineLean: no bone found for {leanBone} — check Avatar is Humanoid and correctly mapped.");
        }
    }

    void LateUpdate()
    {
        if (spineBone == null) return;

        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSmoothSpeed);
        spineBone.localRotation *= Quaternion.Euler(currentLean, 0f, 0f);
    }

    // Call this to add another leanAngle worth of forward lean, up to maxLeanAngle
    public void OnLean()
    {
        targetLean = Mathf.Clamp(targetLean + leanAngle, 0f, maxLeanAngle);
        animator.speed += 0.1f;
    }

    // Call this to remove one leanAngle worth of lean (or fully reset if you prefer)
    public void ResetLean()
    {
        targetLean = Mathf.Clamp(targetLean - leanAngle, 0f, maxLeanAngle);
        animator.speed -= 0.1f;
    }
}