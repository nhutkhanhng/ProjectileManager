using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearTrajectory : Trajectory
{
    public override Vector3 Calculate(float CurrentTime, float TimeToFinish)
    {
        return Interpolate.Ease(Interpolate.Ease(Interpolate.EaseType.Linear), this.OriginPosition, this.TargetPosition - this.OriginPosition, CurrentTime, TimeToFinish);
    }
    public LinearTrajectory() : base() { }
    public LinearTrajectory(Vector3 origin, Vector3 target, float TimeToFinish = 1) : base(origin, target, TimeToFinish) { }
}