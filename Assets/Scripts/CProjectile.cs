using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IProjectile : IEntity
{
    int OwnerID { get; }
    Vector3 Origin { get; set; }
    Vector3 Target { get; set; }
    /// <summary>
    /// Nullable Vector3
    /// </summary>
    float TimeCreate { get; set; }
}

public class Projectile : IProjectile
{   
    public float TimeCreate { get; set; }
    public int ID { get ; set; }
    public string Tag { get; set; }
    public float HP { get; set; }
    public int OwnerID { get; set; }
    public Vector3 Origin { get; set; }
    //bool IsDead { get; set; }
    //Vector3 Target { get; set; }
    public Vector3 Target { get; set; }
    public bool IsDead { get; set; }

    public Projectile(Vector3 Origin, Vector3 Target, float TimeCreate, int OwnerID)
    {
        this.TimeCreate = TimeCreate;
        this.Origin = Origin;
        this.Target = Target;
        this.OwnerID = OwnerID;
    }
}
