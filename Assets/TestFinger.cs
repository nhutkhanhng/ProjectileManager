using DigitalRubyShared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFinger : MonoBehaviour
{
    private TapGestureRecognizer tapGesture;
    private TapGestureRecognizer doubleTapGesture;
    private TapGestureRecognizer tripleTapGesture;
    private LongPressGestureRecognizer longPressGesture;


    public GameObject ObjectDragged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        Debug.LogError("OnMouseEnter");
    }

    private void OnMouseDown()
    {
        Debug.LogError("OnMouseDown");
    }

    private void OnMouseDrag()
    {
        Debug.LogError("OnMouseDrag");
    }

    private void OnMouseUpAsButton()
    {
        Debug.LogError("OnMouseAsButton");
    }

    private void OnMouseUp()
    {
        Debug.LogError("OnMouseUp");

    }
}
