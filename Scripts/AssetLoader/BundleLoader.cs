using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleLoader : MonoBehaviour {

    public List<GDBundle> BundleList;
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
    public void Load<T>(string name) where T : Object
    {
        if (!Bundles[name].Loaded)
            Loader = StartCoroutine(Bundles[name].Load<T>(Application.streamingAssetsPath + "/Bundles/"));
        else
            Debug.Log(name + " is already loaded");
    }

    public void LoadAssync<T>(string name) where T : Object
    {
        if (!Bundles[name].Loaded)
            Loader = StartCoroutine(Bundles[name].Load<T>(Application.streamingAssetsPath + "/Bundles/"));
        else
            Debug.Log(name + " is already loaded");
    }

    
}
