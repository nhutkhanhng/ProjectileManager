using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanSex<T>
{
    void SetUp(T param);
}

public interface ICantSex : ICanSex<bool>
{

}



public class Test : MonoBehaviour
{

    // Start is called before the first frame update
    public IEnumerator<Vector3> sequence;

    Vector3 origin;
    Vector3 _target;
    public Transform target;

    ITrajectoryPath path;

    void Start()
    {
        sequence = Interpolate.NewEase(Interpolate.Ease(Interpolate.EaseType.Linear), this.transform.position, this.target.position, 5f);
        origin = this.transform.position;
        _target = target.transform.position - origin;
        //path = new LinearTrajectory();

        //path.Origin = this.transform.position;
        //path.Target = this.target.position;
        //path.TimeToFinish = 10f;

        //this.transform.position = Interpolate.Ease(Interpolate.Ease(Interpolate.EaseType.Linear), origin, _target, 5f, 10f);
    }

    public PauseParticle particles;

    private float Duration = 0;

    public bool _IsPause;
    public bool IsPause
    {
        get { return _IsPause; }
        set
        {
            _IsPause = value;

            Debug.LogError(_IsPause);

            if (_IsPause == true)
                particles.system.Pause(false);
            else
                particles.system.Play(false);
        }
    }
    private void Update()
    {
        
        //Debug.LogError(particles.system.isPlaying);
        if (Input.GetKeyDown(KeyCode.K))
            IsPause = false;
        if (Input.GetKeyDown(KeyCode.J))
            IsPause = true;
        //if (isPause)
        //    particles.system.Pause(true);
        //else
        //    particles.system.Play(true);

        Duration += Time.deltaTime;

        this.transform.position = Interpolate.Ease(Interpolate.Ease(Interpolate.EaseType.Linear), this.origin, _target, Duration, 3f);

        //if ( CCollision.CircleCollision(new InfoEntity() { Position = this.gameObject.transform.position, BodyBlock = Vector3.one / 2 },
        //    new InfoEntity() { Position = this.target.position, BodyBlock = Vector3.one / 2}) )
        //{
        //    Debug.LogError("Collsion is detected");
        //}
        if (IsPause)
            return;

        //if (sequence.MoveNext())
        //    this.transform.position = sequence.Current;

    }
    public ClassA prefab;

    //public void FindGameObject<T, V>() where T : Component, V
    //    where V : class
    //{
    //    GameObject go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

    //    go.AddComponent<T>();
    //}

    //[ContextMenu("FFFFFFFFFFf")]
    //public void Find()
    //{
    //    FindGameObject<ClassA, ICantSex>();
    //}

    IEnumerator loadFromResourcesFolder()
    {
        //Request data to be loaded
        ResourceRequest loadAsync = Resources.LoadAsync("GameObject", typeof(GameObject));
        Debug.Log("Load Progress: " + loadAsync.progress);

        //Wait till we are done loading
        while (!loadAsync.isDone)
        {
            Debug.Log("Load Progress: " + loadAsync.progress);
            yield return null;
        }

        //Get the loaded data
        GameObject prefab = loadAsync.asset as GameObject;

        GameObject.Instantiate(prefab);
    }

    [ContextMenu("GGGGGGGGGGG")]
    public void LoadOjbect()
    {
        double duration = Time.fixedTime;

        StartCoroutine(loadFromResourcesFolder());
        Debug.LogError(Time.fixedTime - duration);
    }

    [ContextMenu("1234123JKDSFKLSDJFLK")]
    public void Load()
    {
        double duration = Time.fixedTime;

        Object.Instantiate<ClassA>(this.prefab);
        Debug.LogError(Time.fixedTime - duration);
    }

    [ContextMenu("SDJFKL")]
    public void Count()
    {
        var gameObjects = FindObjectsOfType<ClassA>();

        int count = 0;

        foreach(var obj in gameObjects)
        {
            ICantSex temp = obj;

            temp.SetUp(count % 2 == 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(this.transform.position, Vector3.one  * 1.1f);
    }
}
