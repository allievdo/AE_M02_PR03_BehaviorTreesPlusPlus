using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : SteeringBehavior
{
    public Kinematic character;
    float maxSpeed = .5f;

    //the maximum rotation speed we'd like, probabably should be smaller than the maximum possible,
    //for a leisurley change of direction
    float maxRotation = 10f;

    public float randomBinomial()
    {
        float random = Random.value - Random.value;
        random *= Mathf.Rad2Deg;
        return random;
    }

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        //Get velocity from the vector for of the orientation
        result.linear = maxSpeed * character.transform.forward;

        //Change our orientation randomly
        result.angular = randomBinomial() * maxRotation;

        return result;
    }
}
