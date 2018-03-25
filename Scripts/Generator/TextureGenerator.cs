using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Texture generator imports the textures into the terrain data splatPrototypes
/// The imported textures are used to paint the terrain in the second step of the Generete Coroutine
/// Rules for the weights are taken from the GUI values the user has chosen
/// </summary>
public class TextureGenerator : MonoBehaviour
{

    public static TextureGenerator current;

    public Transform Container;

    public ImportElementIntoPanel Import;


    //List of all textures loaded
    List<CTexture> _Textures;
    List<SplatPrototype> SplatPrototypes;


    private float[,,] _splatmapData;


    public static TextureGenerator Get { get { return current; } }


    public void Start()
    {
        if (current == null)
            current = this;

    }

    public void RemoveFromSplatPrototypes(int index)
    {
        SplatPrototypes.RemoveAt(index);
        AppManager.Get.NoiseMap.terrainData.RefreshPrototypes();
    }
    /// <summary>
    /// Function called when pressing the generate button
    /// </summary>
    public void Generate()
    {
        StartCoroutine(GenerateTextureOnNoiseMap());
        StartCoroutine(IGenerate());
    }

    IEnumerator GenerateTextureOnNoiseMap()
    {
        //Only generate if importation was successfull
        if (Import.ElementsAreReady)
        {
            AppManager.Get.NoiseMap.GenerateNoiseBW();

            //Import texture into terrain painter
            _Textures = new List<CTexture>();

            for (int i = 0; i < Import.ReadyElements.Count; i++)
            {
                _Textures.Add((CTexture)Import.ReadyElements[i]);
            }


            for (int y = 0; y < AppManager.Get.NoiseMap.terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < AppManager.Get.NoiseMap.terrainData.alphamapWidth; x++)
                {
                    float y_01 = (float)y / (float)AppManager.Get.NoiseMap.terrainData.alphamapHeight;
                    float x_01 = (float)x / (float) AppManager.Get.NoiseMap.terrainData.alphamapWidth;

                    int ix = Mathf.RoundToInt(x_01 * AppManager.Get.NoiseMap.terrainData.heightmapWidth);
                    int iy = Mathf.RoundToInt(y_01 * AppManager.Get.NoiseMap.terrainData.heightmapHeight);

                    float height = AppManager.Get.NoiseMap.terrainData.GetHeight(ix, iy);                    //Get Steepness of the terrain
                    float steepness = AppManager.Get.NoiseMap.terrainData.GetSteepness(x_01, y_01);
                    Vector3 direction = AppManager.Get.NoiseMap.terrainData.GetInterpolatedNormal(x, y);

                    for (int i = 0; i < _Textures.Count; i++)
                    {
                        CTexture textureProp = _Textures[i];

                        switch (textureProp.Mode)
                        {
                            case CTexture.ApplicationMode.Height:
                                if (height > textureProp.height)
                                {
                                    AppManager.Get.NoiseMap.PixelColor[iy * AppManager.Get.NoiseMap.Width + ix] = _Textures[i].avgColor ;
                                }
                                break;
                            case CTexture.ApplicationMode.Slope:
                                //Slope 0 to 90 degrees
                                if (steepness < textureProp.steepness)
                                {
                                    AppManager.Get.NoiseMap.PixelColor[iy * AppManager.Get.NoiseMap.Width + ix] = _Textures[i].avgColor;
                                }
                                break;
                            case CTexture.ApplicationMode.Orientation:
                                if (direction.z < textureProp.orientation)
                                {
                                    AppManager.Get.NoiseMap.PixelColor[iy * AppManager.Get.NoiseMap.Width + ix] = _Textures[i].avgColor;
                                }
                                break;
                            case CTexture.ApplicationMode.HeightRange:
                                if (height > textureProp.minheight && height < textureProp.maxheight)
                                {
                                    AppManager.Get.NoiseMap.PixelColor[iy * AppManager.Get.NoiseMap.Width + ix] = _Textures[i].avgColor;
                                }
                                break;
                            case CTexture.ApplicationMode.SlopeRange:
                                if (steepness > textureProp.minslope && steepness < textureProp.maxslope)
                                {
                                    AppManager.Get.NoiseMap.PixelColor[iy * AppManager.Get.NoiseMap.Width + ix] = _Textures[i].avgColor;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            AppManager.Get.NoiseMap.GenerateTexture();

        }
        yield return null;
    }
    /// <summary>
    /// Coroutine that generates the textures on the terrain
    /// </summary>
    /// <returns></returns>
    IEnumerator IGenerate()
    {
        //Only generate if importation was successfull
        if(Import.ElementsAreReady)
        {
            SplatPrototypes = new List<SplatPrototype>();

            //Import texture into terrain painter
            _Textures = new List<CTexture>();

            for (int i = 0; i < Import.ReadyElements.Count; i++)
            {
                SplatPrototypes.Add(new SplatPrototype());
                CTexture texture = Import.ReadyElements[i] as CTexture;

                SplatPrototypes[i].texture = texture.texture;
                SplatPrototypes[i].tileSize = new Vector2(1, 1);

                _Textures.Add(texture);
            }

            AppManager.Get.NoiseMap.terrainData.splatPrototypes = SplatPrototypes.ToArray(); ;
            AppManager.Get.NoiseMap.terrainData.RefreshPrototypes();

            //Generate textures
            _splatmapData = new float[AppManager.Get.NoiseMap.terrainData.alphamapWidth, AppManager.Get.NoiseMap.terrainData.alphamapHeight, AppManager.Get.NoiseMap.terrainData.alphamapLayers];

            //Each CTexture has has 3 elements Height,Steepness,Orientation (Other to be added)
            //Those values will influence the base formula
            //HeightInfluence + SteepnessInfluence + OrientationInfluence
            //How to
            //\\Calculatte  -HeightInfluence        
            //\\            -SteepnessInfluence
            //\\            -OrientationInfluence
            //First test with heightOnly

            float[] _weight;
            for (int y = 0; y < AppManager.Get.NoiseMap.terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < AppManager.Get.NoiseMap.terrainData.alphamapWidth; x++)
                {
                    _weight = new float[AppManager.Get.NoiseMap.terrainData.alphamapLayers];
                    float y_01 = (float)y / (float)AppManager.Get.NoiseMap.terrainData.alphamapHeight;
                    float x_01 = (float)x / (float)AppManager.Get.NoiseMap.terrainData.alphamapWidth;

                    float height = AppManager.Get.NoiseMap.terrainData.GetHeight(Mathf.RoundToInt(y_01 * AppManager.Get.NoiseMap.terrainData.heightmapHeight), Mathf.RoundToInt(x_01 * AppManager.Get.NoiseMap.terrainData.heightmapWidth));                    //Get Steepness of the terrain
                    float steepness = AppManager.Get.NoiseMap.terrainData.GetSteepness(x_01, y_01);
                    Vector3 direction = AppManager.Get.NoiseMap.terrainData.GetInterpolatedNormal(x, y);

                    for(int i = 0; i < AppManager.Get.NoiseMap.terrainData.alphamapLayers; i++)
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
                                if (height > textureProp.minheight && height < textureProp.maxheight)
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

                    for (int i = 0; i < AppManager.Get.NoiseMap.terrainData.alphamapLayers; i ++)
                    {
                        _weight[i] /= z;
                        _splatmapData[x, y, i] = _weight[i] ;
                    }
                }
            }

            //Appliquer les textures
            AppManager.Get.NoiseMap.terrainData.SetAlphamaps(0, 0, _splatmapData);


        }
        yield return null;
    }



}
