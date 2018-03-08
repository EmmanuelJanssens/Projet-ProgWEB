using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    
public class ImportAssets : AssetLoader {

    public List<Object> Selected;

    public void StartImport<T>()
    {
        StartCoroutine(Import<T>());
    }
    public IEnumerator Import<T>()
    {
        if(_loaded)
        {
            Toggle[] toggles = Container.GetComponentsInChildren<Toggle>();

            if(Selected != null)
            {
                Selected = null;
            }

            Selected = new List<Object>();

            for(int i = 0; i < toggles.Length; i++)
            {
                if(toggles[i].isOn)
                {
                    if (typeof(T) == typeof(CTexture))
                    {
                        CTexture toAdd = new CTexture();
                        toAdd.sprite = PreviewSprite[i];
                        toAdd.texture = TexturePreview[i];
                        toAdd.Name = TexturePreview[i].name;
                        toAdd.steepness = 0.5f;
                        toAdd.height = 0.5f;
                        toAdd.orientation = 45f;

                        Selected.Add(toAdd);
                    }
                    else if (typeof(T) == typeof(CTree))
                    {
                        CTree toAdd = new CTree();
                        toAdd.obj = (GameObject)_assets[i];
                        toAdd.Name = "Tree";
                        toAdd.description = "yolo";
                        toAdd.age = 0;
                        toAdd.height = 0;

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
