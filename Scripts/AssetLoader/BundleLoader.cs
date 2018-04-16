using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Load available bundles
/// </summary>
public class BundleLoader : MonoBehaviour {

    /// <summary>
    /// List of bundles that have to be loaded
    /// </summary>
    public List<GDBundle> BundleList;

    /// <summary>
    /// Dictionary of the bunddles that where loaded
    /// </summary>
    public Dictionary<string, GDBundle> Bundles;

    public Coroutine Loader;

    public void Start()
    {
        Bundles = new Dictionary<string, GDBundle>();
        for(int i  = 0; i < BundleList.Count; i++)
        {
            string name = BundleList[i].Name;
            Bundles[name] = BundleList[i];
            Bundles[name + "_ui"] = new GDBundle();
            Bundles[name + "_ui"].Name = name + "_ui";
        }

    }
    public IEnumerator Load<T>(string name) where T : Object
    {
        if (!Bundles[name].Loaded)
        {
            Loader = StartCoroutine(Bundles[name].Load<T>(Application.streamingAssetsPath + "/Bundles/"));
            yield return Loader;
        }
        else
            Debug.Log(name + " is already loaded");

        yield return null;
    }

    public IEnumerator LoadAssync<T>(string name) where T : Object
    {
        if (!Bundles[name].Loaded)
        {
            Loader = StartCoroutine(Bundles[name].LoadAsync<T>(Application.streamingAssetsPath + "/Bundles/"));
            yield return Loader;
        }
        else
            Debug.Log(name + " is already loaded");

        yield return null;
    }

    
}
