using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The data that will be atached to the actual game Object
/// </summary>
public class COObject : MonoBehaviour
{
    /// <summary>
    /// enumeration of Available Biomes
    /// </summary>
    public enum BIOME { DESERT, TUNDRA, TAIGA, GRASSLAND, PLATEAU, TROPICAL, ALPINE }

    /// <summary>
    /// The type of Object 
    /// </summary>
    public enum TYPE { TREE,FLOWER,ROCK }

    public int ID;
    public string Name;

    /// <summary>
    /// biome apartenance
    /// </summary>
    public BIOME biome;


    /// <summary>
    /// What type of object it IS
    /// </summary>
    public TYPE type;
    
}
