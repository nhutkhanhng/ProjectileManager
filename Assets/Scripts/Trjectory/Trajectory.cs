
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Trajectory : IProjectilePath
{
    private List<Vector3> _ImpactPoints = new List<Vector3>();
    public List<Vector3> ImpactPoints { get { return _ImpactPoints; } }
    public bool IsImpactPoint { get; }
    public Vector3 OriginPosition { get; set; }
    public Vector3 TargetPosition { get; set; }
    public float TimeToFinish { get; set; }
    protected float _CurrentTime { get; set; } = 0;

    public virtual Vector3 Calculate(float elapsedTime, float Duration)
    {
        _CurrentTime = elapsedTime;
        return Interpolate.Ease(Interpolate.Ease(Interpolate.EaseType.Linear), this.OriginPosition, this.TargetPosition - this.OriginPosition, CurrentTime, this.TimeToFinish);
    }

    public float GetProgress()
    {
        return _CurrentTime / TimeToFinish;
    }

    public float CurrentTime { get { return _CurrentTime; } }

    public Action CallbackWhenCompleted { get; set; }

    public Vector3 CurrentPosition { get { return Calculate(this.CurrentTime, this.TimeToFinish); } }

    public GameObject Owner { get; set; }
    public GameObject Target { get; set; }

    protected bool IsCompeleted = false;
    public virtual void Start()
    {
        //  Todo something else
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

    public virtual void SetUp(Vector3 Origin, Vector3 Target, float Duration)
    {
        this.OriginPosition = Origin;
        this.TargetPosition = Target;
        this.TimeToFinish = Duration;
    }

    public virtual void SetUp(GameObject Owner, GameObject Target, float Duration)
    {
        // Nothign
        this.Owner = Owner;
        this.Target = Target;
        this.TimeToFinish = Duration;
    }

    #region Constructor
    public Trajectory() { }

    public Trajectory(Vector3 origin, Vector3 target, float TimeToFinish = 1f)
    {
        _CurrentTime = 0f;
        IsCompeleted = false;
        this.SetUp(origin, target, TimeToFinish);
    }

    public Trajectory(Vector3 Origin, Vector3 TargetPosition, float TimeToFinish = 1, GameObject Owner = null, GameObject Target = null)
    {
        this.SetUp(Origin, TargetPosition, TimeToFinish);
        this.SetUp(Owner, Target, TimeToFinish);
    }
    #endregion
}

[Serializable]
public class TrajectoryDecorade : IProjectilePath
{
    public IProjectilePath trajectory;
   
    protected float _CurrentTime { get; set; } = 0;

    #region Position - Vector3
    public Vector3 OriginPosition { get { return this.trajectory.OriginPosition; } set { this.trajectory.OriginPosition = value; } }
    public Vector3 TargetPosition { get => this.trajectory.TargetPosition; set => this.trajectory.TargetPosition = value; }
    public Vector3 CurrentPosition { get { return Calculate(this.CurrentTime, this.TimeToFinish); } }
    #endregion

    #region Time
    public float TimeToFinish { get => this.trajectory.TimeToFinish; set => this.trajectory.TimeToFinish = value; }
    public float CurrentTime => _CurrentTime;
    #endregion

    #region World - GameObject
    protected bool IsCompeleted = false;
    public Action CallbackWhenCompleted { get; set; }
  
    public GameObject Owner { get { return this.trajectory.Owner; } set { this.trajectory.Owner = value; } }
    public GameObject Target { get; set; }
    #endregion

    #region Collision
    public List<Vector3> ImpactPoints => this.trajectory.ImpactPoints;
    public bool IsImpactPoint => this.trajectory.IsImpactPoint;
    #endregion

    #region Interface Function
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
    #endregion

    #region Setup
    public virtual void SetUp(Vector3 Origin, Vector3 Target, float Duration)
    {
        _CurrentTime = 0f;
        IsCompeleted = false;
        this.trajectory.SetUp(Origin, Target, Duration);
    }

    public void SetUp(GameObject Owner, GameObject Target, float Duration)
    {
        _CurrentTime = 0f;
        IsCompeleted = false;
        this.trajectory.Owner = Owner;
        this.trajectory.Target = Target;
        this.trajectory.TimeToFinish = Duration;
    }
    #endregion

    #region Constructor 
    public TrajectoryDecorade()
    {
        // Need to impove
    }
    public TrajectoryDecorade(IProjectilePath Ease)
    {
        this.trajectory = Ease;
    }
    public TrajectoryDecorade(IProjectilePath Ease, Vector3 origin, Vector3 target, float TimeToTarget = 1, GameObject BeAdded = null)
    {
        this.trajectory = Ease;
        this.SetUp(origin, target, TimeToTarget);
        this.SetUp(Owner, null, TimeToTarget);
    }
    public TrajectoryDecorade(IProjectilePath Ease, GameObject BeAdded, Vector3 target, float TimeToFinish = 1)
    {
        this.trajectory = Ease;
        this.trajectory.SetUp(BeAdded.gameObject.transform.position, TargetPosition, TimeToFinish);
        this.trajectory.SetUp(BeAdded, null, TimeToFinish);        
    }

    public TrajectoryDecorade(IProjectilePath Ease, GameObject BeAdded, GameObject Target, float Duration = 1f)
    {
        this.trajectory = Ease;
        this.trajectory.SetUp(BeAdded.transform.position, Target.transform.position, Duration);
        this.SetUp(BeAdded, Target, Duration);
    }

    public TrajectoryDecorade(IProjectilePath Ease, Vector3 Origin, Vector3 TargetPosition, GameObject BeAdded, GameObject Target, float Duration = 1f)
    {
        this.trajectory = Ease;
        this.SetUp(Origin, TargetPosition, Duration);
        this.SetUp(BeAdded, Target, Duration);
    }
    #endregion
}
