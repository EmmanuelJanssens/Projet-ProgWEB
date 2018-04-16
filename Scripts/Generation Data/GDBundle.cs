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

    /// <summary>
    /// Asset bundle to be laoded
    /// </summary>
    [HideInInspector]
    public AssetBundle Bundle;

    /// <summary>
    /// Name of the bundle
    /// </summary>
    public string Name;

    /// <summary>
    /// Check if the bundle was loaded corectly
    /// </summary>
    [HideInInspector]
    public bool Loaded = false;

    /// <summary>
    /// Array of all the object present in the bundle
    /// </summary>
    [HideInInspector]
    public Object[] Assets; //Array of loaded assets

    /// <summary>
    /// Loads a bundle
    /// </summary>
    /// <typeparam name="T">Typem of object to be loaded must be a derived class from object</typeparam>
    /// <param name="path">path of the bundle</param>
    /// <returns></returns>
    public IEnumerator Load<T>(string path ) where T : Object
    {
        try
        {
            //Load the bundle
            Bundle = AssetBundle.LoadFromFile(Path.Combine(path, Name));
            Debug.Log("Bundle "+ Name +" loaded successfully from file");
            try
            {
                //Load the assets
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
    
    /// <summary>
    /// Load the bundles asynchrounosly from a file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public IEnumerator LoadAsync<T>(string path) where T : Object
    {
        AssetBundleCreateRequest async = AssetBundle.LoadFromFileAsync(Path.Combine(path, Name));
        //Display a message while the bundle is not loaded
        while(!async.isDone)
        {
            //display message
            Debug.Log("Progress " + async.progress);
            yield return null;
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
