using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public interface ICanSex<T>
{
    void SetUp(T param);
}

public interface ICantSex : ICanSex<bool>
{

}


public static class EnumExtension
{
    public static List<T> SplitToEnum<T>(this T values) where T : struct
    {
        List<T> result = new List<T>();

        T[] Arr = (T[])Enum.GetValues(typeof(T));

        for (int i = 0; i < Arr.Length; i++)
        {
            if (values.IsSet<T>(Arr[i]))
            {
                result.Add(Arr[i]);
            }
        }
        return result.Count > 0 ? result : null;
    }

    /// <summary>
    /// This function just only word with Enum
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <returns></returns>
    public static T Parse<T>(string[] values) where T : struct
    {
        return (T)Enum.Parse(typeof(T), string.Join(",", values));
    }

    /// <summary>
    /// Please "," to split string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values">Need "," to split string</param>
    /// <returns></returns>
    public static T Parse<T>(string values) where T : struct
    {
        return (T)Enum.Parse(typeof(T), values);
    }

    public static T Parse<T>(string values, char key) where T : struct
    {
        string[] split = values.Split(key);

        if (string.IsNullOrEmpty(values))
            throw new ArgumentException(String.Format("Argumnent {0} is null or empty", values));

        foreach (var str in split)
            str.Trim();

        return (T)Enum.Parse(typeof(T), string.Join(",", split));
    }


    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static int LengthEnum<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;

        return Arr.Length;
    }

    public static bool IsSet<T>(this T input, T matchTo)
     where T : struct//the constraint I want that doesn't exist in C#3
    {
        return (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;
    }

    public static int Add<T>(this T input, T Addon)
    {
        return Convert.ToInt32(input) | Convert.ToInt32(Addon);
    }
    public static bool IsSet(this Enum input, Enum matchTo)
    {
        return (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;
    }

    private static ulong ToUInt64(object value)
    {
        switch (Convert.GetTypeCode(value))
        {
            case TypeCode.SByte:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
                return (ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture);

            case TypeCode.Byte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
        }

        throw new InvalidOperationException("Unknown enum type.");
    }
}

[Flags]
public enum ERace : short
{
    Beast = 1,
    Demon = 1 << 1,
    Dwarf = 1 << 2,
    Dragon = 1 << 3,
    Element = 1 << 4,
    Elf = 1 << 5,
    Goblin = 1 << 6,
    Human = 1 << 7,
    Naga = 1 << 8,
    Ogre = 1 << 9,
    Orc = 1 << 10,
    Troll = 1 << 11,
    Undead = 1 << 12,
}

public class Test : MonoBehaviour
{

    // Start is called before the first frame update
    public IEnumerator<Vector3> sequence;

    Vector3 origin;
    Vector3 _target;
    public Transform target;

    public ERace race = ERace.Beast | ERace.Demon;
    ITrajectoryPath path;

    void Start()
    {
        sequence = Interpolate.NewEase(Interpolate.Ease(Interpolate.EaseType.Linear), this.transform.position, this.target.position, 5f);
        origin = this.transform.position;
        _target = target.transform.position - origin;
        //path = new LinearTrajectory();
        var list = race.SplitToEnum<ERace>();

        foreach(var r in list)
        {
            Debug.LogError(r);
        }
        Debug.LogError(list.Count);
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

        UnityEngine.Object.Instantiate<ClassA>(this.prefab);
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
