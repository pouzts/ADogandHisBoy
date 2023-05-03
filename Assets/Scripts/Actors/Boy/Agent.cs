using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    public Player player;
    
    [SerializeField] private int numOfCasts = 5;

    [SerializeField] private float minDistanceFromPlayer = 10f;
    [SerializeField] private float playerLookDistance = 10f;
    [SerializeField] private float playerViewAngle = 45f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float interactViewAngle = 30f;
    [SerializeField] private float interactLookDistance = 1f;
    [SerializeField] private LayerMask interactMask;
    
    public NavMeshAgent NavMeshAgent { get; set; }
    public StateMachine StateMachine { get; set; } = new();

    public GameObject Interactable { get; set; } = null;

    public bool PlayerInSite { get; set; } = false;

    public RefValue<bool> FollowPlayer { get; set; } = new();
    public RefValue<bool> StandHere { get; set; } = new();
    public RefValue<bool> FindInteract { get; set; } = new();
    public RefValue<bool> InteractInSite { get; set; } = new();
    public RefValue<bool> Interacted { get; set; } = new();
    
    private readonly RefValue<float> distanceFromPlayer = new();
    private readonly RefValue<bool> hasInteractive = new();

    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();

        // State Creatation
        StateMachine.AddState(new IdleState("IdleState", this));
        StateMachine.AddState(new FollowState("FollowState", this));
        StateMachine.AddState(new StandHereState("StandHereState", this));
        StateMachine.AddState(new FindState("FindState", this));
        StateMachine.AddState(new InteractState("InteractState", this));

        // Current -> Idle
        StateMachine.AddTransition(StateMachine.GetState("FollowState"), new Transition(new Condition<float>(distanceFromPlayer, Predicate.Less, minDistanceFromPlayer)), StateMachine.GetState("IdleState"));
        StateMachine.AddTransition(StateMachine.GetState("StandHereState"), new Transition(new Condition<bool>(StandHere, Predicate.Equal, false)), StateMachine.GetState("IdleState"));
        StateMachine.AddTransition(StateMachine.GetState("FindState"), new Transition(new Condition<bool>(FindInteract, Predicate.Equal, false)), StateMachine.GetState("IdleState"));
        StateMachine.AddTransition(StateMachine.GetState("InteractState"), new Transition(new Condition<bool>(Interacted, Predicate.Equal, false)), StateMachine.GetState("IdleState"));

        // Current -> Follow
        StateMachine.AddTransition(StateMachine.GetState("IdleState"), new Transition(new Condition<float>(distanceFromPlayer, Predicate.GreaterOrEqual, minDistanceFromPlayer), new Condition<bool>(FollowPlayer, Predicate.Equal, true)), StateMachine.GetState("FollowState"));
        StateMachine.AddTransition(StateMachine.GetState("StandHereState"), new Transition(new Condition<bool>(FollowPlayer, Predicate.Equal, true)), StateMachine.GetState("FollowState"));
        StateMachine.AddTransition(StateMachine.GetState("FindState"), new Transition(new Condition<bool>(FollowPlayer, Predicate.Equal, true)), StateMachine.GetState("FollowState"));

        // Current -> Stand
        StateMachine.AddTransition(StateMachine.GetState("IdleState"), new Transition(new Condition<bool>(StandHere, Predicate.Equal, true)), StateMachine.GetState("StandHereState"));
        StateMachine.AddTransition(StateMachine.GetState("FollowState"), new Transition(new Condition<bool>(StandHere, Predicate.Equal, true)), StateMachine.GetState("StandHereState"));
        StateMachine.AddTransition(StateMachine.GetState("FindState"), new Transition(new Condition<bool>(StandHere, Predicate.Equal, true)), StateMachine.GetState("StandHereState"));

        // Current -> Find
        StateMachine.AddTransition(StateMachine.GetState("IdleState"), new Transition(new Condition<bool>(FindInteract, Predicate.Equal, true), new Condition<bool>(hasInteractive, Predicate.Equal, true)), StateMachine.GetState("FindState"));
        StateMachine.AddTransition(StateMachine.GetState("FollowState"), new Transition(new Condition<bool>(FindInteract, Predicate.Equal, true), new Condition<bool>(hasInteractive, Predicate.Equal, true)), StateMachine.GetState("FindState"));
        StateMachine.AddTransition(StateMachine.GetState("StandHereState"), new Transition(new Condition<bool>(FindInteract, Predicate.Equal, true), new Condition<bool>(hasInteractive, Predicate.Equal, true)), StateMachine.GetState("FindState"));

        // Find -> Interact
        StateMachine.AddTransition(StateMachine.GetState("FindState"), new Transition(new Condition<bool>(Interacted, Predicate.Equal, false), new Condition<bool>(InteractInSite, Predicate.Equal, true)), StateMachine.GetState("InteractState"));
        
        StateMachine.SetState(StateMachine.GetState("IdleState"));
    }

    private void FixedUpdate()
    {
        PlayerInSite = FindObjectRaycast(numOfCasts, playerMask, playerViewAngle, playerLookDistance);
        InteractInSite.value = FindObjectRaycast(numOfCasts, interactMask, interactViewAngle, interactLookDistance);
    }

    private void Update()
    {
        if (NavMeshAgent == null)
            return;

        distanceFromPlayer.value = Vector3.Distance(transform.position, player.transform.position);
        hasInteractive.value = Interactable != null;

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
