using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateAgent : AIAgent
{
    [SerializeField] AIPerception enemyPerception;
    AIStateMachine stateMachine = new AIStateMachine();

    private void Start()
    {
        //add states to state machine
        stateMachine.AddState(nameof(AIIdleState), new AIIdleState(this));
        stateMachine.AddState(nameof(AIPatrolState), new AIPatrolState(this));
        stateMachine.AddState(nameof(AIAttackState), new AIAttackState(this));
        stateMachine.AddState(nameof(AIDeathState), new AIDeathState(this));

        stateMachine.setState(nameof(AIIdleState));
    }
    private void Update()
    {
        var enemies = enemyPerception.GetGameObjects();
        if (enemies.Length > 0) { stateMachine.setState(nameof(AIAttackState)); }
        else { stateMachine.setState(nameof(AIIdleState)); }  

        stateMachine.Update();
    }
}
