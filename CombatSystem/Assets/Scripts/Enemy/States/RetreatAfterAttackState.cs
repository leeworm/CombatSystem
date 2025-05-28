using UnityEngine;

public class RetreatAfterAttackState : State<EnemyController>
{
    [SerializeField] float backwardWalkSpeed = 1.5f;
    [SerializeField] float distanceToRetreat = 3f;

    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
    }

    public override void Execute()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.Target.transform.position) >= distanceToRetreat)
        {
            enemy.ChangeState(EnemyStates.CombatMovement);
            return;
        }



        var vecToTarget = enemy.Target.transform.position - enemy.transform.position;
        enemy.NavAgent.Move(-vecToTarget.normalized * backwardWalkSpeed * Time.deltaTime);


        vecToTarget.y = 0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(vecToTarget), 500 * Time.deltaTime);
    }

}