using UnityEngine;

public enum AICombatStates {  Idle, Chase, Circling}

public class CombatMovementState : State<EnemyController>
{
    [SerializeField] float circlingSpeed = 20f;
    [SerializeField] float distanceToStand = 3f;
    [SerializeField] float adjustDistanceThreshold = 1f;
    [SerializeField] Vector2 idleTimeRange = new Vector2(2, 5);
    [SerializeField] Vector2 circlingTimeRange = new Vector2(3, 6);

    float timer = 0f;

    int circlingDir = 1;

    AICombatStates state;

    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;

        enemy.NavAgent.stoppingDistance = distanceToStand;
    }
    public override void Execute()
    {
        if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) > distanceToStand + adjustDistanceThreshold)
        {
            StartChase();
        }


        if (state == AICombatStates.Idle)
        {
            if(timer <= 0)
            {
                if (Random.Range(0, 2)==0)
                {
                    StartIdle();
                }
                else
                {
                    StartCircling();
                }
            }
        }
        else if (state == AICombatStates.Chase)
        {
            if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) <= distanceToStand + +0.03f)
            {
                StartIdle();
                return;
            }
            enemy.NavAgent.SetDestination(enemy.Target.transform.position);
        }
        else if (state == AICombatStates.Circling)
        {
            if(timer <= 0)
            {
                StartIdle();
                return;
            }
            transform.RotateAround(enemy.Target.transform.position, Vector3.up, circlingSpeed * circlingDir * Time.deltaTime);
        }

        if (timer > 0f)
            timer -= Time.deltaTime;
    }

     void StartCircling()
    {
        Debug.Log("Circling");
        state = AICombatStates.Circling;
        timer = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
        
        circlingDir = Random.Range(0, 2) == 0 ? 1 : -1;

        enemy.Anim.SetBool("circling", true);
        enemy.Anim.SetFloat("circlingDir", circlingDir);
    }

    void StartChase()
    {
        Debug.Log("Chase");
        state = AICombatStates.Chase;
        enemy.Anim.SetBool("combatMode", false);
        enemy.Anim.SetBool("circling", false);
    }

    void StartIdle()
    {
        Debug.Log("Idle");
        state = AICombatStates.Idle;
        timer = Random.Range(idleTimeRange.x, idleTimeRange.y);
        enemy.Anim.SetBool("combatMode", true);
        enemy.Anim.SetBool("circling", false);
    }

    public override void Exit()
    {

    }
}