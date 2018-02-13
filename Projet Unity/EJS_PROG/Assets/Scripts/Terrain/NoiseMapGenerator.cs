using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Used to generate terrain
/// </summary>
public  class NoiseMapGenerator : MonoBehaviour 
{

    public static NoiseMapGenerator current;

    [SerializeField]
	private int _MapHeight = 256;
    [SerializeField]
    private int _MapWidth = 256 ;
    [SerializeField]
    private float _NoiseScale = .4f;

    private Texture2D _GeneratedTexture;
    private Material _GeneratedMaterial;
    private Color[] _PixelColor; // black/white

    private float[,] _Noise;

    public Renderer _NoiseMapRenderer;


    public int Height { get { return _MapHeight; } }
    public int Width { get { return _MapWidth;  } }
    public float Scale { get { return _NoiseScale; } }

	// Use this for initialization
	void Start () 
	{
        if (current == null)
            current = this;

        _Noise = new float[_MapWidth, _MapHeight];
        _PixelColor = new Color[_MapWidth * _MapHeight];
        _GeneratedTexture = new Texture2D(_MapWidth, _MapHeight);
	}
	
    public static NoiseMapGenerator Get()
    {
        return current;
    }

    public void Generate()
    {
        for(int y = 0; y < _MapHeight; y++)
        {
            for(int x = 0; x < _MapWidth; x++)
            {
                float xCoord, yCoord;
                xCoord = x /  _NoiseScale;
                yCoord = y / _NoiseScale;

                float perlinValue = Mathf.PerlinNoise(xCoord, yCoord);

                _Noise[x, y] = perlinValue;
                _PixelColor[y * _MapWidth + x] = Color.Lerp(Color.black, Color.white, _Noise[x, y]);
            }
        }

        _GeneratedTexture.SetPixels(_PixelColor);
        _GeneratedTexture.Apply();
        _NoiseMapRenderer.material.mainTexture = _GeneratedTexture;
    }

    public float[,] GetNoiseData()
    {
        return _Noise;
    }

}
