using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDObject
{
    public enum ApplicationMode { Height, Slope, Orientation, HeightRange, SlopeRange };

    //Object data
    public int ID;
    public string ObjectName;

    //Interface Propreties
    public GameObject ui_object;
    public Sprite ui_sprite;
    public Color32 avgColor;

    //Rules to be applied
    public float steepness;
    public float height;
    public float orientation;
    public float influence = 100;

    public float minheight;
    public float maxheight;

    public float minslope;
    public float maxslope;

    //How to apply rules
    public ApplicationMode Mode;
}
