using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to generate a noisemap 
/// Data from this map is used through the whole project
/// 
/// </summary>
public  class NoiseGenerator : MonoBehaviour 
{

    public static NoiseGenerator current;

	private int _mapHeight = 256;
    private int _mapWidth = 256 ;
    private float _noiseScale =70f;
    private int _seed = 3;

    private int _octaves = 10;
    private float _persistence = 0.5f;
    private float _lacunarity = 1;
    private Vector2 _offset;


    private Texture2D _generatedTexture;

    private Color[] _pixelColor; // black/white

    private float[,] _noise;
    private float _worldScale = 100f;

    private float _minHeight = float.MaxValue;
    private float _maxHeight = float.MinValue;

    private float _midWidth;
    private float _midHeight;

    public Image NoiseImage;

    [HideInInspector]
    public Texture2D GeneratedTexture;


    [HideInInspector]

    public Color[] PixelColor { get { return _pixelColor; } set { _pixelColor = value; } }
    public float[,] Noise { get { return _noise; } }
    public int Seed { get { return _seed; } set { _seed = value; } }
    public int Height { get { return _mapHeight; } set { _mapHeight = value; } }
    public int Width { get { return _mapWidth; } set { _mapWidth = value; } }
    public float Scale { get { return _noiseScale; } set { _noiseScale = value; } }
    public int Octaves { get { return _octaves; } set { _octaves = value; } }
    public float Persistence { get { return _persistence; } set { _persistence = value; } }
    public float Lacunarity { get { return _lacunarity; } set { _lacunarity = value; } }
    public float WorldScale { get { return _worldScale; } set { _worldScale = value; } }


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
    public static NoiseGenerator Get()
    {
        return current;
    }

    /// <summary>
    /// Generate a 2D Noise map texture
    /// </summary>
    public void GenerateNoiseBW()
    {
        //Initate the data
        if (_noise != null)
            _noise = null;
        _noise = new float[_mapWidth, _mapHeight];

        if (_pixelColor != null)
            _pixelColor = null;
        _pixelColor = new Color[_mapWidth * _mapHeight];




        //For more vareity in the noise
        System.Random prng = new System.Random(Seed);
        Vector2[] octaveOffests = new Vector2[Octaves];

        //Randomize the octave start position
        for(int i = 0; i < Octaves; i++)
        {
            float offsetX = prng.Next(-100000, 10000);
            float offsetY = prng.Next(-100000, 10000);
  
            octaveOffests[i] = new Vector2(offsetX, offsetY); ;
        }

        if (_noiseScale == 0)
            _noiseScale = 0.0001f;

        //Scale the noise down to center
        _midWidth = _mapWidth / 2;
        _midHeight = _mapHeight / 2;

        for (int y = 0; y < _mapHeight; y++)
        {
            for(int x = 0; x < _mapWidth; x++)
            {
                float amplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0f;

                for (int i = 0; i < _octaves; i++)
                {
                    float xCoord, yCoord;
                    xCoord = (x - _midWidth) / _noiseScale * frequency + octaveOffests[i].x;
                    yCoord = (y - _midHeight) / _noiseScale * frequency + octaveOffests[i].y;

                    float perlinValue = Mathf.PerlinNoise(xCoord + _seed, yCoord + _seed) * 2 - 1;

                    
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= _persistence;
                    frequency *= _lacunarity;
                }


                if (noiseHeight > _maxHeight)
                    _maxHeight = noiseHeight;
                else if (noiseHeight < _minHeight)
                    _minHeight = noiseHeight;


                _noise[x, y] = noiseHeight;
                _noise[x, y] = Mathf.InverseLerp(_minHeight, _maxHeight, _noise[x, y]);

                //Set color for each coordinates in the noisemap Coresponding to the height scale from Noise array         
                _pixelColor[y * _mapWidth + x] = Color.Lerp(Color.black, Color.white, _noise[x, y]);
            }
        }

        //Generates the texture to visualize the noise map
        GenerateTexture();

    }

    public void GenerateTexture()
    {
        if (_generatedTexture != null)
            _generatedTexture = null;
        _generatedTexture = new Texture2D(_mapWidth, _mapHeight);

        _generatedTexture.SetPixels(_pixelColor);
        _generatedTexture.Apply();


        NoiseImage.sprite = null;
        NoiseImage.sprite = Sprite.Create(_generatedTexture, new Rect(0, 0, _mapWidth, _mapHeight), new Vector2(0.5f, 0.5f)); ;

        AppManager.Get.NoiseMapGenerated = true;
    }

    public float[,] GetNoiseData()
    {
        return _noise;
    }

}
