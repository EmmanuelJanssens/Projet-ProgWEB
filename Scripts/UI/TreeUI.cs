using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeUI : MonoBehaviour {


    public Button cmdSelectTrees;
    public Button cmdGenerate;

    public GameObject frmTreeSelection;
    public UIAssetSelectionLoader Loader;


	// Use this for initialization
	void Start () {
        cmdSelectTrees.onClick.AddListener(delegate
        {
            UIManager.Get.OpenFrame(frmTreeSelection);
            Loader.LoadAssetSelection();

        });

        cmdGenerate.onClick.AddListener(AppManager.Get.TreeMap.Generate);
	}
	

}
