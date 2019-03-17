using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrajectoryPath
{
    Vector3 OriginPosition { get; set; }
    Vector3 TargetPosition { get; set; }
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
    GameObject Owner { get; set; }
    GameObject Target { get; set; }

    void SetUp(GameObject Owner, GameObject Target, float Duration);
}

[Flags]
public enum ETrajectoryType : byte
{
    ZeroTrajectory = 1,
    LinearTrajectory = 1 << 1,
    FollowTrajectory = 1 << 2,
    LookAtTrajectory = 1 << 3,

    EaseInQuad,
    EaseOutQuad,
    EaseInOutQuad,
    EraseInBack,
    EraseOutBack,
    EraseInOutBack,

    Throw,
    KnockBack
}
