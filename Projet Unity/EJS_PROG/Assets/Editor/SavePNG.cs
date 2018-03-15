using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class SavePNG : EditorWindow {

    public string folder;
    Texture2D[] _previews;
    Object[] _objects;
    [MenuItem("Window/SavePNG")]
	// Use this for initialization
    static void Init()
    {
        SavePNG window = (SavePNG)EditorWindow.GetWindow(typeof(SavePNG));
        window.Show();
        
        
    }

    int selection = 0;
	// Update is called once per frame
	void OnGUI ()
    {

        folder  = GUILayout.TextField(folder);

        if (GUILayout.Button("Get Textures"))
        {
            _objects = get<Object>(folder);
            Debug.Log("LOADED  " + _objects.Length);
        }

        if(GUILayout.Button("Save Images"))
        {

            if(_objects.Length > 0 && _objects != null)
            {
                Debug.Log("Writing images");
                _previews = new Texture2D[_objects.Length];
                for (int i = 0; i < _objects.Length; i++)
                {
                    _previews[i] = AssetPreview.GetAssetPreview(_objects[i]);
                    byte[] bytes = ImageConversion.EncodeToPNG(_previews[i]);

                    if (!Directory.Exists(Application.streamingAssetsPath + "/" + folder))
                        Directory.CreateDirectory(Application.streamingAssetsPath + "/" + folder);

                    File.WriteAllBytes(Application.streamingAssetsPath + "/" +folder+"/"+ _objects[i].name + ".png", bytes);
                }

                AssetDatabase.Refresh();
            }
        }
    }

    public IEnumerator WriteToFile()
    {

        yield return null;
    }
    public static T[] get<T>(string path) where T : Object
    {
        T []lst;

        lst = Resources.LoadAll<T>( path) as T[];

        foreach (T o in lst)
        {
            Debug.Log(o);
        }
        
        return lst;
    }
   
}
