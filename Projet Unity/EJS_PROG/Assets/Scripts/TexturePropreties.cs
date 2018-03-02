using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Imports the details of the Texture that was imported
/// and updates all UI Elements with the values of the texture propreties
/// </summary>
public class TexturePropreties : MonoBehaviour
{

    public enum PlacementType { Height,Orientation,Steepness };
    public TextureGenerator textureGenerator;
    public Button cmdApply;
    public Button cmdCancel;

    /// <summary>
    /// Text input fields from the propreties from a CTexture
    /// </summary>
    public InputField Slope;
    public InputField Height;
    public InputField Orientation;
    public InputField Influence;

    public CTexture toModify;

    public void Start()
    {
        cmdApply.onClick.AddListener(applyTexturePropreties);
        cmdCancel.onClick.AddListener(closeTexturePropreties);
    }

    public void OnEnable()
    {
        if(toModify != null)
        {
            Slope.text = toModify.steepness.ToString();
            Height.text = toModify.height.ToString();
            Orientation.text = toModify.orientation.ToString();
            Influence.text = toModify.influence.ToString();
        }
    }

    public void applyTexturePropreties()
    {
        float slope, height, orientation, influence;

        if (float.TryParse(Slope.text, out slope)
                && float.TryParse(Height.text, out height)
                && float.TryParse(Orientation.text, out orientation)
                && float.TryParse(Influence.text, out influence))
        {
            toModify.steepness = slope;
            toModify.height = height;
            toModify.orientation = orientation;
            toModify.influence = influence;
        }

        UIManager.Get.CloseFrame();
    }

    public void closeTexturePropreties()
    {
        UIManager.Get.CloseFrame();
    }
}
