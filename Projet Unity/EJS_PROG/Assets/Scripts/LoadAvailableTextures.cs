using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Single instance class to load textures from a remote server
/// </summary>
public class LoadAvailableTextures : MonoBehaviour
{

    public static LoadAvailableTextures current;

    /// <summary>
    /// Container of the toggles for the available textures
    /// </summary>
    public GameObject goContainer;
    /// <summary>
    /// Prefab of a specificToggle
    /// </summary>
    public GameObject _textureTogglePrefab;

    /// <summary>
    /// Bundle that is to be loaded
    /// </summary>
    private AssetBundle _Bundle;

    private WWW www;

    /// <summary>
    /// Array of available textures in the assetBundle
    /// </summary>
    [SerializeField]
    public Texture2D[] _AvailableTextures;
    public Sprite[] _TextureUISprite;

    /// <summary>
    /// List of the toggles that where ON
    /// elements are added when the OK Button from the frame is pressed  
    /// </summary>
    public List<CTexture> Selected;

    /// <summary>
    /// Keep track of the state of texture loading
    /// </summary>
    public bool Loaded = false;
    public bool Imported = false;

    public static LoadAvailableTextures Get { get { return current; } }

    public void Start()
    {
        {
            if (current == null)
                current = this;
        }
    }

    /// <summary>
    /// When the frame is opened the TextureLoader Starts automaticaly
    /// </summary>
    public void OnEnable()
    {
        StartCoroutine(ILoadTextures());
    }


    /// <summary>
    /// Imports the Toggeled textures
    /// </summary>
    /// <returns></returns>
    public IEnumerator ImportTextures()
    {
        if(Loaded)
        {

            Toggle[] enabled = goContainer.GetComponentsInChildren<Toggle>();

            if (Selected != null)
                Selected = null;

            Selected = new List<CTexture>();

            for(int i = 0; i < enabled.Length; i++)
            {
                if(enabled[i].isOn)
                {
                    CTexture toAdd = new CTexture();
                    toAdd.sprite = _TextureUISprite[i];
                    toAdd.texture = _AvailableTextures[i];
                    toAdd.name = _AvailableTextures[i].name;
                    toAdd.steepness = 0.5f;
                    toAdd.height = 0.5f;
                    toAdd.orientation = 45f;

                    Selected.Add(toAdd);
                }
            }

            //unload the unused textures
            _Bundle.Unload(false);
            _Bundle = null;
            _TextureUISprite = null;
            _AvailableTextures = null;

            for(int i = 0; i <  gameObject.GetComponentsInChildren<Toggle>().Length; i++)
            {
                Destroy(gameObject.GetComponentsInChildren<Toggle>()[i].gameObject);
            }

            Loaded = false;

        }
        yield return null;
    }

    /// <summary>
    /// Load the texture from a remote Server
    /// Actually using local assetStreamingPath
    /// 
    /// get the terrain assetbundle that contains all the usable textures
    /// And create UIToggles for the user to interact with
    /// </summary>
    /// <returns></returns>
    public IEnumerator ILoadTextures()
    {
        if(!Loaded)
        {
            if (_Bundle == null)
            {
                /*www = new WWW(Application.streamingAssetsPath + "/terrain");
                yield return www;
                _Bundle = www.assetBundle;*/

                _Bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath,"terrain"));
            }

            /*if (www.error == null)
            {*/
                _AvailableTextures = _Bundle.LoadAllAssets<Texture2D>();
                
                if (_AvailableTextures != null)
                {
                    Debug.Log(_Bundle.name + " Loaded");

                    _TextureUISprite = new Sprite[_AvailableTextures.Length];

                    for (int i = 0; i < _AvailableTextures.Length; i++)
                    {
                        GameObject newObject = Instantiate(_textureTogglePrefab);
                        newObject.transform.SetParent(goContainer.transform);
                        newObject.transform.localScale = new Vector3(1, 1, 1);
                        Image[] BG = newObject.GetComponentsInChildren<Image>();
                        _TextureUISprite[i] = Sprite.Create(_AvailableTextures[i], new Rect(0, 0, _AvailableTextures[i].width, _AvailableTextures[i].height), new Vector2(0.5f, 0.5f), 100.0f);

                        for (int y = 0; y < BG.Length; i++)
                        {
                            if (BG[y].gameObject.name == "Background")
                            {
                                BG[y].sprite = _TextureUISprite[i];
                                break;
                            }
                        }
                    }
                    Loaded = true;
                    yield return null;

                }
                else
                {
                    Debug.Log("No bundles Available");
                }
            }
            /*else
            {
                Debug.Log("Could not load Bundle : " + www.error);
            }
            yield return null;
        }*/
        yield return null;
    }
}
