//Attach this script to the GameObject you would like to have mouse hovering detected on
//This script outputs a message to the Console when the mouse pointer is currently detected hovering over the GameObject and also when the pointer leaves.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface ReactWithMouse
{
    void OnMouseUp();
    void OnMouseDrag();
    void OnMouseDown();
}
public class TestMouse : MonoBehaviour, IDropHandler, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Debug.LogError("ASDJKASDLK");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.LogError("OnDrop");
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    Debug.LogError("OnBeginDrag");
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Debug.LogError("OnDrag");
    //}

    //public void OnDrop(PointerEventData eventData)
    //{
    //    Debug.LogWarning("OnDrop");
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    Debug.LogError("OnEndDrag");
    //}

    //public void OnMouseUp()
    //{
    //    Debug.LogError("OnMouseUp");
    //}

    //public void OnMouseDrag()
    //{
    //    Debug.LogError("OnMouseDrag");
    //}

    //public void OnMouseDown()
    //{
    //    Debug.LogError("OnMouseDown");
    //}
    ////Detect if the Cursor starts to pass over the GameObject
    //public void OnPointerEnter(PointerEventData pointerEventData)
    //{
    //    //Output to console the GameObject's name and the following message
    //        Debug.Log("Cursor Entering " + name + " GameObject");
    //}

    ////Detect when Cursor leaves the GameObject
    //public void OnPointerExit(PointerEventData pointerEventData)
    //{
    //    //Output the following message with the GameObject's name
    //    Debug.Log("Cursor Exiting " + name + " GameObject");
    //}

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.LogError("Cursor Up" + eventData.selectedObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.LogError("OnSelect");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.LogError("Submit");
    }
};