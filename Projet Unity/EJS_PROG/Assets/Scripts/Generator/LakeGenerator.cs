using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeGenerator : MonoBehaviour {

    public NoiseGenerator NoiseMap;

    private float _oceanLevel = 30f;
    private float _scaledLevel;
    private int _waterDepth =3;

    public float OceanLevel { get { return _oceanLevel; } set { _oceanLevel = value; } }

	public void Generate()
    {
        _scaledLevel = _oceanLevel / NoiseMap.WorldScale;
        //Generate Oceans
        for (int y = 0; y < NoiseMap.Height; y++)
        {
            for (int x = 0; x < NoiseMap.Width; x++)
            {
                if (NoiseMap.Noise[x, y] < _scaledLevel)
                {
                    NoiseMap.PixelColor[y * NoiseMap.Width + x] = new Color(0, 0, 1f);
                }
                if (NoiseMap.Noise[x, y] < _scaledLevel - 0.03f)
                {
                    NoiseMap.PixelColor[y * NoiseMap.Width + x] = new Color(0, 0, 0.8f);
                }
                if (NoiseMap.Noise[x, y] < _scaledLevel - .05f)
                {
                    NoiseMap.PixelColor[y * NoiseMap.Width + x] = new Color(0, 0, 0.5f);
                }
                if (NoiseMap.Noise[x, y] < _scaledLevel - .1f)
                {
                        NoiseMap.PixelColor[y * NoiseMap.Width + x] = new Color(0, 0, 0.3f);
                }

                for (int i = 0; i < _waterDepth; i++)
                {

                    if (NoiseMap.Noise[x, y] > (i/5 - .1) &&
                                   NoiseMap.Noise[x, y] < (i/5 + .1f))
                    {
                        NoiseMap.PixelColor[y * NoiseMap.Width + x] = new Color(0, 0, (i ) * .2f);
                    }
                }

            }
        }

        NoiseMap.GenerateTexture();

    }
}
