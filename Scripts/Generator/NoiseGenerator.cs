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

    /// <summary>
    /// noise map height
    /// </summary>
	private int _mapHeight = 256;
    /// <summary>
    /// Noise map width
    /// </summary>
    private int _mapWidth = 256 ;

    /// <summary>
    /// noise map scale
    /// </summary>
    private float _noiseScale =70f;

    /// <summary>
    /// Random seed of the map
    /// </summary>
    private int _seed = 3;


    /// <summary>
    /// numnbers of octaves
    /// </summary>
    private int _octaves = 10;

    /// <summary>
    /// Persistence of the noise map
    /// </summary>
    private float _persistence = 0.5f;

    /// <summary>
    /// lacunarity of the noise map
    /// </summary>
    private float _lacunarity = 1;

    /// <summary>
    /// Renderer of the 3D plane on wich the noisemap will be rendered
    /// </summary>
    public  Renderer NoiseRenderer;

    /// <summary>
    /// Texture of the noisemap as it will be displayed
    /// </summary>
    private Texture2D _displayTexture;

    /// <summary>
    /// Texture that contains the 16 bit format from the noisemap
    /// </summary>
    private Texture2D _generatedTexture;

    /// <summary>
    /// Color of each pixel of the map
    /// </summary>
    private Color32[] _pixelColor; // black/white

    /// <summary>
    /// Scale of the world
    /// </summary>
    private float _worldScale = 100f;


    private float _minHeight = float.MaxValue;
    private float _maxHeight = float.MinValue;

    private float _midWidth;
    private float _midHeight;



    #region Accessors and mutators
    [HideInInspector]
    public Color32[] PixelColor { get { return _pixelColor; } set { _pixelColor = value; } }
    public float[,] Noise;
    public int Seed { get { return _seed; } set { _seed = value; } }
    public int Height { get { return _mapHeight; } set { _mapHeight = value; } }
    public int Width { get { return _mapWidth; } set { _mapWidth = value; } }
    public float Scale { get { return _noiseScale; } set { _noiseScale = value; } }
    public int Octaves { get { return _octaves; } set { _octaves = value; } }
    public float Persistence { get { return _persistence; } set { _persistence = value; } }
    public float Lacunarity { get { return _lacunarity; } set { _lacunarity = value; } }
    public float WorldScale { get { return _worldScale; } set { _worldScale = value; } }
    public Texture2D DisplayTexture { get { return _displayTexture; } set { _displayTexture = value; } }
    #endregion

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
    public float[,] GenerateNoiseBW()
    {

        float[,] noise = new float[_mapWidth, _mapHeight];

        if (_pixelColor != null)
            _pixelColor = null;

        _pixelColor = new Color32[_mapWidth * _mapHeight];




        //For more vareity in the noise
        System.Random prng = new System.Random(Seed);
        Vector2[] octaveOffests = new Vector2[Octaves];

        _displayTexture = new Texture2D(_mapWidth, _mapHeight);

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


                noise[x, y] = noiseHeight;
                noise[x, y] = Mathf.InverseLerp(_minHeight, _maxHeight, noise[x, y]);

                //Set color for each coordinates in the noisemap Coresponding to the height scale from Noise array         
                _pixelColor[y * _mapWidth + x] = Color.LerpUnclamped(Color.black, Color.white, noise[x, y]);

                _displayTexture.SetPixel(x,y,_pixelColor[y * _mapWidth + x]);
            }
        }
        _displayTexture.Apply();
        //Generates the texture to visualize the noise map
        GenerateTexture();

        return noise;
    }

    public void GenerateTexture()
    {

        NoiseRenderer.material.mainTexture = _displayTexture;


        _generatedTexture = new Texture2D(_mapWidth, _mapHeight,TextureFormat.PVRTC_RGBA4,true);
        _generatedTexture.LoadRawTextureData(_displayTexture.GetRawTextureData());
        _generatedTexture.GetRawTextureData();
        _generatedTexture.Apply();



        AppManager.Get.NoiseMapGenerated = true;
    }



}
