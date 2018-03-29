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

    public NoiseGenerator NoiseMap;
    public TextureGenerator SplatMap;
    public LakeGenerator WaterMap;
    public VegetationGenerator PlantMap;

    /*(public TextureGenerator TextureMap;
    public LakeGenerator WaterMap;
    public TreeGenerator TreeMap;*/

    [HideInInspector]
    public GDObject ObjectToModify;

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
