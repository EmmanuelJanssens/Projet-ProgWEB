using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Used to link values of the interface to the generator
/// </summary>
public class NoiseUI : MonoBehaviour
{
    /// <summary>
    /// Game object on wich generators are attached
    /// </summary>
    public GameObject GOGenerator;

    //Instance of the NoiseMapGenerator
    private NoiseGenerator NoiseGen;

    /// <summary>
    /// the manager of the application
    /// </summary>
    public AppManager AppManage;

    /// <summary>
    /// Gameobject of the noise map
    /// </summary>
    [Header("Noise map settings")]
    public GameObject GONoiseMap;

    /// <summary>
    /// Input for the width
    /// </summary>
    public InputField Width;

    /// <summary>
    /// Input for the height
    /// </summary>
    public InputField Height;

    /// <summary>
    /// Input for the octaves
    /// </summary>
    public InputField Octaves;

    /// <summary>
    /// Input for the noise scale
    /// </summary>
    public InputField NoiseScaleText;

    /// <summary>
    /// Slider for the noise scale
    /// </summary>
    public Slider NoiseScaleSlider;

    /// <summary>
    /// Input for the random seed
    /// </summary>
    public InputField SeedText;

    /// <summary>
    /// Slider for the random seed
    /// </summary>
    public Slider SeedSlider;

    /// <summary>
    /// Input for the persistence value
    /// </summary>
    public InputField PersistenceText;

    /// <summary>
    /// Slider for the persistence value
    /// </summary>
    public Slider PersistenceSlider;

    /// <summary>
    /// input filed for the lacunartity
    /// </summary>
    public InputField LacunarityText;

    /// <summary>
    /// slider for the lacunarity
    /// </summary>
    public Slider LacunaritySlider;

    /// <summary>
    /// Input for the world scale
    /// </summary>
    public InputField WorldScale;


    /// <summary>
    /// Command that generates the noise map
    /// </summary>
    [Header("Generate")]
    public Button cmdGenerateNoiseMap;


    /// <summary>
    /// Adds the listenenrs to the inputfields and the buttons and the sliders
    /// </summary>
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
        UpdateUI();
        /*********************************************************/

    }

    /// <summary>
    /// Updates the value of the inputs with new updated values
    /// </summary>
    public void UpdateUI()
    {
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
        NoiseGen.Seed = int.Parse(SeedText.text);
        NoiseGen.WorldScale = int.Parse(WorldScale.text);

        if (NoiseGen.Width > 10 && NoiseGen.Height > 10 && NoiseGen.Scale > 0.2f )
        {
            GONoiseMap.SetActive(true);
            AppManage.NoiseMapGenerated = true;
            NoiseGen.Noise = NoiseGen.GenerateNoiseBW();
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


    }
    public void UpdatePersistenceSliderValue(float value)
    {
        PersistenceText.text = value.ToString();
        NoiseGen.Persistence = value;


    }

    public void UpdateLacunaritySliderValue(float value)
    {
        LacunarityText.text = value.ToString();
        NoiseGen.Lacunarity = value;


    }
   
}
