using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TreePropreties : ElementPropreties {

    public override void applyTexturePropreties()
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
}
