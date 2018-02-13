using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private Terrain _Terrain;
    private TerrainData _Data;

    [SerializeField]
    private float HeightScale;

    public void Start()
    {
        _Terrain = gameObject.AddComponent<Terrain>();
        _Data = new TerrainData();       
    }

    public void GenerateTerrain()
    {
        _Data.heightmapResolution = NoiseMapGenerator.Get().Width + 1;
        _Data.size = new Vector3(NoiseMapGenerator.Get().Width, HeightScale, NoiseMapGenerator.Get().Height);
        _Data.SetHeights(0, 0, NoiseMapGenerator.Get().GetNoiseData());

        _Terrain.terrainData = _Data;
    }
}
