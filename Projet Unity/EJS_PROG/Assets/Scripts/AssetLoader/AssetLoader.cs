using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetLoader : AssetBundleLoader {

    public Transform Container;

    public GameObject TogglePrefab;

    [HideInInspector]
    public Texture2D[] TexturePreview;
    [HideInInspector]
    public List<Sprite> PreviewSprite;


    public void StartLoading<T>() where T : Object
    {
        StartCoroutine(ILoadAsset<T>());
    }
    /// <summary>
    /// Load the texture from a remote Server
    /// Actually using local assetStreamingPath
    /// 
    /// get the terrain assetbundle that contains all the usable textures
    /// And create UIToggles for the user to interact with
    /// </summary>
    /// <returns></returns>
    public IEnumerator ILoadAsset<T>() where T : Object
    {
        yield return StartCoroutine(LoadLocal<T>(STORAGE.LOCAL));

        if(_loaded)
        {
            if (typeof(T) == typeof(Texture2D))
            {
                PreviewSprite = new List<Sprite>();
                TexturePreview = (Texture2D[])_assets;
                for (int i = 0; i < _assets.Length; i++)
                {
                    GameObject Prefab = Instantiate(TogglePrefab);
                    Prefab.transform.SetParent(Container);
                    Prefab.transform.localScale = new Vector3(1, 1, 1);
                    Image[] Preview = Prefab.GetComponentsInChildren<Image>();

                    for (int y = 0; y < Preview.Length; y++)
                    {
                        if (Preview[y].gameObject.name == "Background")
                        {
                            Preview[y].sprite = Sprite.Create(TexturePreview[i], new Rect(0, 0, TexturePreview[i].width, TexturePreview[i].height), new Vector2(0.5f, 0.5f), 100.0f);
                            PreviewSprite.Add(Preview[y].sprite);
                            break;
                        }
                    }

                    if (Prefab.GetComponentInChildren<Text>() != null)
                        Prefab.GetComponentInChildren<Text>().text = _assets[i].name;
                }
            }
            else if( typeof(T) == typeof(GameObject))
            {
                PreviewSprite = new List<Sprite>();
                for (int i = 0; i < _assets.Length; i++)
                {
                    GameObject Prefab = Instantiate(TogglePrefab);
                    Prefab.transform.SetParent(Container);
                    Prefab.transform.localScale = new Vector3(1, 1, 1);

                
                    if (Prefab.GetComponentInChildren<Text>() != null)
                        Prefab.GetComponentInChildren<Text>().text = _assets[i].name;
                }
            }
            
        }
        yield return null;
    }
}
