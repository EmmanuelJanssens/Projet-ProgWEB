using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TexturePropreties : MonoBehaviour
{
    public TextureGenerator textureGenerator;
    public Button cmdApply;
    public Button cmdCancel;


    public void Start()
    {
        cmdApply.onClick.AddListener(applyTexturePropreties);
        cmdCancel.onClick.AddListener(closeTexturePropreties);
    }

    public void applyTexturePropreties()
    {

    }

    public void closeTexturePropreties()
    {
        UIManager.Get.CloseFrame();
    }
}
