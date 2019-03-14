using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class CurveTrajectory : Trajectory
{
    [SerializeField]
    public CurveTrajectoryInfo curve;
    public override Vector3 Calculate(float elapsedTime, float Duration)
    {
        return this.Origin + Vector3.forward * curve.Curves.Evaluate(elapsedTime/Duration);
    }

    public CurveTrajectory(string CurveName)
    {
        this.curve = CollectCurves.Instance.GetCurve(CurveName);
    }
    public CurveTrajectory(CurveTrajectoryInfo info)
    {
        this.curve = info;
    }
    public CurveTrajectory(CurveTrajectoryInfo info, Vector3 origin, Vector3 target, float TimeToFinish = 1) : base (origin, target, TimeToFinish) { this.curve = info; }
    public CurveTrajectory() : base() { }
    public CurveTrajectory(Vector3 origin, Vector3 target, float TimeToFinish = 1) : base(origin, target, TimeToFinish) { }
}

public class LinearTrajectory : Trajectory
{
    public override Vector3 Calculate(float CurrentTime, float TimeToFinish)
    {
        return Interpolate.Ease(Interpolate.Ease(Interpolate.EaseType.Linear), this.Origin, this.Target - this.Origin, CurrentTime, TimeToFinish);
    }
    public LinearTrajectory() : base() { }
    public LinearTrajectory(Vector3 origin, Vector3 target, float TimeToFinish = 1) : base(origin, target, TimeToFinish) { }
}


[Serializable]
public class FollowTrajectory : TrajectoryDecorade
{
    public GameObject BeFollowed;
    public float MovementSpeed = 18.2f;
    private void UpdateInfoTrajetory()
    {
        this.trajectory.Target = BeFollowed.transform.position;
        this.trajectory.TimeToFinish = Vector3.Distance(this.trajectory.Origin, this.trajectory.Target) / this.MovementSpeed;
    }
    public override void Update(float deltaTime = 0)
    {
        UpdateInfoTrajetory();
        base.Update(deltaTime);
    }

    public override bool IsComplete()
    {
        return Vector3.Distance(this.BeAdded.transform.position, this.Target) <= Mathf.Epsilon;
    }

    public FollowTrajectory(ITrajectoryPath trajectory,GameObject BeAdded, GameObject Target) : base (trajectory,BeAdded, Target)
    {
        this.MovementSpeed = 18.2f;

        this.BeAdded = BeAdded;
        this.BeFollowed = Target;
    }

    public FollowTrajectory(ITrajectoryPath trajectory, Vector3 origin, GameObject BeAdded, GameObject Target) : base(trajectory, BeAdded, Target)
    {
        this.MovementSpeed = 18.2f;

        this.BeAdded = BeAdded;
        this.BeFollowed = Target;
    }
}
public class MovementController : MonoBehaviour
{
    public ITrajectoryPath trajectory;
    public GameObject Target;
    private void Start()
    {
        this.trajectory = new FollowTrajectory(new CurveTrajectory("DownAndUp"), this.gameObject, Target);
        this.trajectory.CallbackWhenCompleted = () => Debug.LogError("JASKDJAKSLDJKLSAD");
    }

    private void Update()
    {
        this.trajectory.Update(Time.deltaTime);
    }
    public CurveTrajectoryInfo curve;

    //void Start()
    //{
    //    //curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    //    //curve.preWrapMode = WrapMode.PingPong;
    //    //curve.postWrapMode = WrapMode.PingPong;
    //}

    //void Update()
    //{
    //    transform.position = new Vector3(transform.position.x, curve.Evaluate(Time.time), transform.position.z);
    //}
}
