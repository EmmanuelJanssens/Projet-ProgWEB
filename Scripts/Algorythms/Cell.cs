using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Rect that contains a list of positions
/// </summary>
public class Cell
{

    public Rect rect;

    public List<Vector3> points;

    public Cell( Rect r)
    {
        points = new List<Vector3>();
        rect = r;
    }

    public void Init(Rect r)
    {
        points = new List<Vector3>();
        rect = r;
    }
}
