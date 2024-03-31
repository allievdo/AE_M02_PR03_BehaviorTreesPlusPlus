using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Wanderererererer : Kinematic
{
    WanderBehaviour myMoveType;
    WanderBehaviour myRotateType;

    // Start is called before the first frame update
    void Start()
    {
        myMoveType = new WanderBehaviour();
        myMoveType.character = this;

        //myMoveType.target = myTarget;

        myRotateType = new WanderBehaviour();
        myRotateType.character = this;
        //myRotateType.target = myTarget;
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = myMoveType.getSteering().linear;
        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.Update();
    }
}
