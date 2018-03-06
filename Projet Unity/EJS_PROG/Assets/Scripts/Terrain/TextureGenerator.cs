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

    public static TextureGenerator current;

    public LoadAvailableTextures Loader;
    public NoiseMapGenerator Noise;
    public ImportTextures Import;

    //List of all textures loaded
    List<CTexture> _Textures;

    public Terrain _terrain;
    private TerrainData _terrainData;
    private float[,,] _splatmapData;
    private float[] _weight;


    public static TextureGenerator Get { get { return current; } }

    public void Start()
    {
        if (current == null)
            current = this;
    }

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
        if(Loader.Imported)
        {
            //Import texture into terrain painter
            SplatPrototype[] Textures = new SplatPrototype[Import.ImportedTextures.Count];
            
            for(int i = 0; i < Import.ImportedTextures.Count; i++)
            {
                Textures[i] = new SplatPrototype();
                Textures[i].texture = Import.ImportedTextures[i].texture;
                Textures[i].tileSize = new Vector2(1, 1);
            }

            _terrainData = _terrain.terrainData;
            _terrainData.splatPrototypes = Textures;
            _terrainData.RefreshPrototypes();


            //Generate textures
            _splatmapData = new float[_terrainData.alphamapWidth, _terrainData.alphamapHeight, _terrainData.alphamapLayers];

            //Each CTexture has has 3 elements Height,Steepness,Orientation (Other to be added)
            //Those values will influence the base formula
            //HeightInfluence + SteepnessInfluence + OrientationInfluence
            //How to
            //\\Calculatte  -HeightInfluence        
            //\\            -SteepnessInfluence
            //\\            -OrientationInfluence
            //First test with heightOnly

            _Textures = ImportTextures.Get.ImportedTextures;
            for (int y = 0; y < _terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < _terrainData.alphamapWidth; x++)
                {
                    _weight = new float[_terrainData.alphamapLayers];
                    float y_01 = (float)y / (float)_terrainData.alphamapHeight;
                    float x_01 = (float)x / (float)_terrainData.alphamapWidth;

                    float height = _terrainData.GetHeight(Mathf.RoundToInt(y_01 * _terrainData.heightmapHeight), Mathf.RoundToInt(x_01 * _terrainData.heightmapWidth));                    //Get Steepness of the terrain
                    float steepness = _terrainData.GetSteepness(x_01, y_01);
                    Vector3 direction = _terrainData.GetInterpolatedNormal(x, y);

                    for(int i = 0; i < _terrainData.alphamapLayers; i++)
                    {
                        CTexture textureProp = _Textures[i];

                        switch(textureProp.Mode)
                        {
                            case CTexture.ApplicationMode.Height:
                                if (height > textureProp.height)
                                    _weight[i] = textureProp.influence;
                                break;
                            case CTexture.ApplicationMode.Slope:
                                if (steepness < textureProp.steepness)
                                    _weight[i] = textureProp.influence;
                                break;
                            case CTexture.ApplicationMode.Orientation:
                                if (direction.z < textureProp.orientation)
                                    _weight[i] = textureProp.influence;
                                break;
                            case CTexture.ApplicationMode.HeightRange:
                                if (height < textureProp.minheight && height > textureProp.maxheight)
                                    _weight[i] = textureProp.influence;
                                break;
                            case CTexture.ApplicationMode.SlopeRange:
                                if (steepness > textureProp.minslope && steepness < textureProp.maxslope)
                                    _weight[i] = textureProp.influence;
                                break;
                            default:
                                break;
                        }

                    }

                    //Rules for splat mapping
                    float z;

                    z = _weight.Sum();

                    for (int i = 0; i < _terrainData.alphamapLayers; i ++)
                    {
                        _weight[i] /= z;
                        _splatmapData[x, y, i] = _weight[i] ;
                    }
                }
            }
            
            //Appliquer les textures
            _terrainData.SetAlphamaps(0, 0, _splatmapData);


        }
        yield return null;
    }

}
