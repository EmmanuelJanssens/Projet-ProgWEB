using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Loads Texture2D for UI previews 
/// </summary>
public class UIAssetSelectionLoader : MonoBehaviour {

    [Header("UI Bundle")]
    /// <summary>
    /// Bundle loader to load UI textures 
    /// </summary>
    public AssetBundleLoader UI_ABLoader;

    [Header("In Game Bundle")]
    public AssetBundleLoader ABLoader;

    [Header("UI Display details")]
    /// <summary>
    /// prefab of the toggle to be instantiated
    /// </summary>
    [Tooltip("Prefab of the toggle to be used")]
    public GameObject TogglePrefab;

    /// <summary>
    /// Container where the toggles will be shown
    /// </summary>
    [Tooltip("Content where the texture will be displayed")]
    public Transform Container;


    /// <summary>
    /// Sprite to be shown
    /// </summary>
    [HideInInspector]
    public Sprite[] PreviewSprite;

    public Dictionary<string, Object> LinkedBundles;

    /// <summary>
    /// Start the coroutine that loads all the available textures
    /// and displays them as toggles
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void LoadAssetSelection()
    {
        StartCoroutine(ILoadAssetSelection());
    }


    /// <summary>
    /// Load the texture from a remote Server
    /// Actually using local assetStreamingPath
    /// 
    /// get the terrain assetbundle that contains all the usable textures
    /// And create UIToggles for the user to interact with
    /// </summary>
    /// <returns></returns>
    public IEnumerator ILoadAssetSelection() 
    {
        
        //loads the assets
        yield return StartCoroutine(UI_ABLoader.LoadAssetBundle<Sprite>(AssetBundleLoader.STORAGE.LOCAL));

        if(UI_ABLoader.IsLoaded)
        {
            PreviewSprite = UI_ABLoader.Assets as Sprite[];

            LinkedBundles = new Dictionary<string, Object>();

            string bundlename = UI_ABLoader.BundleName.Substring(0, UI_ABLoader.BundleName.Length - 3);

            ABLoader.BundleName = bundlename;

            yield return ABLoader.LoadAssetBundle<Object>(AssetBundleLoader.STORAGE.LOCAL);

            for (int i = 0; i < UI_ABLoader.Assets.Length; i++)
            {
                GameObject Prefab = Instantiate(TogglePrefab);
                Prefab.transform.SetParent(Container);
                Prefab.transform.localScale = new Vector3(1, 1, 1);

                LinkedBundles[PreviewSprite[i].name] = ABLoader.Assets[i];

                Image[] previewImage = Prefab.GetComponentsInChildren<Image>();
                
                for (int y = 0; y < previewImage.Length; y++)
                {
                    //find the background of the toggle
                    if (previewImage[y].gameObject.name == "Background")
                    {
                        previewImage[y].sprite = PreviewSprite[i];
                        
                        break;
                    }
                }

                //IF there is a label to be added 
                if (Prefab.GetComponentInChildren<Text>() != null)
                    Prefab.GetComponentInChildren<Text>().text = UI_ABLoader.Assets[i].name;
            }          
        }
        yield return null;
    }

    public void Free()
    {
        UI_ABLoader.Bundle.Unload(true);
    }
}
