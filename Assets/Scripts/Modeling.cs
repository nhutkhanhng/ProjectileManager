using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTag
{
    public const string Player = "Player";
}

public interface IOnInit<T>
{
    void Init(T param);
}
public interface ISetUp<T>
{
    void SetUp(T param);
}


public static class Modeling
{
    public static T LoadResource<T>(string path) where T : UnityEngine.Object
    {
        var result = Resources.Load<T>(path);

        return result;
    }
    public static T LoadResource<T>(string name, string ext) where T : UnityEngine.Object
    {
        T ret = Resources.Load<T>(name);
        return ret;
    }
    public static T Instantiate<T>(this UnityEngine.Object unityObject, T t) where T : UnityEngine.Object
    {
        return UnityEngine.Object.Instantiate(t) as T;
    }

    public static T Instantiate<T>(this UnityEngine.Object unityObject, T t, Vector3 localPosition, Quaternion localRotation, Transform parent = null) where T : UnityEngine.Object
    {
        return UnityEngine.Object.Instantiate(t, localPosition, localRotation, parent) as T;
    }

    //public static T CloneGameObject<T>(this UnityEngine.Object Prefab, T component, Vector3 position, Quaternion localRotation, string tag = "Default", Transform parent = null)
    //    where T : class
    //{
    //    try
    //    {
    //        var ret = UnityEngine.Object.Instantiate<T>(component) as T;

    //        if (ret == null)
    //            Debug.LogError("Cannot load prefab " + path);
    //        return ret;
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError("Cannot load prefab " + path + "with error " + e.Message);
    //    }
    //    return null;
    //}
    public static GameObject LoadPrefab(string path)
    {
        try
        {
            var ret = GameObject.Instantiate(LoadResource<GameObject>(path) as GameObject);

            if (ret == null)
                Debug.LogError("Cannot load prefab " + path);
            return ret;
        }
        catch (Exception e)
        {
            Debug.LogError("Cannot load prefab " + path + "with error " + e.Message);
        }
        return null;
    }

    public static GameObject LoadPrefab(UnityEngine.Object asset)
    {
        try
        {
            var ret = GameObject.Instantiate(asset) as GameObject;

            if (ret == null)
                Debug.LogError("Cannot load prefab " + ret);
            return ret;
        }
        catch (Exception e)
        {
            Debug.LogError("Cannot load prefab with error " + e.Message);
        }
        return null;
    }

    public static GameObject LoadPrefab(GameObject prefab, string tag = "Default", Transform parent = null)
    {
        if (prefab == null) return null;

        GameObject go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        //Debug.LogError(go.name);
        return go;
    }
    public static GameObject LoadPrefab(GameObject prefab, Transform parent = null, bool resetScale = true)
    {
        if (prefab == null) return null;

        GameObject go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        if (resetScale)
            go.transform.localScale = Vector3.one;
        //Debug.LogError(go.name);
        return go;

    }


    public static GameObject LoadGameObject(string modelPath, Vector3 localScale, Quaternion localRotation, string tag = "Default", Transform parent = null)
    {
        GameObject model = GameObject.Instantiate(Resources.Load<GameObject>(modelPath));

        if (model == null)
            return null;

        model.transform.SetParent(parent);

        model.transform.localPosition = Vector3.zero;
        model.transform.localScale = localScale;

        model.transform.localRotation = localRotation;
        model.tag = tag;

        return model;
    }

    public static GameObject LoadGameObject(string modelPath, Vector3 localScale, string tag = "Default", Transform parent = null)
    {
        GameObject model = GameObject.Instantiate(Resources.Load<GameObject>(modelPath));

        if (model == null)
            return null;

        model.transform.SetParent(parent);

        model.transform.localPosition = Vector3.zero;
        model.transform.localScale = localScale;

        model.transform.localRotation = Quaternion.Euler(0, 0, 0);
        model.tag = tag;

        return model;
    }

    public static GameObject LoadGameObject<T>(string path, T component, Vector3 localScale, string tag = "Default", Transform parent = null)
        where T : UnityEngine.Component
    {
        GameObject model = GameObject.Instantiate(Resources.Load<GameObject>(path));

        if (model == null)
            return null;

        model.transform.SetParent(parent);

        model.transform.localPosition = Vector3.zero;
        model.transform.localScale = localScale;

        model.transform.localRotation = Quaternion.Euler(0, 0, 0);
        model.tag = tag;


        model.AddComponent<T>();

        return model;
    }


    public static GameObject LoadPrefab(string path, Vector3 localScale, Quaternion localRataion, Vector3 localPosition, string tag = "Default", Transform parent = null)
    {
        GameObject model = LoadPrefab(path);

        if (model == null)
            return null;

        model.transform.SetParent(parent);

        model.transform.localPosition = localPosition;
        model.transform.localScale = localScale;

        model.transform.localRotation = localRataion;
        model.tag = tag;

        return model;
    }

    public static GameObject LoadPrefab(string path, Vector3 localScale, Quaternion localRotation, string tag = "Default", Transform parent = null)
    {
        return LoadPrefab(path, localScale, localRotation, Vector3.zero, tag, parent);
    }

    public static GameObject LoadPrefab(string path, Quaternion localRataion, Vector3 localPosition, string tag = "Default", Transform parent = null)
    {
        return LoadPrefab(path, Vector3.one, localRataion, Vector3.zero, tag, parent);
    }

    public static GameObject LoadPrefab(string path, string tag = "Default", Transform parent = null)
    {
        return LoadPrefab(path, Vector3.one, Quaternion.Euler(0, 0, 0), Vector3.zero, tag, parent);
    }

    public static GameObject LoadPrefab<T, V>(string path, Vector3 localScale, string tag = "Default", Transform parent = null)
        where T : UnityEngine.Component, V
        where V : class
    {
        return LoadPrefab<T, V>(path, localScale, Quaternion.identity, Vector3.zero, tag, parent);
    }

    public static GameObject LoadPrefab<T, V>(string path, Vector3 localScale, Quaternion localRotation, Vector3 localPosition, string tag = "Default", Transform parent = null)
    where T : UnityEngine.Component, V
    where V : class
    {
        GameObject model = LoadPrefab(path);

        if (model == null)
            return null;

        model.transform.SetParent(parent);

        model.transform.localPosition = localPosition;
        model.transform.localScale = localScale;

        model.transform.localRotation = localRotation;
        model.tag = tag;

        if (model.GetComponent<T>() == null)
            model.AddComponent<T>();

        V init = model.GetComponent<T>();

        return model;
    }

    public static V LoadObject<T, V>(string path, Vector3 localScale, Quaternion localRotation, Vector3 localPosition, Transform parent, string tag)
        where T : UnityEngine.Component, V
        where V : class
    {
        return LoadObject<T>(path, localScale, localRotation, localPosition, parent, tag).GetComponent<T>();
    }

    public static GameObject LoadObject<T>(string path, Vector3 localScale, Quaternion localRotation, Vector3 localPosition, Transform parent, string tag)
    where T : UnityEngine.Component
    {
        GameObject model = GameObject.Instantiate(Resources.Load<GameObject>(path));

        if (model == null)
            return null;

        model.transform.SetParent(parent);

        model.transform.localPosition = localPosition;
        model.transform.localScale = localScale;

        model.transform.localRotation = localRotation;
        model.tag = tag;

        if (model.GetComponent<T>() == null)
            model.AddComponent<T>();

        return model;
    }

    public static void LoadAsyncPrefab(string path, System.Action callback)
    {
        ResourceRequest AsyncJob = new ResourceRequest();

        //AsyncJob = Resources.LoadAsync<T>(path);
    }
}




namespace AsyncModeling
{
    using Async = AsyncModeling.AsyncLoader;

    public static class AsyncLoader
    {
        //class AsyncJob
        //{
        //    public System.Action<>
        //}
        //public static List

        public static V LoadObject<T, V>(string path, Vector3 localScale, Quaternion localRotation, Vector3 localPosition, Transform parent, string tag)
            where T : UnityEngine.Component, V
            where V : class
        {
            return Modeling.LoadObject<T, V>(path, localScale, localRotation, localPosition, parent, tag);
        }

        public static GameObject LoadObject<T>(string path, Vector3 localScale, Quaternion localRotation, Vector3 localPosition, Transform parent, string tag)
        where T : UnityEngine.Component
        {
            return Modeling.LoadObject<T>(path, localScale, localRotation, localPosition, parent, tag);
        }

        public static GameObject LoadObject<T>(string path, string tag = "Default", Transform parent = null)
        where T : UnityEngine.Component
        {
            return Modeling.LoadObject<T>(path, Vector3.one, Quaternion.Euler(0, 0, 0), Vector3.zero, parent, tag);
        }

        public static GameObject LoadObject<T>(string path, Quaternion localRotation, string tag = "Default", Transform parent = null)
        where T : UnityEngine.Component
        {
            return Modeling.LoadObject<T>(path, Vector3.one, localRotation, Vector3.zero, parent, tag);
        }


        private static GameObject Instantiate(GameObject obj, Vector3 localScale, Quaternion localRotation, Vector3 localPosition, Transform parent, string tag)
        {
            var result = GameObject.Instantiate(obj, localPosition, localRotation, parent) as GameObject;

            result.transform.localScale = localScale;
            result.tag = tag;

            return result;
        }

        public static ResourceRequest LoadAsyncObject<T>(string path, System.Action callback) where T : UnityEngine.Object
        {
            ResourceRequest AsyncJob = new ResourceRequest();

            AsyncJob = Resources.LoadAsync<T>(path);

            AsyncJob.completed += async => callback?.Invoke();

            return AsyncJob;
        }

        public static ResourceRequest LoadAsyncObject<T>(string path, System.Action<UnityEngine.Object, System.Action<UnityEngine.Object>> DoJob,
            System.Action<UnityEngine.Object> callback) where T : UnityEngine.Object
        {
            ResourceRequest AsyncJob = new ResourceRequest();

            AsyncJob = Resources.LoadAsync<T>(path);

            AsyncJob.completed += async => DoJob?.Invoke(AsyncJob.asset, obj => callback(obj));

            return AsyncJob;
        }

        public static ResourceRequest LoadAsyncObject<T>(string path,
           System.Action<UnityEngine.Object> callback) where T : UnityEngine.Object
        {
            ResourceRequest AsyncJob = new ResourceRequest();

            AsyncJob = Resources.LoadAsync<T>(path);

            System.Action<UnityEngine.Object, System.Action<UnityEngine.Object>> DoJob = (asset, extraData) =>
            {
                var obj = GameObject.Instantiate(asset) as GameObject;
                extraData?.Invoke(obj);
            };

            AsyncJob.completed += async => DoJob?.Invoke(AsyncJob.asset, obj => callback(obj));

            return AsyncJob;
        }

        public static ResourceRequest LoadAsync<T>(string path, System.Action<UnityEngine.Object> callback) where T : UnityEngine.Object
        {
            ResourceRequest AsyncJob = new ResourceRequest();

            AsyncJob = Resources.LoadAsync<T>(path);

            AsyncJob.completed += async => callback?.Invoke(AsyncJob.asset);

            return AsyncJob;
        }

        public static T AddCallBack<T>(this T target, System.Action callback) where T : ResourceRequest
        {
            target.completed += async => callback?.Invoke();
            return target;
        }


        public static void LoadAsyncObject(string path, Quaternion localRotation, string tag = GameTag.Player, Transform parent = null)
        {
            AsyncModeling.AsyncLoader.LoadAsyncObject<GameObject>(path,
               () => Modeling.LoadPrefab(path, tag, parent));
        }

        public static void LoadAsyncObject(string path, System.Action callback = null, string tag = "Default", Transform parent = null)
        {
            AsyncModeling.AsyncLoader.LoadAsyncObject<GameObject>(path,
             () =>
             {
                 Modeling.LoadPrefab(path, tag, parent);

                 callback?.Invoke();
             });
        }
        //public static GameObject LoadAsyncObject(string path)
        //{

        //}
    }
}