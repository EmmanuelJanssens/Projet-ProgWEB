using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for GUI Interaction
/// And import save values from selected texture
/// 
/// link between textureGenerator
/// </summary>
public class TextureUI : MonoBehaviour {

    public Button cmdSelectTexture;
    public Button cmdGenerateTextures;
    public Button cmdAddTextures;

    public GameObject TextureChooser;

    public Slider SteepnesSlider;
    public InputField Steepness;
    public Slider HeightSlider;
    public InputField Height;
    public Slider OrientationSlider;
    public InputField Orientation;
    
    public void Start()
    {
        cmdSelectTexture.onClick.AddListener(delegate { UIManager.Get.OpenFrame(TextureChooser); });
        cmdAddTextures.onClick.AddListener(ChooseTextures);
    }

    public void ChooseTextures()
    {
        UIManager.Get.CloseFrame();
    }
}
