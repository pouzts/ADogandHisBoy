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
    public RefValue<bool> FollowPlayer { get; set; } = new();
    public RefValue<bool> StandHere { get; set; } = new();

    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();

        StateMachine.AddState(new IdleState("IdleState", this));
        StateMachine.AddState(new FollowState("FollowState", this));
        StateMachine.AddState(new StandHereState("StandHereState", this));

        StateMachine.AddTransition(StateMachine.GetState("IdleState"), new Transition(new Condition<float>(distanceFromPlayer, Predicate.GreaterOrEqual, minDistanceFromPlayer), new Condition<bool>(FollowPlayer, Predicate.Equal, true), new Condition<bool>(StandHere, Predicate.Equal, false)), StateMachine.GetState("FollowState"));
        StateMachine.AddTransition(StateMachine.GetState("FollowState"), new Transition(new Condition<float>(distanceFromPlayer, Predicate.Less, minDistanceFromPlayer)), StateMachine.GetState("IdleState"));

        StateMachine.AddTransition(StateMachine.GetState("IdleState"), new Transition(new Condition<bool>(StandHere, Predicate.Equal, true)), StateMachine.GetState("StandHereState"));
        StateMachine.AddTransition(StateMachine.GetState("FollowState"), new Transition(new Condition<bool>(StandHere, Predicate.Equal, true)), StateMachine.GetState("StandHereState"));
        //StateMachine.AddTransition(StateMachine.GetState("StandHereState"), new Transition(new Condition<bool>(standHere, Predicate.Equal, false)), StateMachine.GetState("Idle"));

        StateMachine.SetState(StateMachine.GetState("IdleState"));
    }

    private void Update()
    {
        if (NavMeshAgent == null)
            return;

        distanceFromPlayer.value = Vector3.Distance(transform.position, player.transform.position);

        StateMachine.OnUpdate();
    }
}
