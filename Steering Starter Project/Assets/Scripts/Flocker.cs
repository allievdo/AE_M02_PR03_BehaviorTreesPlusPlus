using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Kinematic
{
    public GameObject myCohereTarget;
    BlendedSteering mySteering;
    Kinematic[] kBirds;

    void Start ()
    {
        //separate from other birds
        Separation separate = new Separation();
        separate.character = this;
        GameObject[] goBirds = GameObject.FindGameObjectsWithTag("bird");
        kBirds = new Kinematic[goBirds.Length-1];
        int j = 0;

        for (int i = 0; i < goBirds.Length - 1; i++)
        {
            if (goBirds[i] == this)
            {
                continue;
            }
            kBirds[j++] = goBirds[i].GetComponent<Kinematic>();
        }
        separate.targets = kBirds;

        //cohere to center of mass
        Arrive cohere = new Arrive();
        cohere.character = this;
        cohere.target = myCohereTarget;

        //look where center of mass is going
        LookWhereGoing myRotateType = new LookWhereGoing();
        myRotateType.character = this;

        mySteering = new BlendedSteering();
        mySteering.behaviours = new BehaviourAndWeight[3];
        mySteering.behaviours[0] = new BehaviourAndWeight();
        mySteering.behaviours[0].behavior = separate;
        mySteering.behaviours[0].weight = 1f;
        mySteering.behaviours[1] = new BehaviourAndWeight();
        mySteering.behaviours[1].behavior = cohere;
        mySteering.behaviours[1].weight = 1f;
        mySteering.behaviours[2] = new BehaviourAndWeight();
        mySteering.behaviours[2].behavior = myRotateType;
        mySteering.behaviours[2].weight = 1f;
    }

    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate = mySteering.GetSteering();
        base.Update();
    }
}
