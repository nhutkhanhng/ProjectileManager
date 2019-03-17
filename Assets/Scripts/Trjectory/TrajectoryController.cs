using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController: MonoBehaviour 
{ 
    protected IProjectilePath projectile;
    public GameObject Owner;
    public GameObject Target;

    protected ConfigProjectileRecord config;
    protected ETrajectoryType type;

    [Range(1, 5)]
    public float TimeToStart = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitTime(TimeToStart));
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeToStart > 0)
        {
            return;
        }
        this.projectile.Update(Time.deltaTime);
    }

    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        TimeToStart -= time;
        type = ETrajectoryType.LinearTrajectory | ETrajectoryType.FollowTrajectory;
        this.projectile = TrajectoryManager.Instance.Create("FollowTrajectory, LinearTrajectory",
            this.gameObject, this.gameObject.transform.position, this.Target.transform.position, this.Target.gameObject);

        this.projectile.CallbackWhenCompleted = () =>
        {
            Debug.LogError("Done Done Done");
            TrajectoryManager.Instance.Return(this.projectile.ToString(), this.projectile);
        };
    }
}
