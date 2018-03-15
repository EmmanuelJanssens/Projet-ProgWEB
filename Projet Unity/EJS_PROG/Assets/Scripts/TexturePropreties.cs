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

    public SwitchTab Tabs;
    /// <summary>
    /// Text input fields from the propreties from a CTexture
    /// </summary>

    //General influence
    public InputField Influence;


    //For single Values
    private InputField Value;

    //For ranged Values
    private InputField MinValue;
    private InputField MaxValue;

    public CTexture toModify;

     
    public void Start()
    {
        //Commands of the two buttons from the texture propreties frame
        cmdApply.onClick.AddListener(applyTexturePropreties);
        cmdCancel.onClick.AddListener(closeTexturePropreties);
    }
    /// Update UI values if texture to modifiy exists
    /// </summary>
    public void OnEnable()
    {
        if (toModify != null)
        {
            for(int i = 0; i < Tabs.GetTabs.Length; i++)
            {
                switch(toModify.Mode)
                {
                    case CTexture.ApplicationMode.Height:
                        Value.text = toModify.height.ToString();
                        Tabs.SetActive("Height");
                        break;
                    case CTexture.ApplicationMode.Orientation:
                        Value.text = toModify.orientation.ToString();
                        Tabs.SetActive("Orientation");
                        break;
                    case CTexture.ApplicationMode.Slope:
                        Value.text = toModify.steepness.ToString();
                        Tabs.SetActive("Slope");
                        break;
                    case CTexture.ApplicationMode.SlopeRange:
                        MinValue.text = toModify.minslope.ToString();
                        MaxValue.text = toModify.maxslope.ToString();
                        Tabs.SetActive("Slope Range");
                        break;
                    case CTexture.ApplicationMode.HeightRange:
                        MinValue.text = toModify.minheight.ToString();
                        MaxValue.text = toModify.maxheight.ToString();
                        Tabs.SetActive("Height Range");                  
                        break;
                    default:
                        break;
                }
            }
        }
    }


    /// <summary>
    /// Applies a texture according to the Active Tab
    /// </summary>
    public void applyTexturePropreties()
    {
        if(Tabs.gameObject.GetComponentsInChildren<InputField>().Length > 1)
        {
            MinValue = Tabs.gameObject.GetComponentsInChildren<InputField>()[0];
            MaxValue = Tabs.gameObject.GetComponentsInChildren<InputField>()[1];
        }
        else
        {
            Value = Tabs.gameObject.GetComponentInChildren<InputField>();
        }

        switch (Tabs.ActiveTab.Name)
        {
            case "Height":
                toModify.height = SetValue(Value.text);
                toModify.Mode = CTexture.ApplicationMode.Height;
                break;
            case "Slope":
                toModify.steepness = SetValue(Value.text);
                toModify.Mode = CTexture.ApplicationMode.Slope;
                break;
            case "Orientation":
                toModify.orientation = SetValue(Value.text);
                toModify.Mode = CTexture.ApplicationMode.Orientation;
                break;
            case "Height Range":
                toModify.minheight = SetValue(MinValue.text);
                toModify.maxheight = SetValue(MaxValue.text);
                toModify.Mode = CTexture.ApplicationMode.HeightRange;
                break;
            case "Slope Range":
                toModify.minslope = SetValue(MinValue.text);
                toModify.maxslope = SetValue(MaxValue.text);
                toModify.Mode = CTexture.ApplicationMode.SlopeRange;
                break;
            default:
                break;
        }
        toModify.influence = SetValue(Influence.text);
        UIManager.Get.CloseFrame();
    }

    /// <summary>
    /// Returns a converted string from an inputfield
    /// </summary>
    /// <param name="v">string value of the input fiedld<param>
    /// <returns>Float value of the input field</returns>
    public float SetValue(string v)
    {
        float value ;

        bool result = float.TryParse(v, out value);
        if (result)
            return value;
        else
            return 0f;
    }

    /// <summary>
    /// Closes the propreties frame
    /// </summary>
    public void closeTexturePropreties()
    {
        UIManager.Get.CloseFrame();
    }
}
