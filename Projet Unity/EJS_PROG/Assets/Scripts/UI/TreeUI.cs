using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeUI : MonoBehaviour {


    public Button cmdSelectTrees;
    public GameObject frmTreeSelection;
    public ImportTrees Loader;
	// Use this for initialization
	void Start () {
        cmdSelectTrees.onClick.AddListener(delegate
        {
            Loader.Importer.StartLoading<Texture2D>();
            UIManager.Get.OpenFrame(frmTreeSelection);
        });	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
