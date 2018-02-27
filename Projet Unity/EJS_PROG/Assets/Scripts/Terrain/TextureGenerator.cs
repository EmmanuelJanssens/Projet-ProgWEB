using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Texture generator imports the textures into the terrain data splatPrototypes
/// The imported textures are used to paint the terrain in the second step of the Generete Coroutine
/// Rules for the weights are taken from the GUI values the user has chosen
/// </summary>
public class TextureGenerator : MonoBehaviour {

    public LoadAvailableTextures loader;
    public NoiseMapGenerator noise;
    public ImportTextures import;

    //List of all textures loaded
    List<CTexture> _Textures;

    public Terrain terrain;
    private TerrainData data;
    

	/// <summary>
    /// Function called when pressing the generate button
    /// </summary>
    public void Generate()
    {
        StartCoroutine(IGenerate());
    }

    /// <summary>
    /// Coroutine that generates the textures on the terrain
    /// </summary>
    /// <returns></returns>
    IEnumerator IGenerate()
    {
        //Only generate if importation was successfull
        if(loader._imported)
        {
            //Import texture into terrain painter
            SplatPrototype[] Textures = new SplatPrototype[import.addedTextures.Count];
            
            for(int i = 0; i < import.addedTextures.Count; i++)
            {
                Textures[i] = new SplatPrototype();
                Textures[i].texture = import.addedTextures[i].texture;
                Textures[i].tileSize = new Vector2(1, 1);
            }

            data = terrain.terrainData;
            data.splatPrototypes = Textures;
            data.RefreshPrototypes();


            //Generate textures
            float[,,] splatmapData = new float[data.alphamapWidth, data.alphamapHeight, data.alphamapLayers];

            for (int y = 0; y < data.alphamapHeight; y++)
            {
                for (int x = 0; x < data.alphamapWidth; x++)
                {
                    //Rules for splat mapping
                }
            }

            data.SetAlphamaps(0, 0, splatmapData);


        }
        yield return null;
    }

}
