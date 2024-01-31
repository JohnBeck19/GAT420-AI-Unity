using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAutonomousAgent : AIAgent
{
	[SerializeField] AIPerception seekPerception = null;
	[SerializeField] AIPerception fleePerception = null;
	[SerializeField] AIPerception flockPerception = null;
	[SerializeField] AIPerception obstaclePerception = null;

	private void Update()
	{
		if (seekPerception != null)
		{
			var gameObjects = seekPerception.GetGameObjects();
			if (gameObjects.Length > 0)
			{
				movement.ApplyForce(Seek(gameObjects[0]));
			}
        }
        if (fleePerception != null)
        {
            var gameObjects = fleePerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                movement.ApplyForce(Flee(gameObjects[0]));
            }
        }
		if (flockPerception != null)
		{
			var gameObjects = flockPerception.GetGameObjects();
			if (gameObjects.Length > 0)
			{
				movement.ApplyForce(Cohesion(gameObjects));
				movement.ApplyForce(Separation(gameObjects,3));
				movement.ApplyForce(Alignment(gameObjects));
			}
		}
		if (obstaclePerception != null)
		{
			if (((AISpherecastPerception)obstaclePerception).CheckDirection(Vector3.forward))
			{ 
				Vector3 open = Vector3.zero;
				if (((AISpherecastPerception)obstaclePerception).GetOpenDirection(ref open))
				{ 
					movement.ApplyForce(GetSteeringForce(open) * 5);
				}
			}
		}

        transform.position = Utilities.Wrap(transform.position, new Vector3(-25, -25, -25), new Vector3(25, 25, 25));
	}
	private Vector3 Seek(GameObject target)
	{
		Vector3 direction = target.transform.position - transform.position;
		return GetSteeringForce(direction);

	}
    private Vector3 Flee(GameObject target)
    {
        Vector3 direction = transform.position - target.transform.position ;
        return GetSteeringForce(direction);

    }
	private Vector3 Cohesion(GameObject[] neighbors)
	{
		Vector3 positions = Vector3.zero;
		foreach (var neighbor in neighbors)
		{
			positions += neighbor.transform.position;
		}
		Vector3 center = positions / neighbors.Length;
		Vector3 direction = center - transform.position;
		return GetSteeringForce(direction);
	}
    private Vector3 Separation(GameObject[] neighbors, float radius)
    {
        Vector3 separation = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            // get direction vector away from neighbor
            Vector3 direction = (transform.position - neighbor.transform.position);
            // check if within separation radius
            if (direction.magnitude < radius)
            {
                // scale separation vector inversely proportional to the direction distance
                // closer the distance the stronger the separation
                separation += direction / direction.sqrMagnitude;
            }
        }

        Vector3 force = GetSteeringForce(separation);

        return force;
    }
    private Vector3 Alignment(GameObject[] neighbors)
	{
		Vector3  velocities = Vector3.zero;

		foreach (var neighbor in neighbors)
		{
			velocities += neighbor.GetComponent<AIAgent>().movement.Velocity;
		}
		Vector3 averageVelocity = velocities / neighbors.Length;
		return GetSteeringForce(averageVelocity);
	}
    public Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.maxForce);

        return force;
    }
}
