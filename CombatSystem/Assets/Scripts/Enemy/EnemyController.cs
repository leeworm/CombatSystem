using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyStates { Idle, CombatMovement }
public class EnemyController : MonoBehaviour
{
    [field:SerializeField] public float Fov { get; private set; } = 180f;
    public List<MeleeFighter> TargetsInRange { get; private set; } = new List<MeleeFighter>();
    public MeleeFighter Target { get; set; }
    public StateMachine<EnemyController> StateMachine { get; private set; }

    Dictionary<EnemyStates, State<EnemyController>> stateDict;

    public NavMeshAgent NavAgent { get; private set; }

    public Animator Anim { get; private set; }

    private void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();

        stateDict = new Dictionary<EnemyStates, State<EnemyController>>();
        stateDict[EnemyStates.Idle] = GetComponent<IdleState>();
        stateDict[EnemyStates.CombatMovement] = GetComponent<CombatMovementState>();

        StateMachine = new StateMachine<EnemyController>(this);
        StateMachine.ChangeState(stateDict[EnemyStates.Idle]);
    }

    public void ChangeState(EnemyStates state)
    {
        StateMachine.ChangeState(stateDict[state]);
    }

    private void Update()
    {
        StateMachine.Execute();
        Anim.SetFloat("moveAmount", NavAgent.velocity.magnitude / NavAgent.speed);
    }
}
