using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Trajectory/Curve")]
public class CurveTrajectoryInfo : ScriptableObject
{
    [SerializeField]
    public AnimationCurve Curves;
    public float TimeToFinish;
}