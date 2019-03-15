using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;


/// <summary>
/// Mono singleton Class. Extend this class to make singleton component.
/// Example: 
/// <code>
/// public class Foo : MonoSingleton<Foo>
/// </code>. To get the instance of Foo class, use <code>Foo.instance</code>
/// Override <code>Init()</code> method instead of using <code>Awake()</code>
/// from this class.
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T m_Instance = null;
    public static T Instance
    {
        get
        {
            // Instance requiered for the first time, we look for it
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                // Object not found, we create a temporary one
                if (m_Instance == null)
                {
                    Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");

                    isTemporaryInstance = true;
                    m_Instance = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                    // Problem during the creation, this should not happen
                    if (m_Instance == null)
                    {
                        Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                    }
                }
                if (!_isInitialized)
                {
                    _isInitialized = true;
                    m_Instance.Init();
                }
            }
            return m_Instance;
        }
    }

    public static bool isTemporaryInstance { private set; get; }

    private static bool _isInitialized;

    // If no other monobehaviour request the instance in an awake function
    // executing before this one, no need to search the object.
    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
        else if (m_Instance != this)
        {
            Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
            DestroyImmediate(this);
            return;
        }
        if (!_isInitialized)
        {
            DontDestroyOnLoad(gameObject);
            _isInitialized = true;
            m_Instance.Init();
        }
    }


    /// <summary>
    /// This function is called when the instance is used the first time
    /// Put all the initializations you need here, as you would do in Awake
    /// </summary>
    public virtual void Init() { }

    /// Make sure the instance isn't referenced anymore when the user quit, just in case.
    private void OnApplicationQuit()
    {
        m_Instance = null;
    }
}

public class TrajectoryManager : MonoSingleton<TrajectoryManager>
{
    private IProjectilePath newTrajectoryPathCreated = null;

    public SortedList<string, Queue<IProjectilePath>> TrajectoryFactory = new SortedList<string, Queue<IProjectilePath>>();

    public IProjectilePath Create(string name, GameObject Owner,Vector3 OriginPosition, Vector3 TargetPosition, GameObject Target = null)
    {
        IProjectilePath pathBeCreated = null;
        try
        {
            string[] splitName = name.Split('.');
            if (splitName.Length == 1)
            {
                pathBeCreated = Activator.CreateInstance(Type.GetType(name), new object[] { OriginPosition, TargetPosition, 1f }) as IProjectilePath;
                //var instance = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(Type.GetType(name), new object[] { OriginPosition });
            }
            else if (splitName.Length == 2)
            {
                pathBeCreated = Activator.CreateInstance(Type.GetType(splitName[0]), new object[] { Activator.CreateInstance(Type.GetType(splitName[1])) as ITrajectoryPath, OriginPosition, Owner, Target }) as IProjectilePath;
                Debug.LogError(pathBeCreated);
            }
        }
        catch (Exception Ex)
        {
            Debug.LogError(Ex.Message);
            return null;
        }
        //if ()
        if (pathBeCreated != null)
        {
            if (this.TrajectoryFactory.ContainsKey(pathBeCreated.ToString()) == false)
            {
                this.TrajectoryFactory.Add(pathBeCreated.ToString(), new Queue<IProjectilePath>());
                this.TrajectoryFactory[pathBeCreated.ToString()].Enqueue(pathBeCreated);
            }
            else
            {
                this.TrajectoryFactory[pathBeCreated.ToString()].Enqueue(pathBeCreated);
            }
        }

        this.newTrajectoryPathCreated = pathBeCreated;
        return pathBeCreated;
    }
    //public static Type GetGenericElementType(this Type type)
    //{
    //    // Short-circuit for Array types
    //    if (typeof(Array).IsAssignableFrom(type))
    //    {
    //        return type.GetElementType();
    //    }

    //    while (true)
    //    {
    //        // Type is IEnumerable<T>
    //        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
    //        {
    //            return type.GetGenericArguments().First();
    //        }

    //        // Type implements/extends IEnumerable<T>
    //        Type elementType = (from subType in type.GetInterfaces()
    //                            let retType = subType.GetGenericElementType()
    //                            where retType != subType
    //                            select retType).FirstOrDefault();

    //        if (elementType != null)
    //        {
    //            return elementType;
    //        }

    //        if (type.BaseType == null)
    //        {
    //            return type;
    //        }

    //        type = type.BaseType;
    //    }
    //}

    //public Hashtable 
}

