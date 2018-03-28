using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VegetationUI : MonoBehaviour {

    public Button cmdSelectVeg;
    public Button cmdGenerate;

    public GameObject frmVegSelection;
    public UIAssetSelectionLoader Loader;

	// Use this for initialization
	void Start () {
        cmdSelectVeg.onClick.AddListener(delegate
        {
            UIManager.Get.OpenFrame(frmVegSelection);
            Loader.LoadAssetSelection();
        });

    }
	

}
