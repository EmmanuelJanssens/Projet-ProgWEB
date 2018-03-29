using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

/// <summary>
/// Contains data from a bundle to load
/// </summary>
/// 
[System.Serializable]
public class GDBundle 
{
    [HideInInspector]
    public AssetBundle Bundle;

    public string Name;

    [HideInInspector]
    public bool Loaded = false;

    [HideInInspector]
    public Object[] Assets; //Array of loaded assets

    public IEnumerator Load<T>(string path ) where T : Object
    {
        try
        {
            Bundle = AssetBundle.LoadFromFile(Path.Combine(path, Name));
            Debug.Log("Bundle "+ Name +" loaded successfully from file");
            try
            {
                Assets = Bundle.LoadAllAssets<T>();
                Debug.Log("Bundle " + Name + " Successfully loaded");
                Loaded = true;
            }
            catch(System.Exception e)
            {
                Debug.LogError("Could not load Assets from Bundle with name " + Name + " " + e.Message);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Could not load Bundle with name " + Name + " " + e.Message);
        }
        yield return null;
    }
    
    public IEnumerator LoadAsync<T>(string path) where T : Object
    {
        AssetBundleCreateRequest async = AssetBundle.LoadFromFileAsync(Path.Combine(path, Name));
        //Display a message while the bundle is not loaded
        while(!async.isDone)
        {
            //display message
            Debug.Log("Progress " + async.progress);
        }

        //When done loading 
        Bundle = async.assetBundle;

        try
        {
            Assets = Bundle.LoadAllAssets<T>();
            Debug.Log("Bundle " + Name + " Successfully loaded");
            Loaded = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Could not load Assets from Bundle with name " + Name + " " + e.Message);

        }

        //End of the coroutine
        yield return null;
    }
}
