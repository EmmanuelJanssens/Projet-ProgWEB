using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportTextures : MonoBehaviour {

    public LoadAvailableTextures _textureLoader;


    public Button cmdImport;
    public GameObject _textureElement;


    public List<CTexture> addedTextures;

    // Use this for initialization
    void Start ()
    {
        cmdImport.onClick.AddListener(Import);
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

            Text TextureName;
            Image TextureImage;

            TextureName = toAdd.GetComponentInChildren<Text>();
            TextureName.text = _textureLoader._Selected[i].name;

            TextureImage = TextureName.GetComponentInChildren<Image>();
            TextureImage.sprite = _textureLoader._Selected[i].sprite;

            addedTextures.Add(_textureLoader._Selected[i]);
        }
        _textureLoader._imported = true;

        yield return null;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
