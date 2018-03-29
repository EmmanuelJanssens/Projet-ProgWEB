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
public class GDTexture : GDObject
{
    public Texture2D texture;
}
