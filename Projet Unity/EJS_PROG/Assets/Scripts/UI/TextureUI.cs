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

    public ImportTextures Loader;

    public GameObject frmTextureChooser;
    
    public void Start()
    {
        cmdSelectTexture.onClick.AddListener(delegate {
            Loader.Importer.StartLoading<Texture2D>();
            UIManager.Get.OpenFrame(frmTextureChooser);
        });
        cmdGenerateTextures.onClick.AddListener(TextureGenerator.Get.Generate);
    }


}
