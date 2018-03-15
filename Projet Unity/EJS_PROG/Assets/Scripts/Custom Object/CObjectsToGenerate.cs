using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CObjectsToGenerate : MonoBehaviour
{

    public ImportAssets SelectedObjectToImport;

    [Header("Elements to fill List")]

    public Transform Container;

    public GameObject UIListElementPrefab;

    public List<GameObject> AllObjectsToGenerate;
    public List<CObject> AllAddedCustomObject;

    public ListManager ElementList;
    
    public bool AllElementsArePresent { get; set; }
    

    public void StartImportation<T>() where T : CObject
    {
        StartCoroutine(ImportObjectsToGenerate<T>());
    }



    public IEnumerator ImportObjectsToGenerate<T>() where T : CObject
    {
        //Start importation of the Elements in to the list
        yield return StartCoroutine(SelectedObjectToImport.IImportIntoEditorPanel<T>());

        AllObjectsToGenerate = new List<GameObject>();
        AllAddedCustomObject = new List<CObject>();

        for (int i = 0; i < SelectedObjectToImport.Selected.Count; i++)
        {
            GameObject toAdd = Instantiate(UIListElementPrefab);
            toAdd.transform.SetParent(Container);
            toAdd.transform.localScale = new Vector3(1, 1, 1);


            CObject selected = SelectedObjectToImport.Selected[i] as CObject;

           

            Text elementName;
            Image elementImage;

            elementName = toAdd.GetComponentInChildren<Text>();
            elementName.text = selected.Name;

            elementImage = elementName.GetComponentInChildren<Image>();
            elementImage.sprite = selected.sprite;

            selected.ui_object = toAdd;


            AllAddedCustomObject.Add(selected);

            AllObjectsToGenerate.Add(selected.ui_object);
        }

        yield return null;
    }
}
