using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeGenerator : MonoBehaviour {


    private float _oceanLevel = 30f;
    private float _scaledLevel;
    private int _waterDepth =3;

    public float OceanLevel { get { return _oceanLevel; } set { _oceanLevel = value; } }
    public float ScaledLevel { get { return _scaledLevel; } }
    public GameObject waterPlane;

   /* /// <summary>
    /// Generates thte textures on the noise map, displays average color from the texture
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateTextureOnNoiseMap()
    {
        AppManager.Get.NoiseMap.Noise = AppManager.Get.NoiseMap.GenerateNoiseBW();
        NoiseGenerator ng = AppManager.Get.NoiseMap;
        Color32[] color = AppManager.Get.NoiseMap.PixelColor;

        for (int y = 0; y < AppManager.Get.SplatMap.terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < AppManager.Get.SplatMap.terrainData.alphamapWidth; x++)
            {
                float y_01 = (float)y / (float)AppManager.Get.SplatMap.terrainData.alphamapHeight;
                float x_01 = (float)x / (float)AppManager.Get.SplatMap.terrainData.alphamapWidth;

                int ix = Mathf.RoundToInt(x_01 * AppManager.Get.SplatMap.terrainData.heightmapWidth);
                int iy = Mathf.RoundToInt(y_01 * AppManager.Get.SplatMap.terrainData.heightmapHeight);

                float height = AppManager.Get.SplatMap.terrainData.GetHeight(ix, iy);                    //Get Steepness of the terrain
                float steepness = AppManager.Get.SplatMap.terrainData.GetSteepness(x_01, y_01);
                Vector3 direction = AppManager.Get.SplatMap.terrainData.GetInterpolatedNormal(x, y);

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
        ng.DisplayTexture.SetPixels32(color);
        ng.DisplayTexture.Apply();
        ng.GenerateTexture();
        yield return null;
    }
    */
    public void Generate()
    {
        NoiseGenerator ng = AppManager.Get.NoiseMap;
        _scaledLevel = _oceanLevel /ng.WorldScale;
        //Generate Oceans
        for (int y = 0; y < ng.Height; y++)
        {
            for (int x = 0; x < ng.Width; x++)
            {
                if (ng.Noise[x, y] < _scaledLevel)
                {
                    ng.PixelColor[y * ng.Width + x] = new Color(0, 0, 1f);
                }
                if (ng.Noise[x, y] < _scaledLevel - 0.03f)
                {
                    ng.PixelColor[y * ng.Width + x] = new Color(0, 0, 0.8f);
                }
                if (ng.Noise[x, y] < _scaledLevel - .05f)
                {
                    ng.PixelColor[y * ng.Width + x] = new Color(0, 0, 0.5f);
                }
                if (ng.Noise[x, y] < _scaledLevel - .1f)
                {
                    ng.PixelColor[y * ng.Width + x] = new Color(0, 0, 0.3f);
                }

       
                ng.DisplayTexture.SetPixel(x, y, ng.PixelColor[y * ng.Width + x]);
            }
        }

        ng.DisplayTexture.Apply();
        GameObject plane = Instantiate(waterPlane,AppManager.Get.SplatMap.terrain.gameObject.transform);
        plane.transform.localScale = new Vector3(AppManager.Get.SplatMap.terrainData.size.x/10, 1, AppManager.Get.SplatMap.terrainData.size.z/10);
        plane.transform.localPosition = new Vector3(AppManager.Get.SplatMap.terrainData.size.x /2, OceanLevel, AppManager.Get.SplatMap.terrainData.size.z / 2);
        ng.GenerateTexture();
        AppManager.Get.WaterGenerated = true;
    }
}
