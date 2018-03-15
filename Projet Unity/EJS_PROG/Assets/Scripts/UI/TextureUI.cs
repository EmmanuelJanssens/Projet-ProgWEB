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
    [Header("Commands")]
    public Button cmdSelectTexture;
    public Button cmdGenerateTextures;

    [Header("UI asset loader")]
    public UIAssetSelectionLoader Loader;

    [Header("Frame to be displayed")]
    public GameObject frmTextureChooser;
    
    public void Start()
    {
        cmdSelectTexture.onClick.AddListener(delegate {
            UIManager.Get.OpenFrame(frmTextureChooser);
            Loader.LoadAssetSelection();
        });
        cmdGenerateTextures.onClick.AddListener(TextureGenerator.Get.Generate);
    }


}
