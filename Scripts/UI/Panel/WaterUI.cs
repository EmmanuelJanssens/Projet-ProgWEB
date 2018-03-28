using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterUI : MonoBehaviour
{
    public InputField OceanLevel;
    public Button cmdGenerate;

    public void Start()
    {
        cmdGenerate.onClick.AddListener(
            delegate 
            {
                ApplyOptions();
                AppManager.Get.WaterMap.Generate();    
            });
    }

    public void ApplyOptions()
    {
        AppManager.Get.WaterMap.OceanLevel = float.Parse(OceanLevel.text);
    }


}
