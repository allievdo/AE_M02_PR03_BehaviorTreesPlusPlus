using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BehaviourAndWeight
{
    public SteeringBehavior behavior = null;
    public float weight = 0f;
}
public class BlendedSteering
{
    public BehaviourAndWeight[] behaviours;

    //the overall maximum acceleration and rotation
    float maxAcceleration = 1f;
    float maxRotation = 5f;

    public SteeringOutput GetSteering()
    {
        SteeringOutput result = new SteeringOutput();

        foreach(BehaviourAndWeight b in behaviours)
        {
            SteeringOutput s = b.behavior.getSteering();
            //accumulate all accelerations
            if (s != null)
            {
                result.angular += s.angular * b.weight;
                result.linear += s.linear * b.weight;
            }
        }

        //crop the result and return
        result.linear = result.linear.normalized * maxAcceleration;
        float angularAcceleration = Mathf.Abs(result.angular);
        if (angularAcceleration > maxRotation)
        {
            result.angular /= angularAcceleration;
            result.angular *= maxRotation;
        }

        return result;
    }
}
