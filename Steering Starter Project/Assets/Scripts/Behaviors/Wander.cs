using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Wander : SteeringBehavior
{
    public Kinematic character;
    public Vector3 target;


    //the radius and forward offset of the wander circle
    float wanderOffset = 20f;
    float wanderRadius = 10f;

    float targetAngle;

    float slowRadius = 10f;
    float maxRotation = 10f;
    float timeToTarget = 0.1f;
    float maxAngularAcceleration = 100f;

    // the maximum rate at which the wander orientation can change
    float wanderRate = 10f;

    //the current orientation of the character
    float wanderOrientation;

    // the maximum acceleration of the character
    float maxAcceleration = 1f;

    public float randomBinomial()
    {
        float random = Random.value - Random.value;
        random *= Mathf.Rad2Deg;
        return random;
    }

    public float getTargetAngle()
    {
        // --- replace me ---

        Vector3 direction = target - character.transform.position;
        float targetAngle = 0f;

        //check for a zero direction, and make no change if so
        if (direction.magnitude == 0f)
        {
            return character.transform.eulerAngles.y;
        }

        else
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z);
            targetAngle *= Mathf.Rad2Deg;
        }
        // ------------------
        return targetAngle;
    }

    public SteeringOutput alignSteering()
    {
        SteeringOutput result = new SteeringOutput();

        // get the naive direction to the target
        //float rotation = Mathf.DeltaAngle(character.transform.eulerAngles.y, target.transform.eulerAngles.y);
        float rotation = Mathf.DeltaAngle(character.transform.eulerAngles.y, getTargetAngle());
        float rotationSize = Mathf.Abs(rotation);

        // check if we are there, return no steering
        //if (rotationSize < targetRadius)
        //{
        //    return null;
        //}

        // if we are outside the slow radius, then use maximum rotation
        float targetRotation = 0.0f;
        if (rotationSize > slowRadius)
        {
            targetRotation = maxRotation;
        }
        else // otherwise use a scaled rotation
        {
            targetRotation = maxRotation * rotationSize / slowRadius;
        }

        // the final targetRotation combines speed (already in the variable) and direction
        targetRotation *= rotation / rotationSize;

        // acceleration tries to get to the target rotation
        // something is breaking my angularVelocty... check if NaN and use 0 if so
        float currentAngularVelocity = float.IsNaN(character.angularVelocity) ? 0f : character.angularVelocity;
        result.angular = targetRotation - currentAngularVelocity;
        result.angular /= timeToTarget;

        // check if the acceleration is too great
        float angularAcceleration = Mathf.Abs(result.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            result.angular /= angularAcceleration;
            result.angular *= maxAngularAcceleration;
        }

        result.linear = Vector3.zero;
        return result;
    }

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        //update the wander orientation
        wanderOrientation += randomBinomial() * wanderRate;

        //calculate the combined target orientation
        targetAngle = wanderOrientation + character.transform.eulerAngles.y;

        //calculate the center of the wander circle
        //this is wrong so change that later. Change targetAngle to target and change to character.position. somehow fit in wanderAngle???
        target = character.transform.position + (wanderOffset * Vector3.one) + character.transform.forward;

        //calculate the target location
        //fix this
        target += wanderRadius * targetAngle * Vector3.one;

        result = alignSteering();

        //accelerate in the direction of the orientation
        result.linear = maxAcceleration * character.transform.forward;

        //return it
        return result;
    }
}
