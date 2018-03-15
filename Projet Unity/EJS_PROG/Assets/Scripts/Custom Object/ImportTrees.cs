using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Imports tree gameobjects 
/// </summary>
public class ImportTrees : MonoBehaviour
{

    public static ImportTrees current;

    public ImportAssets UIImporter;

    public Transform Container;

    public Button cmdImport;


    public GameObject UITreeElement;

    public List<CTree> ImportedTrees;

    public void Load()
    {
        StartCoroutine(LoadTreeGameObjects());
    }

    public IEnumerator LoadTreeGameObjects()
    {
        //Load the trees
        yield return StartCoroutine(UIImporter.IImportIntoEditorPanel<CTree>());

        for(int i = 0; i < UIImporter.Selected.Count; i++)
        {
            GameObject UIToAdd = Instantiate(UITreeElement);
            UIToAdd.transform.SetParent(Container.transform);
            UIToAdd.transform.localScale = new Vector3(1, 1, 1);

            CTree importedTree = UIImporter.Selected[i] as CTree;

            Text TextureName;
            Image TextureImage;


            TextureName = UIToAdd.GetComponentInChildren<Text>();
            TextureName.text = importedTree.ui_object.name;
            UIToAdd.name = TextureName.text;

            TextureImage = TextureName.GetComponentInChildren<Image>();
            TextureImage.sprite = importedTree.sprite;

        }

        ///Add assets into dictionary
        UIManager.Get.CloseFrame();

        yield return null;
    }


    public void Start()
    {
        cmdImport.onClick.AddListener(Load);
    }
}
