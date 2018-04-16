using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Imports the details of the Texture that was imported
/// and updates all UI Elements with the values of the texture propreties
/// </summary>
public class ElementPropreties : MonoBehaviour
{

    public Button cmdApply;
    public Button cmdCancel;

    public SwitchTab Tabs;

    //For single Values
    protected InputField Value;

    //For ranged Values
    protected InputField MinValue;
    protected InputField MaxValue;

    public InputField Density;
    public InputField Clump;
    public InputField Resolution;

    public Text Title;
    public Image Image;
    /// Update UI values if texture to modifiy exists
    /// </summary>
    public void OnEnable()
    {
        switch (UIManager.Get.CurrentPanel.name)
        {
            case "terrain":
                Density.transform.parent.gameObject.SetActive(false);
                Clump.transform.parent.gameObject.SetActive(false);
                Resolution.transform.parent.gameObject.SetActive(false);
                break;

            default:
                Density.transform.parent.gameObject.SetActive(true);
                Clump.transform.parent.gameObject.SetActive(true);
                Resolution.transform.parent.gameObject.SetActive(true);
                break;
        }

        cmdApply.onClick.RemoveAllListeners();
        cmdCancel.onClick.RemoveAllListeners();

        //Commands of the two buttons from the texture propreties frame
        cmdApply.onClick.AddListener(applyTexturePropreties);
        cmdCancel.onClick.AddListener(closeTexturePropreties);

        if (AppManager.Get.ObjectToModify != null)
        {
            Title.text = AppManager.Get.ObjectToModify.ObjectName;
            Image.sprite = AppManager.Get.ObjectToModify.ui_sprite;

            for (int i = 0; i < Tabs.GetTabs.Length; i++)
            {
                switch (AppManager.Get.ObjectToModify.Mode)
                {
                    case GDObject.ApplicationMode.Height:
                        Value.text = AppManager.Get.ObjectToModify.height.ToString();
                        Tabs.SetActive("Height");
                        break;
                    case GDObject.ApplicationMode.Orientation:
                        Value.text = AppManager.Get.ObjectToModify.orientation.ToString();
                        Tabs.SetActive("Orientation");
                        break;
                    case GDObject.ApplicationMode.Slope:
                        Value.text = AppManager.Get.ObjectToModify.steepness.ToString();
                        Tabs.SetActive("Slope");
                        break;
                    case GDObject.ApplicationMode.SlopeRange:
                        MinValue.text = AppManager.Get.ObjectToModify.minslope.ToString();
                        MaxValue.text = AppManager.Get.ObjectToModify.maxslope.ToString();
                        Tabs.SetActive("Slope Range");
                        break;
                    case GDObject.ApplicationMode.HeightRange:
                        MinValue.text = AppManager.Get.ObjectToModify.minheight.ToString();
                        MaxValue.text = AppManager.Get.ObjectToModify.maxheight.ToString();
                        Tabs.SetActive("Height Range");
                        break;
                    default:
                        break;
                }
            }
            Density.text = AppManager.Get.ObjectToModify.density.ToString();
            Clump.text = AppManager.Get.ObjectToModify.clump.ToString();
            Resolution.text = AppManager.Get.ObjectToModify.details_resolution.ToString();


        }
    }


    /// <summary>
    /// Applies a texture according to the Active Tab
    /// </summary>
    public virtual void applyTexturePropreties()
    {
        if (Tabs.gameObject.GetComponentsInChildren<InputField>().Length > 1)
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
                AppManager.Get.ObjectToModify.height = SetValue(Value.text);
                AppManager.Get.ObjectToModify.Mode = GDObject.ApplicationMode.Height;
                break;
            case "Slope":
                AppManager.Get.ObjectToModify.steepness = SetValue(Value.text);
                AppManager.Get.ObjectToModify.Mode = GDObject.ApplicationMode.Slope;
                break;
            case "Orientation":
                AppManager.Get.ObjectToModify.orientation = SetValue(Value.text);
                AppManager.Get.ObjectToModify.Mode = GDObject.ApplicationMode.Orientation;
                break;
            case "Height Range":
                AppManager.Get.ObjectToModify.minheight = SetValue(MinValue.text);
                AppManager.Get.ObjectToModify.maxheight = SetValue(MaxValue.text);
                AppManager.Get.ObjectToModify.Mode = GDObject.ApplicationMode.HeightRange;
                break;
            case "Slope Range":
                AppManager.Get.ObjectToModify.minslope = SetValue(MinValue.text);
                AppManager.Get.ObjectToModify.maxslope = SetValue(MaxValue.text);
                AppManager.Get.ObjectToModify.Mode = GDObject.ApplicationMode.SlopeRange;
                break;
            default:
                break;
        }
        AppManager.Get.ObjectToModify.clump = SetValue(Clump.text);
        AppManager.Get.ObjectToModify.density = (int)SetValue(Density.text);
        AppManager.Get.ObjectToModify.details_resolution = (int)SetValue(Resolution.text);

        UIManager.Get.CloseFrame();
    }

    /// <summary>
    /// Returns a converted string from an inputfield
    /// </summary>
    /// <param name="v">string value of the input fiedld<param>
    /// <returns>Float value of the input field</returns>
    public float SetValue(string v)
    {
        float value;

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
