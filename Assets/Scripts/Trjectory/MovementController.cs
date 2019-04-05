using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ReactionList<T> : List<T>
{
    public event ListChangedEventDelegate ListChanged;
    public delegate void ListChangedEventDelegate();

    public ListChangedEventDelegate changed;
    public new void Add(T item)
    {
        base.Add(item);

        changed?.Invoke();

        if (ListChanged != null
            && ListChanged.GetInvocationList().Any())
        {
            ListChanged();
        }
    }
}

#region CurveTrajectory
public class CurveTrajectory : Trajectory
{
    [SerializeField]
    public CurveTrajectoryInfo curve;
    public override Vector3 Calculate(float elapsedTime, float Duration)
    {
        return this.OriginPosition + Vector3.forward * curve.Curves.Evaluate(elapsedTime/Duration);
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


#endregion

public class MovementController : MonoBehaviour
{
    public ITrajectoryPath trajectory;
    public GameObject Target;
    public GameObject LookAtTarget;

    public ReactionList<GameObject> list;

    private void Start()
    {
        //this.trajectory = new FollowTrajectory(new LinearTrajectory(), this.gameObject, Target);
        // this.trajectory = TrajectoryManager.Instance.Create("FollowTrajectory.LinearTrajectory", this.gameObject, this.gameObject.transform.position, this.Target.transform.position, this.Target);
        //this.trajectory = new FollowTrajectory(new LinearTrajectory(), this.gameObject, this.Target);
        this.trajectory = new LookAtTrajectory(new FollowTrajectory(new LinearTrajectory(), this.gameObject, this.Target), this.gameObject, this.LookAtTarget);
        this.trajectory.CallbackWhenCompleted = () => Debug.LogError("JASKDJAKSLDJKLSAD");

        list = new ReactionList<GameObject>();
        list.changed += () => Debug.LogError("ASDJKSADKLSA");

        list.Add(this.gameObject);
    }

    [ContextMenu("Test")]
    public void TestLookAt()
    {
        this.gameObject.transform.LookAt(this.LookAtTarget.transform);
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
