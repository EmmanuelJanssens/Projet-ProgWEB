using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Imports the details of the Texture that was imported
/// and updates all UI Elements with the values of the texture propreties
/// </summary>
public class ElementPropreties : BasicPropreties
{
    public CObject toModify;
     

    /// Update UI values if texture to modifiy exists
    /// </summary>
    public void OnEnable()
    {

        cmdApply.onClick.RemoveAllListeners();
        cmdCancel.onClick.RemoveAllListeners();

        //Commands of the two buttons from the texture propreties frame
        cmdApply.onClick.AddListener(applyTexturePropreties);
        cmdCancel.onClick.AddListener(closeTexturePropreties);
        if (toModify != null)
        {
            for(int i = 0; i < Tabs.GetTabs.Length; i++)
            {
                switch(toModify.Mode)
                {
                    case CObject.ApplicationMode.Height:
                        Value.text = toModify.height.ToString();
                        Tabs.SetActive("Height");
                        break;
                    case CObject.ApplicationMode.Orientation:
                        Value.text = toModify.orientation.ToString();
                        Tabs.SetActive("Orientation");
                        break;
                    case CObject.ApplicationMode.Slope:
                        Value.text = toModify.steepness.ToString();
                        Tabs.SetActive("Slope");
                        break;
                    case CObject.ApplicationMode.SlopeRange:
                        MinValue.text = toModify.minslope.ToString();
                        MaxValue.text = toModify.maxslope.ToString();
                        Tabs.SetActive("Slope Range");
                        break;
                    case CObject.ApplicationMode.HeightRange:
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
    public virtual void applyTexturePropreties()
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
                toModify.Mode = CObject.ApplicationMode.Height;
                break;
            case "Slope":
                toModify.steepness = SetValue(Value.text);
                toModify.Mode = CObject.ApplicationMode.Slope;
                break;
            case "Orientation":
                toModify.orientation = SetValue(Value.text);
                toModify.Mode = CObject.ApplicationMode.Orientation;
                break;
            case "Height Range":
                toModify.minheight = SetValue(MinValue.text);
                toModify.maxheight = SetValue(MaxValue.text);
                toModify.Mode = CObject.ApplicationMode.HeightRange;
                break;
            case "Slope Range":
                toModify.minslope = SetValue(MinValue.text);
                toModify.maxslope = SetValue(MaxValue.text);
                toModify.Mode = CObject.ApplicationMode.SlopeRange;
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
