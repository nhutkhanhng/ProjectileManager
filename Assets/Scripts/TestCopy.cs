using UnityEngine;

public static class Extionsion
{
    public static T Instantiate<T>(this UnityEngine.Object unityObject, T t) where T : UnityEngine.Object
    {
        return UnityEngine.Object.Instantiate(t) as T;
    }
}
public class TestCopy : MonoBehaviour
{
    // Instantiate a Prefab with an attached Missile script
    public ClassA projectile;
    public GameObject obj;
    void Update()
    {
        // Ctrl was pressed, launch a projectile
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Instantiate the projectile at the position and rotation of this transform
            ClassA clone = Extionsion.Instantiate<ClassA>(this.obj, projectile);

            // Set the missiles timeout destructor to 5
            //clone.timeoutDestructor = 5;
        }
    }
}