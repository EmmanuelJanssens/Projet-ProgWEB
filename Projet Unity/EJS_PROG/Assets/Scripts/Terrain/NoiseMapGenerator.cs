using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Used to generate a noisemap 
/// Data from this map is used through the whole project
/// 
/// </summary>
public  class NoiseMapGenerator : MonoBehaviour 
{

    public static NoiseMapGenerator current;

	private int _MapHeight = 256;
    private int _MapWidth = 256 ;
    private float _NoiseScale =70;
    private int _Seed = 3;

    private int _octaves = 10;
    private float _persistence = 0.5f;
    private float _lacunarity = 1f;

    private Texture2D _GeneratedTexture;
    private Material _GeneratedMaterial;
    private Color[] _PixelColor; // black/white

    private float[,] _Noise;

    public Renderer _NoiseMapRenderer;

    public int Seed { get { return _Seed; } set { _Seed = value; } }
    public int Height { get { return _MapHeight; } set { _MapHeight = value; } }
    public int Width { get { return _MapWidth; } set { _MapWidth = value; } }
    public float Scale { get { return _NoiseScale; }set { _NoiseScale = value; } }
    public int Octaves { get { return _octaves; } set { _octaves = value; } }
    public float Persistence { get { return _persistence; } set { _persistence = value; } }
    public float Lacunarity { get { return _lacunarity; } set { _lacunarity = value; } }


    private float _minHeight = float.MaxValue;
    private float _maxHeight = float.MinValue;

    private float _midWidth;
    private float _midHeight;

	// Use this for initialization
	void Start () 
	{
        if (current == null)
            current = this;
	}
	
    /// <summary>
    /// Get the instance of the NoiseMapGenerator
    /// There is only one 
    /// </summary>
    /// <returns>Instance of the Generator</returns>
    public static NoiseMapGenerator Get()
    {
        return current;
    }

    /// <summary>
    /// Generate a 2D Noise map texture
    /// </summary>
    public void Generate()
    {
        //Initate the data
        _Noise = new float[_MapWidth, _MapHeight];
        _PixelColor = new Color[_MapWidth * _MapHeight];
        _GeneratedTexture = new Texture2D(_MapWidth, _MapHeight);

        _midWidth = _MapWidth / 2;
        _midHeight = _MapHeight / 2;
        for (int y = 0; y < _MapHeight; y++)
        {
            for(int x = 0; x < _MapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < _octaves; i++)
                {

                    float xCoord, yCoord;
                    xCoord = (x - _midWidth) / _NoiseScale * frequency;
                    yCoord = (y - _midHeight) / _NoiseScale * frequency;

                    float perlinValue = Mathf.PerlinNoise(xCoord + _Seed, yCoord + _Seed) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= _persistence;
                    frequency *= _lacunarity;

                }

                if (noiseHeight > _maxHeight)
                    _maxHeight = noiseHeight;
                else if (noiseHeight < _minHeight)
                    _minHeight = noiseHeight;

                _Noise[x, y] = noiseHeight;

            }
        }
        for (int y = 0; y < _MapHeight; y++)
        {
            for (int x = 0; x < _MapWidth; x++)
            {
                _Noise[x, y] = Mathf.InverseLerp(_minHeight, _maxHeight, _Noise[x, y]);

                //Set color for each coordinates in the noisemap Coresponding to the height scale from Noise array
                _PixelColor[y * _MapWidth + x] = Color.Lerp(Color.black, Color.white, _Noise[x, y]);

            }
        }

        //Generates the texture to visualize the noise map
        _GeneratedTexture.SetPixels(_PixelColor);
        _GeneratedTexture.Apply();
        _NoiseMapRenderer.material.mainTexture = _GeneratedTexture;
    }

    public float[,] GetNoiseData()
    {
        return _Noise;
    }

}
