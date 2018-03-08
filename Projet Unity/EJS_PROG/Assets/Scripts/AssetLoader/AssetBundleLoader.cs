using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

/// <summary>
/// Used to load assets from an asset bundle
/// \\Local or remote storage
/// </summary>
public class AssetBundleLoader : MonoBehaviour
{
    public enum STORAGE { LOCAL, REMOTE }
    [SerializeField]
    protected string _bundlePath;
    [SerializeField]
    protected string _bundleName;
    protected AssetBundle _bundle;
    protected Texture2D _assetPreview;
    protected bool _loaded = false;

    protected Object[] _assets;

    public string BundlePath { get { return _bundlePath; } set { _bundlePath = value; } }
    public string BundleName { get { return _bundleName; } set { _bundleName = value; } }
    public AssetBundle Bundle { get { return _bundle; } }
    public Texture2D AssetPreview { get { return _assetPreview; } }
    public bool IsLoaded { get { return _loaded; } }



    public IEnumerator LoadLocal<T>(STORAGE storage) where T : Object
    {
        if(_bundle == null)
        {
            switch(storage)
            {
                case STORAGE.LOCAL:
                    _bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, _bundleName));
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


	// Update is called once per frame
	void Update () {
		
	}
}
