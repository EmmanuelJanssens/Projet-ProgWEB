using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Data class that will be used to generate the trees onto the therrain
/// </summary>
public class GDObject
{

    /// <summary>
    /// The mode of application
    /// </summary>
    public enum ApplicationMode { Height, Slope, Orientation, HeightRange, SlopeRange };

    /// <summary>
    /// Identifier
    /// </summary>
    public int ID;

    /// <summary>
    /// Its name 
    /// </summary>
    public string ObjectName;

    /// <summary>
    /// What will be displayed for the user
    /// </summary>
    public Sprite ui_sprite;

    /// <summary>
    /// The color it should be when generated on the NoiseMap
    /// </summary>
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

    public int density;
    public float clump;
    public int details_resolution;

    //How to apply rules
    public ApplicationMode Mode;
}
