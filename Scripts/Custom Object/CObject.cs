using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CObject
{
    //Object data
    public int ID;
    public string oo;
    public string ObjectName;
    public string Description;

    //Interface Propreties
    public GameObject ui_object;
    public Sprite ui_sprite;
    public Color avgColor;

    //Rules to be applied
    public float steepness;
    public float height;
    public float orientation;
    public float influence;

    public float minheight;
    public float maxheight;

    public float minslope;
    public float maxslope;

    //How to apply rules
    public enum ApplicationMode { Height, Slope, Orientation, HeightRange, SlopeRange };
    public ApplicationMode Mode;
}
