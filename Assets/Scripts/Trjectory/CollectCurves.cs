using System;
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


public class Box : IEquatable<Box>
{

    public Box(int h, int l, int w)
    {
        this.Height = h;
        this.Length = l;
        this.Width = w;
    }
    public int Height { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }

    // Defines equality using the
    // BoxSameDimensions equality comparer.
    public bool Equals(Box other)
    {
        if (new BoxSameDimensions().Equals(this, other))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class BoxCollection<T> : ICollection<T>
{
    // The generic enumerator obtained from IEnumerator<Box>
    // by GetEnumerator can also be used with the non-generic IEnumerator.
    // To avoid a naming conflict, the non-generic IEnumerable method
    // is explicitly implemented.

    public IEnumerator<T> GetEnumerator()
    {
        return new BoxEnumerator<T>(this);
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return new BoxEnumerator<T>(this);
    }

    // The inner collection to store objects.
    private List<T> innerCol;

    public BoxCollection()
    {
        innerCol = new List<T>();
    }

    // Adds an index to the collection.
    public T this[int index]
    {
        get { return (T)innerCol[index]; }
        set { innerCol[index] = value; }
    }

    // Determines if an item is in the collection
    // by using the BoxSameDimensions equality comparer.
    public bool Contains(T item)
    {
        bool found = false;

        foreach (T bx in innerCol)
        {
            // Equality defined by the Box
            // class's implmentation of IEquitable<T>.
            if (bx.Equals(item))
            {
                found = true;
            }
        }

        return found;
    }

    // Determines if an item is in the 
    // collection by using a specified equality comparer.
    public bool Contains(T item, EqualityComparer<T> comp)
    {
        bool found = false;

        foreach (T bx in innerCol)
        {
            if (comp.Equals(bx, item))
            {
                found = true;
            }
        }

        return found;
    }

    // Adds an item if it is not already in the collection
    // as determined by calling the Contains method.
    public void Add(T item)
    {

        if (!Contains(item))
        {
            innerCol.Add(item);
        }
        else
        {
        }
    }

    public void Clear()
    {
        innerCol.Clear();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException("The array cannot be null.");
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
        if (Count > array.Length - arrayIndex + 1)
            throw new ArgumentException("The destination array has fewer elements than the collection.");

        for (int i = 0; i < innerCol.Count; i++)
        {
            array[i + arrayIndex] = innerCol[i];
        }
    }

    public int Count
    {
        get
        {
            return innerCol.Count;
        }
    }

    public bool IsReadOnly
    {
        get { return false; }
    }

    public bool Remove(T item)
    {
        bool result = false;

        // Iterate the inner collection to 
        // find the box to be removed.
        for (int i = 0; i < innerCol.Count; i++)
        {

            T curBox = (T)innerCol[i];

            if (curBox.Equals(item))
            {
                innerCol.RemoveAt(i);
                result = true;
                break;
            }
        }
        return result;
    }
}


// Defines the enumerator for the Boxes collection.
// (Some prefer this class nested in the collection class.)
public class BoxEnumerator<T> : IEnumerator<T>
{
    private BoxCollection<T> _collection;
    private int curIndex;
    private T curBox;


    public BoxEnumerator(BoxCollection<T> collection)
    {
        _collection = collection;
        curIndex = -1;
        curBox = default(T);

    }

    public bool MoveNext()
    {
        //Avoids going beyond the end of the collection.
        if (++curIndex >= _collection.Count)
        {
            return false;
        }
        else
        {
            // Set current box to next item in collection.
            curBox = _collection[curIndex];
        }
        return true;
    }

    public void Reset() { curIndex = -1; }

    void IDisposable.Dispose() { }

    public T Current
    {
        get { return curBox; }
    }


    object IEnumerator.Current
    {
        get { return Current; }
    }

}


// Defines two boxes as equal if they have the same dimensions.
public class BoxSameDimensions : EqualityComparer<Box>
{

    public override bool Equals(Box b1, Box b2)
    {
        if (b1.Height == b2.Height && b1.Length == b2.Length
                            && b1.Width == b2.Width)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public override int GetHashCode(Box bx)
    {
        int hCode = bx.Height ^ bx.Length ^ bx.Width;
        return hCode.GetHashCode();
    }

}

// Defines two boxes as equal if they have the same volume.
public class BoxSameVol : EqualityComparer<Box>
{

    public override bool Equals(Box b1, Box b2)
    {
        if ((b1.Height * b1.Length * b1.Width) ==
                (b2.Height * b2.Length * b2.Width))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public override int GetHashCode(Box bx)
    {
        int hCode = bx.Height ^ bx.Length ^ bx.Width;
        return hCode.GetHashCode();
    }
}

