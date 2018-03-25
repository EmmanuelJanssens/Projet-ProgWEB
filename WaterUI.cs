using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaterUI : MonoBehaviour
{
    public InputField OceanLevel;

    public LakeGenerator LakeGen;

    public Button cmdGenerate;

    public void Start()
    {
        cmdGenerate.onClick.AddListener(
            delegate 
            {
                ApplyOptions();
                LakeGen.Generate();    
            }    
            );
    }

    public void ApplyOptions()
    {
        LakeGen.OceanLevel = float.Parse(OceanLevel.text);
    }


}
