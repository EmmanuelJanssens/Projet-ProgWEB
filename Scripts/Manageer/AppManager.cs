using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Keeps track of every usable element between panels
/// \\Terrain Generated
/// \\List of used Textures
/// \\Texture of the noiseMap
/// \\...
/// \\ Can Be used for loading
/// </summary>
public class AppManager : MonoBehaviour
{
    public static AppManager current;


    public GameObject DarkUI;

    [HideInInspector]
    public Texture2D CNoiseMap;

    public NoiseGenerator NoiseMap;
    public TextureGenerator TextureMap;
    public LakeGenerator WaterMap;
    public TreeGenerator TreeMap;

    [HideInInspector]
    public Terrain CTerrain;

    [HideInInspector]
    public bool NoiseMapGenerated;

    [HideInInspector]
    public bool TerrainGenerated;

    [HideInInspector]
    public bool TextureGenrated;

    [HideInInspector]
    public bool WaterGenerated;

    [HideInInspector]
    public bool RocksGenerated;

    [HideInInspector]
    public bool TreeGenerated;

    [HideInInspector]
    public bool PlantGenerated;



    private void Start()
    {
        if(current == null)
        {
            current = this;
        }
    }

    public static AppManager Get { get { return current; } }


    /// <summary>
    /// Load Data from a file
    /// </summary>
    public void OnLoad()
    {

    }

    /// <summary>
    /// Save data to a file
    /// </summary>
    public void OnClose()
    {

    }
}
