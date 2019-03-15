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

public class RunARoundTrajectory : Trajectory
{
    public Transform objectToOrbit; //Object To Orbit
    Vector3 orbitAxis = Vector3.up; //Which vector to use for Orbit
    float orbitRadius = 75.0f; //Orbit Radius
    float orbitRadiusCorrectionSpeed = 0.5f; //How quickly the object moves to new position
    float orbitRoationSpeed = 10.0f; //Speed Of Rotation arround object
    float orbitAlignToDirectionSpeed = 0.5f; //Realign speed to direction of travel

    private Vector3 orbitDesiredPosition;
    private Vector3 previousPosition;
    private Vector3 relativePos;
    private Quaternion rotation;
    private Transform thisTransform;

    //---------------------------------------------------------------------------------------------------------------------

    public override void Update(float DeltaTime)
    {
        //Movement
        thisTransform.RotateAround(objectToOrbit.position, orbitAxis, orbitRoationSpeed * DeltaTime);
        orbitDesiredPosition = (thisTransform.position - objectToOrbit.position).normalized * orbitRadius + objectToOrbit.position;
        thisTransform.position = Vector3.Slerp(thisTransform.position, orbitDesiredPosition, Time.deltaTime * orbitRadiusCorrectionSpeed);

        //Rotation
        relativePos = thisTransform.position - previousPosition;
        rotation = Quaternion.LookRotation(relativePos);
        thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, rotation, orbitAlignToDirectionSpeed * Time.deltaTime);
        previousPosition = thisTransform.position;
    }
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

    private void LookAt(float deltaTime)
    {
        Quaternion _lookRotation = Quaternion.LookRotation((this.trajectory.Target - this.Owner.transform.position).normalized);
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

        UpdateInfoTrajetory();
        LookAt(deltaTime);
        base.Update(deltaTime);
    }

    public override bool IsComplete()
    {
        return Vector3.Distance(this.Owner.transform.position, this.Target) <= Mathf.Epsilon;
    }

    public FollowTrajectory(ITrajectoryPath trajectory) : base (trajectory)
    {
        
    }

    public FollowTrajectory(ITrajectoryPath trajectory,GameObject BeAdded, GameObject Target) : base (trajectory,BeAdded, Target)
    {
        this.MovementSpeed = 18.2f;

        this.Owner = BeAdded;
        this.BeFollowed = Target;
    }

    public FollowTrajectory(ITrajectoryPath trajectory, Vector3 origin, GameObject BeAdded, GameObject Target) : base(trajectory, BeAdded, Target)
    {
        this.MovementSpeed = 18.2f;

        this.Owner = BeAdded;
        this.BeFollowed = Target;
    }

    public override string ToString()
    {
        return this.GetType() + "." + this.trajectory.GetType();
    }
}

[Serializable]
public class LookAtTarget : TrajectoryDecorade
{
    public GameObject BeFollowed;
    public float RotationSpeed = 2f;
    private void LookAt(float deltaTime)
    {
        Quaternion _lookRotation = Quaternion.LookRotation((this.trajectory.Target - this.Owner.transform.position).normalized);
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

    public LookAtTarget(ITrajectoryPath trajectory, GameObject BeAdded, GameObject Target) : base(trajectory, BeAdded, Target)
    {
        this.RotationSpeed = 2f;
        this.Owner = BeAdded;
        this.BeFollowed = Target;
    }

    public LookAtTarget(ITrajectoryPath trajectory, Vector3 origin, GameObject BeAdded, GameObject Target) : base(trajectory, BeAdded, Target)
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

public class MovementController : MonoBehaviour
{
    public ITrajectoryPath trajectory;
    public GameObject Target;
    private void Start()
    {
        //this.trajectory = new FollowTrajectory(new LinearTrajectory(), this.gameObject, Target);
        //this.trajectory = TrajectoryManager.Instance.Create("FollowTrajectory.LinearTrajectory", this.gameObject, this.gameObject.transform.position, this.Target.transform.position, this.Target);
        this.trajectory = new LookAtTarget(new FollowTrajectory(new LinearTrajectory()), this.gameObject, this.Target);
        this.trajectory.CallbackWhenCompleted = () => Debug.LogError("JASKDJAKSLDJKLSAD");

        Debug.LogError(this.trajectory.ToString());
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
