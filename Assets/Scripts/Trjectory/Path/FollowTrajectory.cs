using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FollowTrajectory : TrajectoryDecorade
{
    public GameObject BeFollowed;
    public float MovementSpeed = 18.2f;
    private void UpdateInfoTrajetory()
    {
        this.trajectory.TargetPosition = BeFollowed.transform.position;
        this.trajectory.TimeToFinish = Vector3.Distance(this.trajectory.OriginPosition, this.trajectory.TargetPosition) / this.MovementSpeed;
    }
    private void LookAt(float deltaTime)
    {
        Quaternion _lookRotation = Quaternion.LookRotation((this.trajectory.TargetPosition - this.Owner.transform.position).normalized);
        //over time
        this.Owner.transform.rotation =
            Quaternion.Slerp(this.Owner.transform.rotation, _lookRotation, deltaTime * TimeToFinish);
        //instant
    }
    public override void Update(float deltaTime = 0)
    {
        if (IsComplete())
        {
            if (IsCompeleted == false)
                CallbackWhenCompleted?.Invoke();

            IsCompeleted = true;
            return;
        }

        UpdateInfoTrajetory();
        LookAt(deltaTime);
        base.Update(deltaTime);
    }

    public override bool IsComplete()
    {
        Debug.Log(Vector3.Distance(this.Owner.transform.position, this.TargetPosition) <= Mathf.Epsilon);
        return Vector3.Distance(this.Owner.transform.position, this.TargetPosition) <= Mathf.Epsilon;
    }

    public FollowTrajectory() : base()
    {

    }
    public FollowTrajectory(IProjectilePath trajectory) : base(trajectory)
    {

    }
 
    public FollowTrajectory(IProjectilePath trajectory, GameObject BeAdded, GameObject Target) : base(trajectory, BeAdded, Target)
    {
        this.MovementSpeed = 18.2f;

        this.Owner = BeAdded;
        this.TargetPosition = Target.transform.position;
        this.BeFollowed = Target;
    }

    public FollowTrajectory(IProjectilePath trajectory, Vector3 origin, GameObject BeAdded, GameObject Target) : base(trajectory, BeAdded, Target)
    {
        this.MovementSpeed = 18.2f;

        this.Owner = BeAdded;
        this.TargetPosition = Target.transform.position;
        this.BeFollowed = Target;
    }

    public override string ToString()
    {
        return this.GetType() + "." + this.trajectory.GetType();
    }
}