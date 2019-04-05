using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsyncModeling;

public class ScheduleLoader : MonoBehaviour
{
    public Dictionary<string, UnityEngine.Object> Cache = new Dictionary<string, Object>();
    public ResourceRequest Loader(string Path)
    {
        // Callback action
        System.Action<UnityEngine.Object, System.Action<UnityEngine.Object>> DoJob = (asset, callback) =>
        {
            Object obj = null;
            this.Cache.TryGetValue(Path, out obj);

            if (obj == null)
            {
                this.Cache.Add(Path, obj);
                obj = Modeling.LoadPrefab(asset);
            }

            callback?.Invoke(obj);
        };

        var AsysnJob = AsyncLoader.LoadAsyncObject<GameObject>(Path,
           DoJob,   callback: beLoaded =>
           {
               //var AI = (beLoaded as GameObject).GetComponent<ClassA>();
               //Debug.LogError(AI.name);

               //Nothi
           }
           );

        return AsysnJob;
    }
    
    public T Load<T>(string Path, bool IsIMMEDIATE = false) where T: UnityEngine.Object
    {
        if (this.Cache == null)
            this.Cache = new Dictionary<string, Object>();

        Object Obj = null;
        // Cái TryGetValue sẽ tự Add Key luôn.
        if (this.Cache.TryGetValue(Path, out Obj))
        {
            if (Obj == null)
            {
                Debug.LogError("34567i9o0");
                // Nếu chưa Loadxong mà Return ngay lúc dó thì sao ?
                /* return Async.asset as GameObject; */
                if (IsIMMEDIATE == false)
                {
                    ResourceRequest Async = Loader(Path);
                    Async.completed += async =>
                    {
                        this.Cache[Path] = Async.asset;
                        // Chổ này cái Obj bị hủy thì có tính là Null không ta ?
                        // À. Nó tính là Null nè;
                        if (Obj != null)
                            Obj = Async.asset;
                    };
                    return Async.asset as T;
                }
                // Vậy nếu cần ngay thì sao ?!.
                // Chuyển sang Load ngay lập tức và Return luôn
                else
                {
                    Obj = Modeling.LoadResource<T>(Path);
                    this.Cache.Add(Path, Obj);
                    return Obj as T;
                }
            }
        }
        else
        {
            // 
            Debug.LogError("34567i9o0");
            // Nếu chưa Loadxong mà Return ngay lúc dó thì sao ?
            /* return Async.asset as GameObject; */
            if (IsIMMEDIATE == false)
            {
                ResourceRequest Async = Loader(Path);
                Async.completed += async =>
                {
                    this.Cache[Path] = Async.asset;
                    // Chổ này cái Obj bị hủy thì có tính là Null không ta ?
                    // À. Nó tính là Null nè;
                    if (Obj != null)
                        Obj = Async.asset;
                };
                return Async.asset as T;
            }
            // Vậy nếu cần ngay thì sao ?!.
            // Chuyển sang Load ngay lập tức và Return luôn
            else
            {
                Obj = Modeling.LoadResource<T>(Path);
                this.Cache.Add(Path, Obj);
                return Obj as T;
            }
        }


        return Obj as T;
    }

    // Start is called before the first frame update
    void Start()
    {
        var loaded = Load<Material>("Materials/Blue");

        Debug.LogError(loaded);
    }

    // Update is called once per frame
}
