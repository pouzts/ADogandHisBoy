using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private float minDistanceFromPlayer = 3f;
    [SerializeField] private RefValue<bool> followPlayer = new();
    
    public NavMeshAgent NavMeshAgent { get; set; }

    private readonly StateMachine stateMachine = new();
    private readonly RefValue<float> distanceFromPlayer = new();

    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine.AddState(new IdleState("IdleState", this));
        stateMachine.AddState(new FollowState("FollowState", this));

        stateMachine.AddTransition(stateMachine.GetState("IdleState"), new Transition(new Condition<float>(distanceFromPlayer, Predicate.GreaterOrEqual, minDistanceFromPlayer), new Condition<bool>(followPlayer, Predicate.Equal, true)), stateMachine.GetState("FollowState"));
        stateMachine.AddTransition(stateMachine.GetState("FollowState"), new Transition(new Condition<float>(distanceFromPlayer, Predicate.Less, minDistanceFromPlayer)), stateMachine.GetState("IdleState"));

        stateMachine.SetState(stateMachine.GetState("IdleState"));
    }

    private void Update()
    {
        if (NavMeshAgent == null)
            return;

        distanceFromPlayer.value = Vector3.Distance(transform.position, player.transform.position);
        stateMachine.OnUpdate();
    }
}
