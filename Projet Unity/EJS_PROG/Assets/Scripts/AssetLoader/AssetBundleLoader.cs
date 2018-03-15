using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

/// <summary>
/// Used to load assets from an asset bundle
/// \\Local or remote storage
/// </summary>
[SerializeField]
public class AssetBundleLoader : MonoBehaviour
{
    public enum STORAGE { LOCAL, REMOTE }

    [SerializeField]
    protected string _bundlePath;
    public string BundlePath { get { return _bundlePath; } set { _bundlePath = value; } }

    [SerializeField]
    protected string _bundleName;
    public string BundleName { get { return _bundleName; } set { _bundleName = value; } }

    protected AssetBundle _bundle;
    public AssetBundle Bundle { get { return _bundle; } }


    protected bool _loaded = false;
    public bool IsLoaded { get { return _loaded; } }

    protected Object[] _assets;
    public Object[] Assets { get { return _assets; } }

    
    public IEnumerator LoadAssetBundle<T>(STORAGE storage) where T : Object
    {
        if(_bundle == null)
        {
            switch(storage)
            {
                case STORAGE.LOCAL:
                    _bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + "/Bundles/", _bundleName));
                    break;

                case STORAGE.REMOTE:
                    _bundle = AssetBundle.LoadFromFile(Path.Combine(_bundlePath, _bundleName));
                    break;
            }
        }

        if(_bundle != null)
        {
            _assets = _bundle.LoadAllAssets<T>();

            if(_assets != null)
            {
                Debug.Log(_bundleName + " is loaded");
                _loaded = true;
            }
        }
        yield return null;
    }

}
