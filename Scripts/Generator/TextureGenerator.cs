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

    public ChooseAssets Import;


    //List of all textures loaded
    List<GDTexture> _Textures;
    List<SplatPrototype> SplatPrototypes;

    private float[,,] _splatmapData;


    public static TextureGenerator Get { get { return current; } }

    public GameObject TerrainToApplyTexture;

    [HideInInspector]
    public Terrain terrain;

    [HideInInspector]
    public TerrainData terrainData;

    [HideInInspector]
    public TerrainCollider terrainCollider;

    public void Start()
    {
        if (current == null)
            current = this;

        terrain = TerrainToApplyTexture.GetComponent<Terrain>();
        terrainCollider = TerrainToApplyTexture.GetComponent<TerrainCollider>();
        terrainData = new TerrainData();
    }

    public void RemoveFromSplatPrototypes(int index)
    {
        SplatPrototypes.RemoveAt(index);
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
        AppManager.Get.NoiseMap.GenerateNoiseBW();

        //Import texture into terrain painter
        _Textures = new List<GDTexture>();

        for (int i = 0; i < Import.ChoosenTextures.Count; i++)
        {
            _Textures.Add(Import.ChoosenTextures[i] as GDTexture);
        }


        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float) terrainData.alphamapWidth;

                int ix = Mathf.RoundToInt(x_01 * terrainData.heightmapWidth);
                int iy = Mathf.RoundToInt(y_01 * terrainData.heightmapHeight);

                float height = terrainData.GetHeight(ix, iy);                    //Get Steepness of the terrain
                float steepness = terrainData.GetSteepness(x_01, y_01);
                Vector3 direction = terrainData.GetInterpolatedNormal(x, y);

                for (int i = 0; i < _Textures.Count; i++)
                {
                    GDTexture textureProp = _Textures[i];

                    switch (textureProp.Mode)
                    {
                        case GDTexture.ApplicationMode.Height:
                            if (height > textureProp.height)
                            {
                                AppManager.Get.NoiseMap.PixelColor[iy * AppManager.Get.NoiseMap.Width + ix] = _Textures[i].avgColor ;

                            }
                            break;
                        case GDTexture.ApplicationMode.Slope:
                            //Slope 0 to 90 degrees
                            if (steepness < textureProp.steepness)
                            {
                                AppManager.Get.NoiseMap.PixelColor[iy * AppManager.Get.NoiseMap.Width + ix] = _Textures[i].avgColor;
                            }
                            break;
                        case GDTexture.ApplicationMode.Orientation:
                            if (direction.z < textureProp.orientation)
                            {
                                AppManager.Get.NoiseMap.PixelColor[iy * AppManager.Get.NoiseMap.Width + ix] = _Textures[i].avgColor;
                            }
                            break;
                        case GDTexture.ApplicationMode.HeightRange:
                            if (height > textureProp.minheight && height < textureProp.maxheight)
                            {
                                AppManager.Get.NoiseMap.PixelColor[iy * AppManager.Get.NoiseMap.Width + ix] = _Textures[i].avgColor;
                            }
                            break;
                        case GDTexture.ApplicationMode.SlopeRange:
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
        yield return null;
    }
    /// <summary>
    /// Coroutine that generates the textures on the terrain
    /// </summary>
    /// <returns></returns>
    IEnumerator IGenerate()
    {

        terrainData.heightmapResolution = AppManager.Get.NoiseMap.Width;
        terrainData.size = new Vector3(AppManager.Get.NoiseMap.Width, AppManager.Get.NoiseMap.WorldScale, AppManager.Get.NoiseMap.Height);
        terrainData.SetHeights(0, 0, AppManager.Get.NoiseMap.Noise);

        terrain.terrainData = terrainData;
        terrainCollider.terrainData = terrainData;


        SplatPrototypes = new List<SplatPrototype>();

        //Import texture into terrain painter
        _Textures = new List<GDTexture>();

        for (int i = 0; i < Import.ChoosenTextures.Count; i++)
        {
            SplatPrototypes.Add(new SplatPrototype());
            GDTexture texture = Import.ChoosenTextures[i] as GDTexture;

            SplatPrototypes[i].texture = texture.texture;
            SplatPrototypes[i].tileSize = new Vector2(1, 1);

            _Textures.Add(texture);
        }

        terrainData.splatPrototypes = SplatPrototypes.ToArray(); ;
        terrainData.RefreshPrototypes();

        //Generate textures
        _splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        //Each CTexture has has 3 elements Height,Steepness,Orientation (Other to be added)
        //Those values will influence the base formula
        //HeightInfluence + SteepnessInfluence + OrientationInfluence
        //How to
        //\\Calculatte  -HeightInfluence        
        //\\            -SteepnessInfluence
        //\\            -OrientationInfluence
        //First test with heightOnly

        float[] _weight;
        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                _weight = new float[terrainData.alphamapLayers];
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;

                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight), Mathf.RoundToInt(x_01 * terrainData.heightmapWidth));                    //Get Steepness of the terrain
                float steepness = terrainData.GetSteepness(x_01, y_01);
                Vector3 direction = terrainData.GetInterpolatedNormal(x, y);

                for(int i = 0; i < terrainData.alphamapLayers; i++)
                {
                    GDTexture textureProp = _Textures[i];

                    switch(textureProp.Mode)
                    {
                        case GDTexture.ApplicationMode.Height:
                            if (height > textureProp.height)
                                _weight[i] = textureProp.influence;
                            break;
                        case GDTexture.ApplicationMode.Slope:
                            if (steepness < textureProp.steepness)
                                _weight[i] = textureProp.influence;
                            break;
                        case GDTexture.ApplicationMode.Orientation:
                            if (direction.z < textureProp.orientation)
                                _weight[i] = textureProp.influence;
                            break;
                        case GDTexture.ApplicationMode.HeightRange:
                            if (height > textureProp.minheight && height < textureProp.maxheight)
                                _weight[i] = textureProp.influence;
                            break;
                        case GDTexture.ApplicationMode.SlopeRange:
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

                for (int i = 0; i < terrainData.alphamapLayers; i ++)
                {
                    _weight[i] /= z;
                    _splatmapData[x, y, i] = _weight[i] ;
                }
            }
        }

        //Appliquer les textures
        terrainData.SetAlphamaps(0, 0, _splatmapData);

        yield return null;
    }



}
