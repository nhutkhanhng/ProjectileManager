using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class LookAtTrajectory : TrajectoryDecorade
{
    public GameObject BeFollowed;
    public float RotationSpeed = 2f;
    private void LookAt(float deltaTime)
    {
        Quaternion _lookRotation = Quaternion.LookRotation((this.BeFollowed.transform.position - this.Owner.transform.position).normalized);
        //over time
        this.Owner.transform.rotation =
            Quaternion.Slerp(this.Owner.transform.rotation, _lookRotation, deltaTime * TimeToFinish);
        //instant
    }
    public override void Update(float deltaTime = 0)
    {
        if (IsComplete())
        {
            return;
        }

        LookAt(deltaTime);
        base.Update(deltaTime);
    }

    public LookAtTrajectory(IProjectilePath trajectory, GameObject BeAdded, GameObject Target) : base(trajectory, BeAdded, Target)
    {
        this.RotationSpeed = 2f;
        this.Owner = BeAdded;
        this.BeFollowed = Target;
    }

    public LookAtTrajectory(IProjectilePath trajectory, Vector3 origin, GameObject BeAdded, GameObject Target) : base(trajectory, BeAdded, Target)
    {
        this.RotationSpeed = 2f;

        this.Owner = BeAdded;
        this.BeFollowed = Target;
    }

    public override string ToString()
    {
        return this.GetType() + "." + this.trajectory.ToString();
    }
}
