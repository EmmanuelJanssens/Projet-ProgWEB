using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject GOTerrain;

    private TerrainCollider _terrainCollider;
    private Terrain _terrain;
    private TerrainData _terrainData;

    [SerializeField]
    private float _heightScale = 20f;

    public float HeightScale { get { return _heightScale; } set { _heightScale = value; } }

    public void Start()
    {
        _terrainData = new TerrainData();       
    }

    public void GenerateTerrain()
    {
        _terrain = GOTerrain.GetComponent<Terrain>();
        _terrainCollider = GOTerrain.GetComponent<TerrainCollider>();

        _terrainData.heightmapResolution = NoiseMapGenerator.Get().Width + 1;
        _terrainData.size = new Vector3(NoiseMapGenerator.Get().Width, _heightScale, NoiseMapGenerator.Get().Height);
        _terrainData.SetHeights(0, 0, NoiseMapGenerator.Get().GetNoiseData());

        _terrain.terrainData = _terrainData;
        _terrainCollider.terrainData = _terrainData;
      
    }
}
