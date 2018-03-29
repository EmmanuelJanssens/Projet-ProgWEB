using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseAssets : MonoBehaviour
{
    private string _uiBundleName;
    private string _bundleName;

    public GameObject ElementAvailable;
    public GameObject ElementSelected;
    public GameObject frmElementPropreties;

    public Button StartImportation;
    public Button[] cmdOpenPropreties;
    public BundleLoader Loader;

    public List<GDTexture> ChoosenTextures;
    public List<GDPlant> ChoosenPlants;


    private ListManager _listManager;
	// Use this for initialization
	void Start ()
    {
        ChoosenTextures = new List<GDTexture>();
        ChoosenPlants = new List<GDPlant>();
            
        StartImportation.onClick.AddListener(
            delegate
            {             
                StartCoroutine(ExportAssets());
                //Clean the transform's children for next use
                foreach(Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
            });

	}

    /// <summary>
    /// When the frame opens whe have to start the displaying
    /// </summary>
    public void OnEnable()
    {
        _listManager = UIManager.Get.CurrentPanel.GetComponentInChildren<ListManager>();

        _listManager.onEvent.onDelete -= RemoveElementFromChoosen;
        _listManager.onEvent.onDelete += RemoveElementFromChoosen;

        StartCoroutine(DisplayAssets());
    }

    /// <summary>
    /// Displays all the available UI elements into  the selection frame
    /// </summary>
    /// <returns></returns>
    public IEnumerator DisplayAssets()
    {
       
        _uiBundleName = UIManager.Get.Title.text.ToLower() + "_ui";
        _bundleName = UIManager.Get.Title.text.ToLower();

        Loader.Load<Sprite>(_uiBundleName);

        for(int i = 0; i < Loader.Bundles[_uiBundleName].Assets.Length; i++)
        {
            GameObject clone = Instantiate(ElementAvailable);
            clone.transform.SetParent(transform);
            clone.transform.localScale = new Vector3(1, 1, 1);
            clone.GetComponentInChildren<Image>().sprite = Loader.Bundles[_uiBundleName].Assets[i] as Sprite;
        }
        yield return null;
    }


    /// <summary>
    /// Exports the assets to be used with custom objects
    /// Creates an array of all selected assets (toogle.IsOn = true)
    /// And simultanously creates Elements into a list 
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExportAssets()
    {
        _listManager = UIManager.Get.CurrentPanel.GetComponentInChildren<ListManager>();
        GameObject DisplayList = _listManager.gameObject;
        Toggle[] all = gameObject.GetComponentsInChildren<Toggle>();

        for(int i = 0; i < all.Length; i++)
        {
            if(all[i].isOn)
            {
                GameObject clone = Instantiate(ElementSelected);
                clone.transform.SetParent(DisplayList.transform);
                clone.transform.localScale = new Vector3(1, 1, 1);
                clone.GetComponent<ListElement>().Init();

                Sprite ICON = Loader.Bundles[_uiBundleName].Assets[i] as Sprite;
                clone.GetComponentInChildren<Text>().text = ICON.name;
                clone.GetComponentsInChildren<Image>()[1].sprite = ICON;

                switch (UIManager.Get.CurrentPanel.name)
                {
                    case "terrain":
                        GDTexture texture = new GDTexture();
                        texture.texture = Loader.Bundles[_bundleName].Assets[i] as Texture2D;
                        texture.ObjectName = ICON.name;
                        texture.avgColor = AverageColorFromTexture(ICON.texture);
                        texture.ui_sprite = ICON;

                        ChoosenTextures.Add(texture);

                        break;
                    case "vegetation":
                        GDPlant vegetal = new GDPlant();
                        vegetal.obj = Loader.Bundles[_bundleName].Assets[i] as GameObject;
                        vegetal.ObjectName = ICON.name;
                        vegetal.avgColor = AverageColorFromTexture(ICON.texture);
                        vegetal.ui_sprite = ICON;

                        ChoosenPlants.Add(vegetal);
                        break;
                    case "rock":
                        break;
                }
            }
        }

        cmdOpenPropreties = null;
        cmdOpenPropreties = _listManager.GetComponentsInChildren<Button>();
        for(int i  = 0; i <  cmdOpenPropreties.Length; i++)
        {
            if (cmdOpenPropreties[i].name == "Propreties")
            {
                ListElement linked = cmdOpenPropreties[i].GetComponentInParent<ListElement>();
                cmdOpenPropreties[i].onClick.AddListener(delegate { OpenPropreties(linked.ID); });
            }
        }


        _listManager.Init();

        UIManager.Get.CloseFrame();
        yield return null;
    }

    public void OpenPropreties(int i)
    {
        switch(UIManager.Get.CurrentPanel.name)
        {
            case "terrain":
                AppManager.Get.ObjectToModify = ChoosenTextures[i];
                break;
            case "vegetation":
                AppManager.Get.ObjectToModify = ChoosenPlants[i];
                break;
            case "rock":
                break;
        }
        UIManager.Get.OpenFrame(frmElementPropreties);
    }

    public void RemoveElementFromChoosen(int id)
    {
        switch (UIManager.Get.CurrentPanel.name)
        {
            case "terrain":
                ChoosenTextures.RemoveAt(id);
                break;
            case "vegetation":
                ChoosenPlants.RemoveAt(id);
                break;
            case "rock":
                break;
        }
    }

    public void MoveElementUp(int id)
    {
        switch (UIManager.Get.CurrentPanel.name)
        {
            case "terrain":
                Debug.Log("Removing " + ChoosenTextures[id].ObjectName);
                if (id != 0)
                {
                    GDObject removed = ChoosenTextures[id];
                    ChoosenTextures.Insert(id - 1, ChoosenTextures[id]);
                    ChoosenTextures.RemoveAt(id);
                }
                break;
            case "vegetation":
                Debug.Log("Removing " + ChoosenPlants[id].ObjectName);
                if (id != 0)
                {
                    GDObject removed = ChoosenPlants[id];
                    ChoosenPlants.Insert(id - 1, ChoosenPlants[id]);
                    ChoosenPlants.RemoveAt(id);
                }
                break;
            case "rock":
                break;
        }


    }
    public void MoveElementDown(int id)
    {
        switch (UIManager.Get.CurrentPanel.name)
        {
            case "terrain":
                Debug.Log("Removing " + ChoosenTextures[id].ObjectName);
                if (id < ChoosenTextures.Count)
                {
                    GDObject removed = ChoosenTextures[id];
                    ChoosenTextures.Insert(id + 1, ChoosenTextures[id]);
                    ChoosenTextures.RemoveAt(id);
                }
                break;

            case "vegetation":
                Debug.Log("Removing " + ChoosenPlants[id].ObjectName);
                if (id < ChoosenTextures.Count)
                {
                    GDObject removed = ChoosenPlants[id];
                    ChoosenPlants.Insert(id + 1, ChoosenPlants[id]);
                    ChoosenPlants.RemoveAt(id);
                }
                break;
            case "rock":
                break;
        }

    }



    Color32 AverageColorFromTexture(Texture2D tex)
    {
        Color32[] texColors = tex.GetPixels32();

        int total = texColors.Length;

        float r = 0;
        float g = 0;
        float b = 0;

        for (int i = 0; i < total; i++)
        {
            r += texColors[i].r;
            g += texColors[i].g;
            b += texColors[i].b;
        }
        return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 255);
    }
}
