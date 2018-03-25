using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    
public class ImportAssets : MonoBehaviour
{
    [Header("Asset UI Selection")]
    public UIAssetSelectionLoader UI_Selection;

    public List<CObject> Selected;

    /// <summary>
    /// Start Importation into toggle list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void ImportIntoEditorPanel<T>()
    {
        StartCoroutine(IImportIntoEditorPanel<T>());
    }


    /// <summary>
    /// Import the Assets from the UI_Selection tool
    /// Import the asset from the bundle
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerator IImportIntoEditorPanel<T>()
    {
        //Load bundles
        if(UI_Selection.ABLoader.IsLoaded)
        {
            Toggle[] toggles = UI_Selection.Container.GetComponentsInChildren<Toggle>();

            if(Selected != null)
            {
                Selected = null;
            }

            Selected = new List<CObject>();

            for(int i = 0; i < toggles.Length; i++)
            {
                if(toggles[i].isOn)
                {
                    if (typeof(T) == typeof(CTexture))
                    {
                        CTexture toAdd = new CTexture();
                        toAdd.ui_sprite = UI_Selection.PreviewSprite[i];
                        toAdd.texture = UI_Selection.LinkedBundles[UI_Selection.PreviewSprite[i].name] as Texture2D;
                        toAdd.ObjectName = UI_Selection.PreviewSprite[i].name;
                        toAdd.steepness = 0.5f;
                        toAdd.height = 0.5f;
                        toAdd.avgColor = Color.green;
                        toAdd.orientation = 45f;


                        Selected.Add(toAdd);
                    }
                    else if (typeof(T) == typeof(CTree))
                    {
                        CTree toAdd = new CTree();
                        toAdd.ui_sprite = UI_Selection.PreviewSprite[i];
                        toAdd.ObjectName = UI_Selection.PreviewSprite[i].name;
                        toAdd.Description = "yolo";
                        toAdd.age = 0;
                        toAdd.treeHeight = 0;
                        toAdd.avgColor = Color.green;
                        toAdd.obj = UI_Selection.LinkedBundles[UI_Selection.PreviewSprite[i].name] as GameObject;
                        Selected.Add(toAdd);
                    }
                }              
            }
            for (int i = 0; i < toggles.Length; i ++)
            {
                Destroy(toggles[i].gameObject);
            }

        }

        yield return null;
    }

   
}
