using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EProjectileState
{
    Start,
    Idle,
    Flying,
    End
}

public class ProjectileController<T> : Entity where T : MonoBehaviour
{
    private ITrajectoryPath m_Trajectory;
    private T Target;
    private T Owner;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
