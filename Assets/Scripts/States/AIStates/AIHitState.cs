using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIHitState : AIState
{
    
    public AIHitState(AIStateAgent agent) : base(agent)
    {
        AIStateTransition transition = new AIStateTransition(nameof(AIIdleState));
        transition.AddCondition(new FloatCondition(agent.timer, Condition.Predicate.LESS, 0));
        transitions.Add(transition);
    }

    public override void OnEnter()
    {
        agent.health.value -= 5;
        agent.animator?.SetTrigger("Hit");
        agent.timer.value = Time.time + 2;
        Debug.Log("hit");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        agent.timer.value -= Time.deltaTime;

    }
}
