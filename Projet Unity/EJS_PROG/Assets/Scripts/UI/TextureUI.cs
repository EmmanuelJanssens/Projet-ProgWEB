using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for GUI Interaction
/// And import save values from selected texture
/// 
/// link between textureGenerator
/// </summary>
public class TextureUI : MonoBehaviour
{

    public Button cmdSelectTexture;
    public Button cmdGenerateTextures;

    public LoadAvailableTextures Loader;

    public GameObject frmTextureChooser;
    
    public void Start()
    {
        cmdSelectTexture.onClick.AddListener(delegate {
            Loader.StartLoading();
            UIManager.Get.OpenFrame(frmTextureChooser);

        });
        cmdGenerateTextures.onClick.AddListener(TextureGenerator.Get.Generate);
    }


}
