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
    public GameObject BeAdded;

    protected float _CurrentTime { get; set; } = 0;
    public Vector3 Origin { get => this.trajectory.Origin; set => this.Origin = value; }
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
        BeAdded.transform.position = this.trajectory.Calculate(this.CurrentTime, this.TimeToFinish);
    }

    public virtual Vector3 Calculate(float CurrentTime, float TimeToFinish)
    {
        return this.trajectory.Calculate(CurrentTime, TimeToFinish);
    }

    public virtual bool IsComplete()
    {
        return this.trajectory.IsComplete();
    }

    public TrajectoryDecorade(ITrajectoryPath Ease, GameObject BeAdded, Vector3 target, float TimeToFinish = 1)
    {
        this.trajectory = Ease;
        this.BeAdded = BeAdded;
        this.trajectory.Origin = BeAdded.gameObject.transform.position;
        this.trajectory.Target = target;

        this.trajectory.TimeToFinish = TimeToFinish;
    }

    public TrajectoryDecorade(ITrajectoryPath Ease, GameObject BeAdded, GameObject Target)
    {
        this.trajectory = Ease;
        this.BeAdded = BeAdded;
        this.trajectory.Origin = BeAdded.transform.position;
        this.trajectory.Target = Target.transform.position;

        this.TimeToFinish = 10f;
    }

    public TrajectoryDecorade(ITrajectoryPath Ease, Vector3 origin, Vector3 target, GameObject BeAdded)
    {
        this.trajectory = Ease;
        this.trajectory.Origin = origin;
        this.trajectory.Target = target;
        this.BeAdded = BeAdded;
    }
} 
