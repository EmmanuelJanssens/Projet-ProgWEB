using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportTextures : MonoBehaviour {

    public LoadAvailableTextures _textureLoader;


    public Button cmdImport;
    public Button[] cmdTextureProp;
    public Text txtTitle;
    public Image sprTexture;

    public GameObject _frmTextureProp;
    public GameObject _textureElement;

    public InputField Slope;
    public InputField Height;
    public InputField Orientation;

    public InputField Influence;

    public List<CTexture> addedTextures;

    public CTexture[] allLoadedTexture;

    public static int Identifier = 0;

    // Use this for initialization
    void Start ()
    {
        cmdImport.onClick.AddListener(Import);  
    }
	
    public void OpenTextureProp(int id)
    {
        UIManager.Get.OpenFrame(_frmTextureProp);

        txtTitle.text = addedTextures[id].name;
        sprTexture.sprite = addedTextures[id].sprite;


    }
    public void RemoveTexture()
    {

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
        yield return StartCoroutine(_textureLoader.ImportTextures());


        // Set the data for the texture elements ( texture element prefab )
        addedTextures = new List<CTexture>();
        for(int i = 0; i < _textureLoader._Selected.Count; i++)
        {
            GameObject toAdd = Instantiate(_textureElement);
            toAdd.transform.SetParent(this.transform);
            toAdd.transform.localScale = new Vector3(1, 1, 1);

            CTexture texture = toAdd.GetComponent<CTexture>();
            
            Text TextureName;
            Image TextureImage;

            TextureName = toAdd.GetComponentInChildren<Text>();
            TextureName.text = _textureLoader._Selected[i].name;

            TextureImage = TextureName.GetComponentInChildren<Image>();
            TextureImage.sprite = _textureLoader._Selected[i].sprite;

            texture.name = _textureLoader._Selected[i].name;
            texture.texture = _textureLoader._Selected[i].texture;
            texture.sprite = _textureLoader._Selected[i].sprite;       

            Identifier++;

            texture.ID = Identifier-1;
            addedTextures.Add(texture);
        }
        _textureLoader.Imported = true;
        cmdTextureProp = gameObject.GetComponentsInChildren<Button>(true);

        for (int i = 0; i < cmdTextureProp.Length; i++)
        {
            if(cmdTextureProp[i].name == "Propreties")
            {
                CTexture linked = cmdTextureProp[i].GetComponentInParent<CTexture>();
                cmdTextureProp[i].onClick.AddListener(delegate { OpenTextureProp(linked.ID); });

            }
            if (cmdTextureProp[i].name == "remove")
                cmdTextureProp[i].onClick.AddListener(RemoveTexture);
        }
        yield return null;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
