using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateAgent : AIAgent
{
    public Animator animator;
    public AIStateMachine stateMachine = new AIStateMachine();
    public float health;

    [SerializeField] public AIPerception enemyPerception;

    private void Start()
    {
        //add states to state machine
        stateMachine.AddState(nameof(AIIdleState), new AIIdleState(this));
        stateMachine.AddState(nameof(AIPatrolState), new AIPatrolState(this));
        stateMachine.AddState(nameof(AIAttackState), new AIAttackState(this));
        stateMachine.AddState(nameof(AIDeathState), new AIDeathState(this));
        stateMachine.AddState(nameof(AIChaseState), new AIChaseState(this));

        stateMachine.setState(nameof(AIIdleState));
    }
    private void Update()
    {
        if (health <= 0) stateMachine.setState(nameof(AIDeathState));

        animator?.SetFloat("Speed", movement.Velocity.magnitude);
        
        stateMachine.Update();
    }

    private void OnGUI()
    {
        // draw label of current state above agent
        GUI.backgroundColor = Color.black;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        Rect rect = new Rect(0, 0, 100, 20);
        // get point above agent
        Vector3 point = Camera.main.WorldToScreenPoint(transform.position);
        rect.x = point.x - (rect.width / 2);
        rect.y = Screen.height - point.y - rect.height - 20;
        // draw label with current state name
        GUI.Label(rect, stateMachine.CurrentState.name);
    }
}
