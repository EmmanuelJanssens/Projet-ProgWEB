using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Imports texture into the list of 
/// textures to be generated 
/// Added onto _textureImporter gameobject
/// </summary>
public class ImportTextures : MonoBehaviour
{

    public static ImportTextures current;

    /// <summary>
    /// Needed to access all loaded texture from the assetbundle
    /// </summary>
    [HideInInspector]
    public ImportAssets Importer;

    /// <summary>
    /// Container of the graphical elements
    /// </summary>
    public GameObject goContainer;

    /// <summary>
    /// Interaction with the texture elements
    /// </summary>
    [HideInInspector]
    public Button[] cmdTextureProp;
    public Text txtTitle;
    public Image sprTexture;

    /// <summary>
    /// Command that imports the textures from the bundle
    /// </summary>
    public Button cmdImport;

    /// <summary>
    /// Frame template that shows the propreties from the texture
    /// </summary>
    public GameObject _frmTextureProp;

    /// <summary>
    /// Template to an texture element
    /// A CTexture script is attached to IT
    /// contains a Text,Image,RemoveButton and propreties Button
    /// </summary>
    public GameObject _textureElement;

    /// <summary>
    /// List of textures that are available into the generator
    /// </summary>
    [HideInInspector]
    public List<CTexture> ImportedTextures;

    /// <summary>
    /// List of the textures
    /// </summary>
    public ListManager TextureList;

    /// <summary>
    /// Counts the total of textures to affect an unique identifier to them
    /// </summary>
    public static int Identifier = 0;

    public static ImportTextures Get { get { return current; } }

    public bool TextureImported = false;
    // Use this for initialization
    void Start ()
    {

        Importer = gameObject.GetComponent<ImportAssets>();

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
        TexturePropreties proprs = _frmTextureProp.GetComponent<TexturePropreties>();
        proprs.toModify = ImportedTextures[id];

        txtTitle.text = ImportedTextures[id].Name;
        sprTexture.sprite = ImportedTextures[id].sprite;

        UIManager.Get.OpenFrame(_frmTextureProp);
    }

    /// <summary>
    /// Removes an element from the list and updates the identification Numbers of all the texture
    /// </summary>
    /// <param name="id">Identification of the texture that has to be removed</param>
    public void RemoveTexture(int id)
    {
        Destroy(ImportedTextures[id].localGameobject);
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
        yield return StartCoroutine(Importer.Import<CTexture>());

        // Add the selected textures from the loader into the imported texture
        if(ImportedTextures == null)
            ImportedTextures = new List<CTexture>();

        for(int i = 0; i < Importer.Selected.Count; i++)
        {
            GameObject toAdd = Instantiate(_textureElement);
            toAdd.transform.SetParent(goContainer.transform);
            toAdd.transform.localScale = new Vector3(1, 1, 1);

            CTexture texture = toAdd.GetComponent<CTexture>();
            CTexture importedTexture  = Importer.Selected[i] as CTexture;

            Text TextureName;
            Image TextureImage;


            TextureName = toAdd.GetComponentInChildren<Text>();
            TextureName.text = importedTexture.Name;
            toAdd.name = TextureName.text;

            TextureImage = TextureName.GetComponentInChildren<Image>();
            TextureImage.sprite = importedTexture.sprite;

            texture.Name = importedTexture.Name;
            texture.texture = importedTexture.texture;
            texture.sprite = importedTexture.sprite;
            texture.localGameobject = toAdd;

            Identifier++;

            texture.ID = Identifier-1;
            ImportedTextures.Add(texture);
        }

        TextureImported = true;
        cmdTextureProp = goContainer.GetComponentsInChildren<Button>(true);

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
