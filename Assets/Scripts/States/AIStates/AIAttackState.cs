using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIAttackState : AIState
{

    public AIAttackState(AIStateAgent agent) : base(agent)
    {
        AIStateTransition transition = new AIStateTransition(nameof(AIIdleState));
        transition.AddCondition(new FloatCondition(agent.timer, Condition.Predicate.LESS, 0));
        transitions.Add(transition);
    }

    public override void OnEnter()
    {
        agent.movement.Stop();
        agent.movement.Velocity = Vector3.zero;

        agent.animator?.SetTrigger("Attack");
        agent.timer.value = Time.time+2;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        agent.timer.value -= Time.deltaTime;

    }

}
