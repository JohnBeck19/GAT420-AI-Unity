using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIDeathState : AIState
{
    
    public AIDeathState(AIStateAgent agent) : base(agent)
    {
    }

    public override void OnEnter()
    {
        agent.animator?.SetTrigger("Death");
        agent.timer.value = Time.time + 2;
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (Time.time > agent.timer.value)
        {
           GameObject.Destroy(agent.gameObject);
        }
    }
}
