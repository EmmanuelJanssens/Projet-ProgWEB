using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    //Gameobject on wich the terrain will be applied
    public GameObject GOTerrain;
    
    //Data of the terrain
    private TerrainCollider _terrainCollider;
    private Terrain _terrain;
    private TerrainData _terrainData;

    private float _heightScale = 90f;

    public float HeightScale { get { return _heightScale; } set { _heightScale = value; } }

    public void Start()
    {
        _terrainData = new TerrainData();       
    }

    /// <summary>
    /// Applies the noie scale from the NoiseMap generated beforeHands
    /// </summary>
    public void GenerateTerrain()
    {
        _terrain = GOTerrain.GetComponent<Terrain>();
        _terrainCollider = GOTerrain.GetComponent<TerrainCollider>();

        _terrainData.heightmapResolution = NoiseGenerator.Get().Width + 1;
        _terrainData.size = new Vector3(NoiseGenerator.Get().Width, _heightScale, NoiseGenerator.Get().Height);
        _terrainData.SetHeights(0, 0, NoiseGenerator.Get().GetNoiseData());

        _terrain.terrainData = _terrainData;
        _terrainCollider.terrainData = _terrainData;     
    }
}
