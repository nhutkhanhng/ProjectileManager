using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrajectoryPath
{
    Vector3 Origin { get; set; }
    Vector3 Target { get; set; }
    Vector3 Calculate(float elapsedTime, float Duration);
    void Start();
    float GetProgress();
    bool IsComplete();
    List<Vector3> ImpactPoints { get; }
    bool IsImpactPoint { get; }
    float CurrentTime { get; }
    float TimeToFinish { get; set; }
    Action CallbackWhenCompleted { get; set; }

    Vector3 CurrentPosition { get; }
    void Update(float Deltatime);

    void SetUp(Vector3 Origin, Vector3 Target, float Duration);
}

public interface IProjectilePath : ITrajectoryPath
{
    GameObject Owner { get; }
}

public enum ETrajectoryType
{
    Zero,
    Linear,
    EaseInQuad,
    EaseOutQuad,
    EaseInOutQuad,
    EraseInBack,
    EraseOutBack,
    EraseInOutBack,

    Throw,

    KnockBack
}

[Serializable]
public class Trajectory : ITrajectoryPath
{
    private List<Vector3> _ImpactPoints = new List<Vector3>();
    public List<Vector3> ImpactPoints { get { return _ImpactPoints; } }
    public bool IsImpactPoint { get; }
    public Vector3 Origin { get; set; }
    public Vector3 Target { get; set; }
    public float TimeToFinish { get; set; }
    protected float _CurrentTime { get; set; } = 0;

    public virtual Vector3 Calculate(float elapsedTime, float Duration)
    {
        _CurrentTime = elapsedTime;
        return Interpolate.Ease(Interpolate.Ease(Interpolate.EaseType.Linear), this.Origin, this.Target - this.Origin, CurrentTime, this.TimeToFinish);
    }

    public float GetProgress()
    {
        return _CurrentTime / TimeToFinish;
    }

    public float CurrentTime { get { return _CurrentTime; } }

    public Action CallbackWhenCompleted { get; set; }

    public Vector3 CurrentPosition { get { return Calculate(this.CurrentTime, this.TimeToFinish); } }

    protected bool IsCompeleted = false;
    public void Start()
    {
        
    }

    public virtual bool IsComplete()
    {
        return _CurrentTime >= TimeToFinish;
    }

    public virtual void Update(float Deltatime)
    {
        if (IsComplete())
        {
            if (IsCompeleted == false)
                CallbackWhenCompleted?.Invoke();

            IsCompeleted = true;
            return;
        }

        this._CurrentTime += Deltatime;
    }

    public void SetUp(Vector3 Origin, Vector3 Target, float Duration)
    {
        this.Origin = Origin;
        this.Target = Target;
        this.TimeToFinish = Duration;
    }

    public Trajectory() { }

    public Trajectory(Vector3 origin, Vector3 target, float TimeToFinish = 1f )
    {
        this.Origin = origin;
        this.Target = target;
        this.TimeToFinish = TimeToFinish;
    }
}

[Serializable]
public class TrajectoryDecorade : ITrajectoryPath
{
    public ITrajectoryPath trajectory;
    public GameObject Owner { get; set; }

    protected float _CurrentTime { get; set; } = 0;
    public Vector3 Origin { get { return this.trajectory.Origin; } set { this.trajectory.Origin = value; } }
    public Vector3 Target { get => this.trajectory.Target; set => this.trajectory.Target = value; }
    public List<Vector3> ImpactPoints => this.trajectory.ImpactPoints;
    public bool IsImpactPoint => this.trajectory.IsImpactPoint;
    public float TimeToFinish { get => this.trajectory.TimeToFinish; set => this.trajectory.TimeToFinish = value; }
    public float CurrentTime => _CurrentTime;

    private bool IsCompeleted = false;
    public Action CallbackWhenCompleted { get; set; }
    public Vector3 CurrentPosition { get { return Calculate(this.CurrentTime, this.TimeToFinish); } }

    public void Start()
    {
        this.trajectory.Start();
    }

    public float GetProgress()
    {
        return this.trajectory.GetProgress();
    }

    public virtual void Update(float deltaTime = 0)
    {

        if (IsComplete())
        {
            if (IsCompeleted == false)
                CallbackWhenCompleted?.Invoke();

            IsCompeleted = true;
            return;
        }

        this._CurrentTime += deltaTime;
        this.trajectory.Update(deltaTime);
        Owner.transform.position = this.trajectory.Calculate(this.CurrentTime, this.TimeToFinish);
    }

    public virtual Vector3 Calculate(float CurrentTime, float TimeToFinish)
    {
        return this.trajectory.Calculate(CurrentTime, TimeToFinish);
    }

    public virtual bool IsComplete()
    {
        return this.trajectory.IsComplete();
    }

    public void SetUp(Vector3 Origin, Vector3 Target, float Duration)
    {
        this.trajectory.SetUp(Origin, Target, Duration);
    }

    public TrajectoryDecorade(ITrajectoryPath Ease, GameObject BeAdded, Vector3 target, float TimeToFinish = 1)
    {
        // Cẩn thận. Chổ này dùng Đệ qui khi Add nhiều Decorade.
        // Coi chừng dùng sai Modify có thể Loop
        this.trajectory = Ease;
        this.trajectory.SetUp(BeAdded.gameObject.transform.position, Target, TimeToFinish);

        this.Owner = BeAdded;
    }

    public TrajectoryDecorade(ITrajectoryPath Ease, GameObject BeAdded, GameObject Target)
    {
        this.trajectory = Ease;
        this.TimeToFinish = 10f;
        this.trajectory.SetUp(BeAdded.transform.position, Target.transform.position, TimeToFinish);
        this.Owner = BeAdded;
        this.trajectory.Origin = BeAdded.transform.position;
        this.trajectory.Target = Target.transform.position;
    }

    public TrajectoryDecorade(ITrajectoryPath Ease, Vector3 origin, Vector3 target, GameObject BeAdded)
    {
        this.trajectory = Ease;
        this.trajectory.Origin = origin;
        this.trajectory.Target = target;
        this.Owner = BeAdded;
    }

    public TrajectoryDecorade(ITrajectoryPath Ease)
    {
        this.trajectory = Ease;
    }

    //public TrajectoryDecorade(IProjectilePath projectile)
    //{

    //}
    public TrajectoryDecorade()
    {

    }
} 
