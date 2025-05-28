using System.Collections;
using UnityEngine;

public class AttackState : State<EnemyController>
{

    [SerializeField] float attackDistance = 1f;

    bool isAttacking;
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;

        enemy.NavAgent.stoppingDistance = attackDistance;
    }

    public override void Execute()
    {
        if (isAttacking)
            return;


        enemy.NavAgent.SetDestination(enemy.Target.transform.position);

        if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) <= attackDistance + 0.03f)
        {
            StartCoroutine(Attack(Random.Range(1, enemy.Fighter.Attacks.Count + 1)));
        }

    }

    IEnumerator Attack(int comboCount = 1)
    {
        isAttacking = true;
        enemy.Anim.applyRootMotion = true;

        Debug.Log($"Starting combo attack sequence. Total combos: {comboCount}");
        enemy.Fighter.TryToAttack();

        for (int i = 1; i <= comboCount; i++)
        {



            yield return new WaitUntil(() => enemy.Fighter.attackState == EAttackState.Cooldown);
            enemy.Fighter.TryToAttack();


        }


        yield return new WaitUntil(() => enemy.Fighter.attackState == EAttackState.Idle);

        // 마지막 공격 후 충분한 대기 시간 추가
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Attack sequence complete");
        enemy.Anim.applyRootMotion = false;
        isAttacking = false;

        if (enemy.IsInState(EnemyStates.Attack))
            enemy.ChangeState(EnemyStates.RetreatAfterAttack);
    }

    public override void Exit()
    {
        enemy.NavAgent.ResetPath();
    }



}