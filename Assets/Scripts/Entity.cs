using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEntity
{
    int ID { get; set; }
    string Tag { get; set; }
    bool IsDead { get; set; }
    float HP { get; set; }
}

public struct InfoEntity
{
    public Vector3 Position;
    public Vector3 BodyBlock;
}

public class Entity : MonoBehaviour, IEntity
{
    private int _Id;
    private string _Tag;
    public int ID { get { return _Id; } set { _Id = value; } }
    public string Tag { get { return _Tag; } set { _Tag = value; } }

    public bool IsDead { get; set; }
    public float HP { get; set; }

    public Vector2 BodyBlock { get; set; } = Vector2.one;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
