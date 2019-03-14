using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassA : MonoBehaviour, ICantSex
{
    public int a = 1;
    

    public void SetUp(bool param)
    {
        Debug.LogError(param);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
