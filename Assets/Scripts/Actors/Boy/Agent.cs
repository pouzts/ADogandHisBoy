using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public Player player;

    [SerializeField] private float minDistanceFromPlayer = 3f;
    
    public NavMeshAgent NavMeshAgent { get; set; }
    public StateMachine StateMachine { get; set; } = new();

    private readonly RefValue<float> distanceFromPlayer = new();
    private readonly RefValue<bool> followPlayer = new();
    private readonly RefValue<bool> standHere = new();

    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();

        StateMachine.AddState(new IdleState("IdleState", this));
        StateMachine.AddState(new FollowState("FollowState", this));
        StateMachine.AddState(new StandHereState("StandHereState", this));

        StateMachine.AddTransition(StateMachine.GetState("IdleState"), new Transition(new Condition<float>(distanceFromPlayer, Predicate.GreaterOrEqual, minDistanceFromPlayer), new Condition<bool>(followPlayer, Predicate.Equal, true), new Condition<bool>(standHere, Predicate.Equal, false)), StateMachine.GetState("FollowState"));
        StateMachine.AddTransition(StateMachine.GetState("FollowState"), new Transition(new Condition<float>(distanceFromPlayer, Predicate.Less, minDistanceFromPlayer)), StateMachine.GetState("IdleState"));

        StateMachine.AddTransition(StateMachine.GetState("IdleState"), new Transition(new Condition<bool>(standHere, Predicate.Equal, true)), StateMachine.GetState("StandHereState"));
        StateMachine.AddTransition(StateMachine.GetState("FollowState"), new Transition(new Condition<bool>(standHere, Predicate.Equal, true)), StateMachine.GetState("StandHereState"));

        StateMachine.SetState(StateMachine.GetState("IdleState"));
    }

    private void Update()
    {
        if (NavMeshAgent == null)
            return;

        distanceFromPlayer.value = Vector3.Distance(transform.position, player.transform.position);
        followPlayer.value = player.FollowPlayer;

        print(distanceFromPlayer.value);
        print(followPlayer.value);

        StateMachine.OnUpdate();
    }
}
