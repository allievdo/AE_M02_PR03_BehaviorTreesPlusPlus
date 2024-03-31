using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : Seek
{

    public GameObject[] targets;

    public float threshold = 0.5f;
    float decayCoefficient = 100f;
    float maxAcceleration = 0.5f;

   int index = 0;
    protected override Vector3 getTargetPosition()
    {
        //SteeringOutput result = new SteeringOutput();

        //if (targets[0].transform.position.x > targets[1].transform.position.x)
        //{

        //}

        foreach (GameObject target in targets)
        {
            Vector3 directionToTarget = targets[index].transform.position - character.transform.position;
            float distanceToTarget = directionToTarget.magnitude;
            //float mySpeed = 5f;

            if (distanceToTarget < threshold) //i know what this does i just dont know how to make it do what i want
            {
                index++;

                if (index >= targets.Length)
                {
                    index = 0;
                }
            }
        }

        return targets[index].transform.position; //for now

    }
}
