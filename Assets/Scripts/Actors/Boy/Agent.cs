using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public Player player;

    [SerializeField] private float minDistanceFromPlayer = 3f;
    [SerializeField] private float lookDistance = 10f;
    [SerializeField] private float viewAngle = 45f;
    [SerializeField] private int numOfCasts = 5;
    [SerializeField] private LayerMask playerMask;
    
    public NavMeshAgent NavMeshAgent { get; set; }
    public StateMachine StateMachine { get; set; } = new();

    public bool PlayerInSite { get; set; } = false;

    public RefValue<bool> FollowPlayer { get; set; } = new();
    public RefValue<bool> StandHere { get; set; } = new();
    
    private readonly RefValue<float> distanceFromPlayer = new();

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

        StateMachine.SetState(StateMachine.GetState("IdleState"));
    }

    private void FixedUpdate()
    {
        PlayerInSite = FindObjectRaycast(numOfCasts, playerMask, viewAngle, lookDistance);
    }

    private void Update()
    {
        if (NavMeshAgent == null)
            return;

        distanceFromPlayer.value = Vector3.Distance(transform.position, player.transform.position);

        StateMachine.OnUpdate();
    }

    private bool FindObjectRaycast(int numOfCasts, LayerMask mask, float angle = 45f, float distance = Mathf.Infinity)
    {
        // Code for Raycast used modified code from ChatGPT
        float step = angle / (numOfCasts - 1);
        float offset = angle / 2;

        for (int i = 0; i < numOfCasts; i++)
        {
            float targetAngle = transform.eulerAngles.y - offset + i * step;
            Vector3 dir = new(Mathf.Sin(targetAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(targetAngle * Mathf.Deg2Rad));
            
            if (Physics.Raycast(transform.position, dir, distance, mask))
                return true;
        }
        
        return false;
    }
}
