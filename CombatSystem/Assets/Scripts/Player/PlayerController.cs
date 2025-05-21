using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 이동 속도
    [SerializeField] float moveSpeed = 5f;
    // 회전 속도
    [SerializeField] float rotationSpeed = 500f;

    [Header("그라운드 체크 설정")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    
    bool isGrounded;

    // 목표 회전값
    Quaternion targetRotation;
    float ySpeed;

    // 참조
    CameraController cameraController;
    Animator animator;
    CharacterController characterController;
    MeleeFighter meleeFighter;

    private void Awake()
    {
        // 메인 카메라에서 CameraController 컴포넌트 가져오기
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        meleeFighter = GetComponent<MeleeFighter>();
    }

    void Update()
    {
        if (meleeFighter.inAction)
        {
            animator.SetFloat("moveAmount", 0f);
            return;
        }

        // 수평, 수직 입력값 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 전체 이동량 계산
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        // 입력 방향 정규화
        var moveInput = (new Vector3(h, 0, v)).normalized;

        // 카메라 방향을 기준으로 이동 방향 계산
        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();

        if (isGrounded)
        {
            ySpeed = -0.5f; // 바닥에 닿아있을 때 ySpeed를 약간 음수로 설정
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);


        // 이동 입력이 있을 때만 처리
        if (moveAmount > 0)
        {
            // 이동 방향으로 회전 목표 설정
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        // 부드러운 회전 처리
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}