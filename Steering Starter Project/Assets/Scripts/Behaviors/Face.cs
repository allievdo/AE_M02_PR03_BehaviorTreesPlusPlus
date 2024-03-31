using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Face : Align
{
    //IDK IF THIS IS WHAT I NEED TO DO
    // TODO: override Align's getTargetAngle to face the target instead of matching it's orientation
    public override float getTargetAngle()
    {
        // --- replace me ---

        Vector3 direction = target.transform.position - character.transform.position;
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
}
