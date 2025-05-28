using System.Collections;
using UnityEngine;

public class GettingHitState : State<EnemyController>
{
    [SerializeField] float stunTime = 0.5f;
    EnemyController enemy;

    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        enemy.Fighter.OnHitComplete += () => StartCoroutine(GoToCombatMovement());
    }

    IEnumerator GoToCombatMovement()
    {
        yield return new WaitForSeconds(stunTime);
        enemy.ChangeState(EnemyStates.CombatMovement);
    }
}
