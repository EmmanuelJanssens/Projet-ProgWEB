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

    /// <summary>
    /// Used to import the assets that where seleced
    /// </summary>
    public ChooseAssets Import;

    /// <summary>
    /// List of all the textures that where loaded
    /// </summary>
    List<GDTexture> _textures;

    /// <summary>
    /// Splat prototypes to be used by the terrain
    /// </summary>
    List<SplatPrototype> SplatPrototypes;

    /// <summary>
    /// Data for the terrain data
    /// </summary>
    private float[,,] _splatmapData;


    public static TextureGenerator Get { get { return current; } }

    /// <summary>
    /// Terrain object on wich the terrain component is attached
    /// </summary>
    public GameObject TerrainToApplyTexture;

    /// <summary>
    /// Terrain that is used
    /// </summary>
    [HideInInspector]
    public Terrain terrain;


    /// <summary>
    /// Terrain data from the terrain
    /// </summary>
    [HideInInspector]
    public TerrainData terrainData;

    /// <summary>
    /// Terrain collider
    /// </summary>
    [HideInInspector]
    public TerrainCollider terrainCollider;


    public List<GDTexture> Textures { get { return _textures; } }
    public void Start()
    {
        if (current == null)
            current = this;

        terrain = TerrainToApplyTexture.GetComponent<Terrain>();
        terrainCollider = TerrainToApplyTexture.GetComponent<TerrainCollider>();
        terrainData = new TerrainData();
    }


    /// <summary>
    /// Function called when pressing the generate button
    /// </summary>
    public void Generate()
    {
        StartCoroutine(GenerateTextureOnNoiseMap());
    }

    /// <summary>
    /// Generates thte textures on the noise map, displays average color from the texture
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateTextureOnNoiseMap()
    {
        AppManager.Get.NoiseMap.Noise = AppManager.Get.NoiseMap.GenerateNoiseBW();
        NoiseGenerator ng = AppManager.Get.NoiseMap;
        Color32[] color = AppManager.Get.NoiseMap.PixelColor;
        //First generate a blank terrain
        yield return StartCoroutine(IGenerate());

        //Import texture into terrain painter
        _textures = new List<GDTexture>();

        for (int i = 0; i < Import.ChoosenTextures.Count; i++)
        {
            _textures.Add(Import.ChoosenTextures[i] as GDTexture);
        }


        for (int y = terrainData.alphamapHeight; y > 0 ; y--)
        {
            for (int x = terrainData.alphamapWidth; x > 0; x--)
            {
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;

                int ix = Mathf.RoundToInt(x_01 * terrainData.heightmapWidth);
                int iy = Mathf.RoundToInt(y_01 * terrainData.heightmapHeight);

                float height = terrainData.GetHeight(ix, iy);                    //Get Steepness of the terrain
                float steepness = terrainData.GetSteepness(x_01, y_01);
                Vector3 direction = terrainData.GetInterpolatedNormal(x, y);

                for (int i = 0; i < _textures.Count; i++)
                {
                    GDTexture textureProp = _textures[i];

                    switch (textureProp.Mode)
                    {
                        case GDTexture.ApplicationMode.Height:
                            if (height > textureProp.height)
                            {
                                color[iy * ng.Width + ix] = _textures[i].avgColor;
                            }
                            break;
                        case GDTexture.ApplicationMode.Slope:
                            //Slope 0 to 90 degrees
                            if (steepness < textureProp.steepness)
                            {
                                color[iy * ng.Width + ix] = _textures[i].avgColor;
                            }
                            break;
                        case GDTexture.ApplicationMode.Orientation:
                            if (direction.z < textureProp.orientation)
                            {
                                color[iy * ng.Width + ix] = _textures[i].avgColor;
                            }
                            break;
                        case GDTexture.ApplicationMode.HeightRange:
                            if (height > textureProp.minheight && height < textureProp.maxheight)
                            {
                                color[iy * ng.Width + ix] = _textures[i].avgColor;
                            }
                            break;
                        case GDTexture.ApplicationMode.SlopeRange:
                            if (steepness > textureProp.minslope && steepness < textureProp.maxslope)
                            {
                                color[iy * ng.Width + ix] = _textures[i].avgColor;
                            }
                            break;
                        default:
                            break;
                    }
                    //Set color for each coordinates in the noisemap Coresponding to the height scale from Noise array         

                }
            }
        }
        ng.DisplayTexture.SetPixels32 (color);
        ng.DisplayTexture.Apply();
        ng.GenerateTexture();
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
        terrainData.baseMapResolution = AppManager.Get.NoiseMap.Width;
        terrainData.alphamapResolution = AppManager.Get.NoiseMap.Width; 

        terrain.terrainData = terrainData;
        terrainCollider.terrainData = terrainData;


        SplatPrototypes = new List<SplatPrototype>();

        //Import texture into terrain painter
        _textures = new List<GDTexture>();

        for (int i = 0; i < Import.ChoosenTextures.Count; i++)
        {
            SplatPrototypes.Add(new SplatPrototype());
            GDTexture texture = Import.ChoosenTextures[i] as GDTexture;

            SplatPrototypes[i].texture = texture.texture;
            SplatPrototypes[i].tileSize = new Vector2(1, 1);

            _textures.Add(texture);
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
                    GDTexture textureProp = _textures[i];

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
