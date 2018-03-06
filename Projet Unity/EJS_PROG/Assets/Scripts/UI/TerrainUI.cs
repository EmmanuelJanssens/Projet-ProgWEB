using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Used to link values of the interface to the generator
/// </summary>
public class TerrainUI : MonoBehaviour
{
    public GameObject GOGenerator;

    //Instance of the NoiseMapGenerator
    private NoiseMapGenerator NoiseGen;
    private TerrainGenerator TerrainGen;
    public EditorNavigation EditorNav;

    public AppManager AppManage;

    [Header("Noise map settings")]
    public GameObject GONoiseMap;

    public InputField Width;
    public InputField Height;

    public InputField Octaves;

    public InputField NoiseScaleValue;
    public Slider NoiseScale;

    public InputField SeedValue;
    public Slider RandomSeed;

    public InputField PersistenceValue;
    public Slider Persistence;

    public InputField LacunaricityValue;
    public Slider Lacunarity;

    [Header("Terrain propreties")]
    public GameObject GOTerrain;

    public InputField HeightScaleValue;
    public Slider HeightScale;

    [Header("Generate")]
    public Button cmdGenerateNoiseMap;
    public Button cmdGenerateTerrain;


	// Use this for initialization
	void Start ()
    {

        //Affiliated gameobject 
        NoiseGen = GOGenerator.GetComponent<NoiseMapGenerator>();
        TerrainGen = GOGenerator.GetComponent<TerrainGenerator>();


        //Event Listeners
        /*********************************************************/
        NoiseScale.onValueChanged.AddListener(UpdateNoiseScaleSliderValue);
        HeightScale.onValueChanged.AddListener(UpdateHeightScaleSliderValue);

        Width.onValueChanged.AddListener(UpdateNoiseWidth);
        Height.onValueChanged.AddListener(UpdateNoiseHeight);
        Octaves.onValueChanged.AddListener(UpdateOctave);

        RandomSeed.onValueChanged.AddListener(UpdateSeed);
        Lacunarity.onValueChanged.AddListener(UpdateLacunaritySliderValue);
        Persistence.onValueChanged.AddListener(UpdatePersistenceSliderValue);

        cmdGenerateNoiseMap.onClick.AddListener(GenerateNoiseMap);
        cmdGenerateTerrain.onClick.AddListener(GenerateTerrain);
        /*********************************************************/


        //Update UI interface Values
        /*********************************************************/
        HeightScaleValue.text = TerrainGen.HeightScale.ToString();

        Width.text = NoiseGen.Width.ToString();
        Height.text = NoiseGen.Height.ToString();
        Octaves.text = NoiseGen.Octaves.ToString();

        NoiseScaleValue.text = NoiseGen.Scale.ToString();
        NoiseScale.value = float.Parse(NoiseScaleValue.text);

        SeedValue.text = NoiseGen.Seed.ToString();
        RandomSeed.value = float.Parse(SeedValue.text);

        LacunaricityValue.text = NoiseGen.Lacunarity.ToString();
        Lacunarity.value = float.Parse(LacunaricityValue.text);

        PersistenceValue.text = NoiseGen.Persistence.ToString();
        Persistence.value = float.Parse(PersistenceValue.text);
        /*********************************************************/

    }

    /// <summary>
    /// Generates a noise map
    /// </summary>
    public void GenerateNoiseMap()
    {
        NoiseGen.Width = int.Parse(Width.text);
        NoiseGen.Height = int.Parse(Height.text);
        NoiseGen.Lacunarity = float.Parse(LacunaricityValue.text);
        NoiseGen.Persistence = float.Parse(PersistenceValue.text);
        NoiseGen.Scale = float.Parse(NoiseScaleValue.text);
        NoiseGen.Octaves = int.Parse(Octaves.text);
        NoiseGen.Seed = int.Parse(Width.text);
        
        if (NoiseGen.Width > 10 && NoiseGen.Height > 10 && NoiseGen.Scale > 0.2f )
        {


            NoiseGen.Generate();
            GONoiseMap.SetActive(true);
            GOTerrain.SetActive(false);
            AppManage.NoiseMapGenerated = true;
        }
    }
    
    /// <summary>
    /// Generates the terrain
    /// Only possible if the noiseMap has been generated
    /// </summary>
    public void GenerateTerrain()
    {
        if(AppManage.NoiseMapGenerated == true)
        {
            if (TerrainGen.HeightScale > 2)
            {
                TerrainGen.GenerateTerrain();
                GONoiseMap.SetActive(false);
                GOTerrain.SetActive(true);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void UpdateOctave(string value)
    {
        int oct;
        bool number = int.TryParse(value, out oct);
        if(number)
        {
            if (oct > 10)
                oct = 10;
            if (oct < 1)
                oct = 1;
            NoiseGen.Octaves = oct;
        }
    }
    /// <summary>
    /// Update the Noise width text value
    /// </summary>
    /// <param name="value"></param>
    public void UpdateNoiseWidth(string value)
    {
        int w;
        bool number = int.TryParse(value, out w);
        if(number)
        {
            NoiseGen.Width = w;
            if (NoiseGen.Width > 10 && NoiseGen.Height > 10 && NoiseGen.Scale > 0.2f)
                NoiseGen.Generate();
        }
    }

    /// <summary>
    /// Updates the noise Height text value
    /// </summary>
    /// <param name="value"></param>
    public void UpdateNoiseHeight(string value)
    {
        int h;
        bool number = int.TryParse(value, out h);
        if (number)
        {
            NoiseGen.Height = h;
            if (NoiseGen.Width > 10 && NoiseGen.Height > 10 && NoiseGen.Scale > 0.2f)
                NoiseGen.Generate();
        }
    }

    /// <summary>
    /// Upates the random seed text value
    /// </summary>
    /// <param name="value"></param>
    public void UpdateSeed(float value)
    {
        if(AppManage.NoiseMapGenerated)
        {
            SeedValue.text = value.ToString();
            NoiseGen.Seed = (int)value;
            NoiseGen.Generate();
        }
    }

    /// <summary>
    /// Update the textfield f the Noise Scale slider
    /// </summary>
    /// <param name="value"></param>
    public void UpdateNoiseScaleSliderValue(float value)
    {
        NoiseScaleValue.text = value.ToString();
        NoiseGen.Scale = value;

        if (NoiseGen.Width > 10 && NoiseGen.Height > 10 && NoiseGen.Scale > 0.2f)
        {
            NoiseGen.Generate();  
        }
    }
    public void UpdatePersistenceSliderValue(float value)
    {
        PersistenceValue.text = value.ToString();
        NoiseGen.Persistence = value;

        if (AppManage.NoiseMapGenerated)
        {
            if (NoiseGen.Persistence > 0)
            {
                NoiseGen.Generate();
            }
        }

    }

    public void UpdateLacunaritySliderValue(float value)
    {
        LacunaricityValue.text = value.ToString();
        NoiseGen.Lacunarity = value;

        if (AppManage.NoiseMapGenerated)
        {
            if (NoiseGen.Lacunarity > 0)
            {
                NoiseGen.Generate();
            }
        }
    }

    /// <summary>
    /// Updates the textfield of the Terrain height Scale value
    /// </summary>
    /// <param name="value"></param>
    public void UpdateHeightScaleSliderValue(float value)
    {
        HeightScaleValue.text = value.ToString();
        TerrainGen.HeightScale = value;
        
        if(AppManage.NoiseMapGenerated)
        {
            if (TerrainGen.HeightScale > 2)
            {
                TerrainGen.GenerateTerrain();
            }
        }
    }

   
}
