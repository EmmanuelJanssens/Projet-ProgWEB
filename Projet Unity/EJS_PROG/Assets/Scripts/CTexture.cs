using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Data container for a texture 
/// Those values are used to apply rules for the weight distribution 
/// when applying the splat map on the terrain
/// 
/// Sprites are used for UI visualisation
/// Textures are used to paint on the terrain
/// </summary>
[SerializeField]
public class CTexture : MonoBehaviour
{
    public int ID;

    public string name;
    public Sprite sprite;
    public Texture2D texture;
    public GameObject localGameobject;

    public float steepness;
    public float height;
    public float orientation;
    public float influence;
}
