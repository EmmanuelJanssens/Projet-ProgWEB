using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Imports texture into the list of 
/// textures to be generated 
/// Added onto _textureImporter gameobject
/// </summary>
public class UsableTextures : MonoBehaviour
{
    public static UsableTextures current;

    /// <summary>
    /// Needed to access all loaded texture from the assetbundle
    /// </summary>
    public ImportAssets UIImporter;

    [Header("Content of the imported usable textures")]
    /// <summary>
    /// Container of the graphical elements
    /// </summary>
    public Transform Container;
    /// <summary>
    /// Template to an texture element
    /// A CTexture script is attached to IT
    /// contains a Text,Image,RemoveButton and propreties Button
    /// </summary>
    public GameObject UITextureElement;

    /// <summary>
    /// List of textures that are available into the generator
    /// </summary>
    [HideInInspector]
    public List<CTexture> ImportedTextures;

    /// <summary>
    /// List of the textures
    /// </summary>
    public ListManager TextureList;

    [Header("Texture frame propreties")]
    public Text txtTitle;
    public Image sprTexture;

    /// <summary>
    /// Interaction with the texture elements
    /// </summary>
    [HideInInspector]
    public Button[] cmdTextureProp;

    /// <summary>
    /// Frame template that shows the propreties from the texture
    /// </summary>
    public GameObject _frmTextureProp;

    [Header("Button that starts texture importation")]
    /// <summary>
    /// Command that imports the textures from the bundle
    /// </summary>
    public Button cmdImport;

    /// <summary>
    /// Counts the total of textures to affect an unique identifier to them
    /// </summary>
    public static int Identifier = 0;

    public static UsableTextures Get { get { return current; } }

    public bool TexturesAreImported { get; set; }
    // Use this for initialization
    void Start ()
    {
        if (current == null)
            current = this;

        cmdImport.onClick.AddListener(Import);  
    }
	
    /// <summary>
    /// Opens the texture propreties frame 
    /// </summary>
    /// <param name="id">Identification number of the texture to be loaded</param>
    public void OpenTextureProp(int id)
    {
        ElementPropreties proprs = _frmTextureProp.GetComponent<ElementPropreties>();
        proprs.toModify = ImportedTextures[id];

        txtTitle.text = ImportedTextures[id].ObjectName;
        sprTexture.sprite = ImportedTextures[id].ui_sprite;

        UIManager.Get.OpenFrame(_frmTextureProp);
    }

    /// <summary>
    /// Removes an element from the list and updates the identification Numbers of all the texture
    /// </summary>
    /// <param name="id">Identification of the texture that has to be removed</param>
    public void RemoveTexture(int id)
    {
        Destroy(ImportedTextures[id].ui_object);
        ImportedTextures.RemoveAt(id);
        Identifier = 0;
        for (int i = 0; i < ImportedTextures.Count; i++)
        {

            ImportedTextures[i].ID = Identifier;
            Identifier++;
        }
    }

    public void UpdateTextureList()
    {
        for (int i = 0; i < ImportedTextures.Count; i++)
        {
            ImportedTextures[i].ID = TextureList.Elements[i].ID;
        }

    }

    /// <summary>
    /// Functions that start the Importation
    /// </summary>
    public void Import()
    {
        StartCoroutine(CoImport());
    }

    /// <summary>
    /// Imports the texture to be generated from the assetbundle server
    /// Adds them to the list in the texture propreties after the user chose the wanted textures
    /// </summary>
    /// <returns></returns>
    IEnumerator CoImport()
    {

        // First import the selected/enabled textures in the texture chooser
        yield return StartCoroutine(UIImporter.IImportIntoEditorPanel<CTexture>());

        // Add the selected textures from the loader into the imported texture
        if(ImportedTextures == null)
            ImportedTextures = new List<CTexture>();

        for(int i = 0; i < UIImporter.Selected.Count; i++)
        {
            GameObject toAdd = Instantiate(UITextureElement);
            toAdd.transform.SetParent(Container.transform);
            toAdd.transform.localScale = new Vector3(1, 1, 1);

            CTexture texture = toAdd.GetComponent<CTexture>();
            CTexture importedTexture  = UIImporter.Selected[i] as CTexture;

            Text TextureName;
            Image TextureImage;


            TextureName = toAdd.GetComponentInChildren<Text>();
            TextureName.text = importedTexture.ObjectName;
            toAdd.name = TextureName.text;

            TextureImage = TextureName.GetComponentInChildren<Image>();
            TextureImage.sprite = importedTexture.ui_sprite;


            texture.ObjectName = importedTexture.ObjectName;
            texture.texture = importedTexture.texture;
            texture.ui_sprite = importedTexture.ui_sprite;
            texture.ui_object = toAdd;
            texture.avgColor = importedTexture.avgColor;

            Identifier++;

            texture.ID = Identifier-1;
            ImportedTextures.Add(texture);
        }

        TexturesAreImported = true;
        cmdTextureProp = Container.GetComponentsInChildren<Button>(true);

        for (int i = 0; i < cmdTextureProp.Length; i++)
        {
            CTexture linked = cmdTextureProp[i].GetComponentInParent<CTexture>();

            if (cmdTextureProp[i].name == "Propreties")
            {
                cmdTextureProp[i].onClick.AddListener(delegate { OpenTextureProp(linked.ID); });
            }
            if (cmdTextureProp[i].name == "remove")
            {
                cmdTextureProp[i].onClick.AddListener(delegate { RemoveTexture(linked.ID); });
            }
        }

        UIManager.Get.CloseFrame();

        TextureList.Init();
        yield return null;
    }

}
