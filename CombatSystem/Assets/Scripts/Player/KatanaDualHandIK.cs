using UnityEngine;

[RequireComponent(typeof(Animator))]
public class KatanaDualHandIK : MonoBehaviour
{
    public Transform rightHandIKTarget;   // 오른손 IK 목표 (카타나 끝)
    public Transform leftHandIKTarget;    // 왼손 IK 목표 (카타나 중간)

    [Range(0f, 1f)] public float rightHandIKWeight = 1f;
    [Range(0f, 1f)] public float leftHandIKWeight = 0f; // 처음에는 0

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null)
            return;

        // 오른손 IK
        if (rightHandIKTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTarget.rotation);
        }

        // 왼손 IK
        if (leftHandIKTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        }
    }
}