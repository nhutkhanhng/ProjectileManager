using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T _instance = null;
    public static T Instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            return _instance;
        }
    }
}

[CreateAssetMenu(menuName = "Trajectory/CollectCurve")]
public class CollectCurves : SingletonScriptableObject<CollectCurves>
{
    [SerializeField]
    public List<CurveTrajectoryInfo> curves;
    // Start is called before the first frame update

    public CurveTrajectoryInfo GetCurve(string name)
    {
        if (curves == null || curves.Count == 0)
        {
            curves = Resources.FindObjectsOfTypeAll<CurveTrajectoryInfo>().ToList();
        }

        foreach(var curve in curves)
        {
            if (curve.name.Equals(name))
            {
                return curve;
            }
        }

        return null;
    }
}
