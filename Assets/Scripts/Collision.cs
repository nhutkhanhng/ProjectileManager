using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionTYpe
{
    Point = 1,
    Box = 2,
    BoxRotation = 4,
    Circle = 8,
}

public struct Annular
{
    public Vector3 Position;

    public Vector2 LongRadius;
    public Vector2 ShortRadius;

    public float TimeToFinish;
}
public static class CCollision
{
    public static bool AABBCollision (InfoEntity Origin, InfoEntity Target)
    {
        Vector3 OriginPosition = Origin.Position;
        Vector3 TargetPosition = Target.Position;

        Vector2 Origin2D = new Vector2(OriginPosition.x, OriginPosition.z);
        Vector2 Target2D = new Vector2(TargetPosition.x, TargetPosition.z);

        Rect origin = new Rect(Origin2D, Origin.BodyBlock);
        Rect target = new Rect(Target2D, Target.BodyBlock);

        return _AABB(origin, target);
    }

    private static bool _AABB(Rect box, Rect boxTarget)
    {
        return (box.Overlaps(boxTarget));
    }

    public static bool CircleCollision(InfoEntity Origin, InfoEntity Target, int Axis = 0)
    {
        return _CircleCollsion(Origin, Target, Axis);
    }

    private static bool _CircleCollsion(InfoEntity origin, InfoEntity target, int Axis = 0)
    {
        float DistanceSquard = Vector3.Distance(origin.Position, target.Position);

        return DistanceSquard <= (Mathf.Abs((Axis == 0) ? origin.BodyBlock.x : origin.BodyBlock.z) + Mathf.Abs((Axis == 0) ? target.BodyBlock.x : target.BodyBlock.z));
    }
}
