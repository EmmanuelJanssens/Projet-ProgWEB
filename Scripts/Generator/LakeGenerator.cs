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

    public void Generate()
    {
        _scaledLevel = _oceanLevel / AppManager.Get.NoiseMap.WorldScale;
        //Generate Oceans
        for (int y = 0; y < AppManager.Get.NoiseMap.Height; y++)
        {
            for (int x = 0; x < AppManager.Get.NoiseMap.Width; x++)
            {
                if (AppManager.Get.NoiseMap.Noise[x, y] < _scaledLevel)
                {
                    AppManager.Get.NoiseMap.PixelColor[y * AppManager.Get.NoiseMap.Width + x] = new Color(0, 0, 1f);
                }
                if (AppManager.Get.NoiseMap.Noise[x, y] < _scaledLevel - 0.03f)
                {
                    AppManager.Get.NoiseMap.PixelColor[y * AppManager.Get.NoiseMap.Width + x] = new Color(0, 0, 0.8f);
                }
                if (AppManager.Get.NoiseMap.Noise[x, y] < _scaledLevel - .05f)
                {
                    AppManager.Get.NoiseMap.PixelColor[y * AppManager.Get.NoiseMap.Width + x] = new Color(0, 0, 0.5f);
                }
                if (AppManager.Get.NoiseMap.Noise[x, y] < _scaledLevel - .1f)
                {
                    AppManager.Get.NoiseMap.PixelColor[y * AppManager.Get.NoiseMap.Width + x] = new Color(0, 0, 0.3f);
                }

                for (int i = 0; i < _waterDepth; i++)
                {

                    if (AppManager.Get.NoiseMap.Noise[x, y] > (i/5 - .1) &&
                                    AppManager.Get.NoiseMap.Noise[x, y] < (i/5 + .1f))
                    {
                        AppManager.Get.NoiseMap.PixelColor[y * AppManager.Get.NoiseMap.Width + x] = new Color(0, 0, (i ) * .2f);
                    }
                }

            }
        }


        GameObject plane = Instantiate(waterPlane,AppManager.Get.SplatMap.terrain.gameObject.transform);
        plane.transform.localScale = new Vector3(AppManager.Get.SplatMap.terrainData.size.x/10, 1, AppManager.Get.SplatMap.terrainData.size.z/10);
        plane.transform.localPosition = new Vector3(AppManager.Get.SplatMap.terrainData.size.x /2, OceanLevel, AppManager.Get.SplatMap.terrainData.size.z / 2);
        AppManager.Get.NoiseMap.GenerateTexture();
        AppManager.Get.WaterGenerated = true;
    }
}
