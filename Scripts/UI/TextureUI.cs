using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureUI : MonoBehaviour {

    [Header("Commands")]
    public Button cmdGenerateTextures;

    public void Start()
    { 
        cmdGenerateTextures.onClick.AddListener(TextureGenerator.Get.Generate);
    }

}
