using UnityEngine;

public class CombatController : MonoBehaviour
{
    EnemyController targetEnemy;
    public EnemyController TargetEnemy
    {
        get => targetEnemy;
        set
        {
            targetEnemy = value;
            if (targetEnemy == null)
                combatMode = false;
        }
    }

    bool combatMode;

    public bool CombatMode
    {
        get => combatMode;
        set
        {
            combatMode = value;

            if (targetEnemy == null)
                combatMode = false; // 타겟이 없으면 전투 모드 비활성화

            animator.SetBool("combatMode", combatMode);
        }
    }

    // 근접 전투 시스템 참조
    MeleeFighter meleeFighter;
    Animator animator;
    CameraController cam;

    public void Awake()
    {
        meleeFighter = GetComponent<MeleeFighter>();
        animator = GetComponent<Animator>();
        cam = Camera.main.GetComponent<CameraController>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var enemy = EnemyManager.i.GetAttackingEnemy();
            if (enemy != null && enemy.Fighter.isCounterable && !meleeFighter.inAction)
            {
                StartCoroutine(meleeFighter.PerformCounterAttack(enemy));
            }
            else
            {
                var enemyToAttack = EnemyManager.i.GetClosestEnemyToDirection(PlayerController.i.InputDir);
                Vector3? dirToAttack = null;
                if (enemyToAttack != null)
                    dirToAttack = enemyToAttack.transform.position - transform.position;

                meleeFighter.TryToAttack(dirToAttack);
                CombatMode = true; // 공격 시 전투 모드 활성화
            }
        }

        if (Input.GetButtonDown("LockOn"))
        {
            CombatMode = !CombatMode;
        }
    }

    private void OnAnimatorMove()
    {
        if (!meleeFighter.inCounter)
        {
            transform.position += animator.deltaPosition;
        }

        transform.rotation *= animator.deltaRotation;
    }

    public Vector3 GetTargetingDir()
    {
        if(!CombatMode)
        {
        var vecFromCam = transform.position - cam.transform.position;
        vecFromCam.y = 0f;
        return vecFromCam.normalized;
        }
        else
        {
            return transform.forward;
        }
    }
}
