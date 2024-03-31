using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderererer2 : Kinematic
{
    Wander myMoveType;
    //Wander myRotateType;

    // Start is called before the first frame update
    void Start()
    {
        myMoveType = new Wander();
        myMoveType.character = this;

        //myMoveType.target = myTarget;

        //myRotateType = new WanderBehaviour();
        //myRotateType.character = this;
        //myRotateType.target = myTarget;
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = myMoveType.getSteering().linear;
        steeringUpdate.angular = myMoveType.getSteering().angular;
        base.Update();
    }
}