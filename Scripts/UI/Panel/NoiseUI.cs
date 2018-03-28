using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Used to link values of the interface to the generator
/// </summary>
public class NoiseUI : MonoBehaviour
{
    public GameObject GOGenerator;

    //Instance of the NoiseMapGenerator
    private NoiseGenerator NoiseGen;

    public EditorNavigation EditorNav;

    public AppManager AppManage;

    [Header("Noise map settings")]
    public GameObject GONoiseMap;

    public InputField Width;
    public InputField Height;

    public InputField Octaves;

    public InputField NoiseScaleText;
    public Slider NoiseScaleSlider;

    public InputField SeedText;
    public Slider SeedSlider;

    public InputField PersistenceText;
    public Slider PersistenceSlider;

    public InputField LacunarityText;
    public Slider LacunaritySlider;

    public InputField WorldScale;



    [Header("Generate")]
    public Button cmdGenerateNoiseMap;

	// Use this for initialization
	void Start ()
    {

        //Affiliated gameobject 
        NoiseGen = GOGenerator.GetComponent<NoiseGenerator>();

        //Event Listeners
        /*********************************************************/
        NoiseScaleSlider.onValueChanged.AddListener(UpdateNoiseScaleSliderValue);


        Width.onValueChanged.AddListener(UpdateNoiseWidth);
        Height.onValueChanged.AddListener(UpdateNoiseHeight);
        Octaves.onValueChanged.AddListener(UpdateOctave);

        SeedSlider.onValueChanged.AddListener(UpdateSeed);
        LacunaritySlider.onValueChanged.AddListener(UpdateLacunaritySliderValue);
        PersistenceSlider.onValueChanged.AddListener(UpdatePersistenceSliderValue);

        cmdGenerateNoiseMap.onClick.AddListener(GenerateNoiseMap);
        /*********************************************************/


        //Update UI interface Values
        /*********************************************************/

        Width.text = NoiseGen.Width.ToString();
        Height.text = NoiseGen.Height.ToString();
        Octaves.text = NoiseGen.Octaves.ToString();

        NoiseScaleText.text = NoiseGen.Scale.ToString();
        NoiseScaleSlider.value = float.Parse(NoiseScaleText.text);

        SeedText.text = NoiseGen.Seed.ToString();
        SeedSlider.value = float.Parse(SeedText.text);

        LacunarityText.text = NoiseGen.Lacunarity.ToString();
        LacunaritySlider.value = float.Parse(LacunarityText.text);

        PersistenceText.text = NoiseGen.Persistence.ToString();
        PersistenceSlider.value = float.Parse(PersistenceText.text);

        WorldScale.text = NoiseGen.WorldScale.ToString();
        /*********************************************************/

    }

    /// <summary>
    /// Generates a noise map
    /// </summary>
    public void GenerateNoiseMap()
    {
        NoiseGen.Width = int.Parse(Width.text);
        NoiseGen.Height = int.Parse(Height.text);
        NoiseGen.Lacunarity = float.Parse(LacunarityText.text);
        NoiseGen.Persistence = float.Parse(PersistenceText.text);
        NoiseGen.Scale = float.Parse(NoiseScaleText.text);
        NoiseGen.Octaves = int.Parse(Octaves.text);
        NoiseGen.Seed = int.Parse(Width.text);
        NoiseGen.WorldScale = int.Parse(WorldScale.text);

        if (NoiseGen.Width > 10 && NoiseGen.Height > 10 && NoiseGen.Scale > 0.2f )
        {
            NoiseGen.GenerateNoiseBW();
            GONoiseMap.SetActive(true);
            AppManage.NoiseMapGenerated = true;
        }
    }
    
    /// <summary>
    /// Generates the terrain
    /// Only possible if the noiseMap has been generated
    /// </summary>
    /*public void GenerateTerrain()
    {
        if(AppManage.NoiseMapGenerated == true)
        {
            if (TerrainGen.HeightScale > 2)
            {
                TerrainGen.GenerateTerrain();
                GONoiseMap.SetActive(false);
            }
        }
    }*/
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
                NoiseGen.GenerateNoiseBW();
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
                NoiseGen.GenerateNoiseBW();
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
            SeedText.text = value.ToString();
            NoiseGen.Seed = (int)value;
            NoiseGen.GenerateNoiseBW();
        }
    }

    /// <summary>
    /// Update the textfield f the Noise Scale slider
    /// </summary>
    /// <param name="value"></param>
    public void UpdateNoiseScaleSliderValue(float value)
    {
        NoiseScaleText.text = value.ToString();
        NoiseGen.Scale = value;

        if (NoiseGen.Width > 10 && NoiseGen.Height > 10 && NoiseGen.Scale > 0.2f)
        {
            NoiseGen.GenerateNoiseBW();  
        }
    }
    public void UpdatePersistenceSliderValue(float value)
    {
        PersistenceText.text = value.ToString();
        NoiseGen.Persistence = value;

        if (AppManage.NoiseMapGenerated)
        {
            if (NoiseGen.Persistence > 0)
            {
                NoiseGen.GenerateNoiseBW();
            }
        }

    }

    public void UpdateLacunaritySliderValue(float value)
    {
        LacunarityText.text = value.ToString();
        NoiseGen.Lacunarity = value;

        if (AppManage.NoiseMapGenerated)
        {
            if (NoiseGen.Lacunarity > 0)
            {
                NoiseGen.GenerateNoiseBW();
            }
        }
    }
   
}
