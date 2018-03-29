using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantUI : MonoBehaviour
{
    public Button cmdGenerate;
    public VegetationGenerator gen;
    // Use this for initialization
    void Start()
    {
        cmdGenerate.onClick.AddListener(gen.Generate);
    }
}
