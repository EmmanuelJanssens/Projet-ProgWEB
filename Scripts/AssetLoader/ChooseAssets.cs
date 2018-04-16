using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used when the user desires to import new assets in to the editor
/// loads assets from the bundles that were loaded
/// </summary>
public class ChooseAssets : MonoBehaviour
{
    /// <summary>
    /// Names of the bundles that will be used
    /// UI Bundle is the bundle name + _ui
    /// </summary>
    private string _uiBundleName;
    /// <summary>
    /// Bundle name
    /// </summary>
    private string _bundleName;


    /// <summary>
    /// Elements to be displayed in the selection frame
    /// </summary>
    public GameObject ElementAvailable;

    /// <summary>
    /// Elements that will be displayed in the editor
    /// </summary>
    public GameObject ElementSelected;

    /// <summary>
    /// Frame of the propreties from each element
    /// One unique frame updated everytime a different element is selected
    /// </summary>
    public GameObject frmElementPropreties;

    /// <summary>
    /// Command that starts the importation of the selected elements into the editor panel
    /// </summary>
    public Button StartImportation;

    /// <summary>
    /// List of commands to control each selected elements
    /// Move Up,Down, Open propreties
    /// </summary>
    public Button[] cmdOpenPropreties;

    /// <summary>
    /// Used to load the bundles
    /// </summary>
    public BundleLoader Loader;

    /// <summary>
    /// A list of the textures that where selected
    /// </summary>
    public List<GDTexture> ChoosenTextures;

    /// <summary>
    /// A list of the plants that where selected
    /// </summary>
    public List<GDPlant> ChoosenPlants;

    /// <summary>
    /// Manages the list to move elements up and down
    /// </summary>
    private ListManager _listManager;


    // Use this for initialization
    void Start()
    {
        ChoosenTextures = new List<GDTexture>();
        ChoosenPlants = new List<GDPlant>();

        //Exports the elements to the editor panel and destroy  all elements from the display panel 
        StartImportation.onClick.AddListener(
            delegate
            {
                StartCoroutine(ExportAssets());
                //Clean the transform's children for next use
                foreach (Transform child in transform)
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
        //Get the bundles to be loaded
        //The title of the panel is used for this one
        _uiBundleName = UIManager.Get.Title.text.ToLower() + "_ui";
        _bundleName = UIManager.Get.Title.text.ToLower();

        //load the UI Images to be displayed
        yield return Loader.LoadAssync<Sprite>(_uiBundleName);

        //Creates the toggles of the elements
        for (int i = 0; i < Loader.Bundles[_uiBundleName].Assets.Length; i++)
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
        //Get the list manager of the current panel/ terrain,water,plants,...
        _listManager = UIManager.Get.CurrentPanel.GetComponentInChildren<ListManager>();

        //Parent object of all the available elements
        GameObject DisplayList = _listManager.gameObject;

        //Toggle component of all the available elements
        Toggle[] all = gameObject.GetComponentsInChildren<Toggle>();

        //Go trough all the toggles
        for (int i = 0; i < all.Length; i++)
        {
            //if a specific toggle is on this means the element is selected
            //Instantiates a UI prefab of the selected Item 
            //Add data to the list
            if (all[i].isOn)
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

        //Add the listener to the open propreties
        cmdOpenPropreties = null;
        cmdOpenPropreties = _listManager.GetComponentsInChildren<Button>();
        for (int i = 0; i < cmdOpenPropreties.Length; i++)
        {
            if (cmdOpenPropreties[i].name == "Propreties")
            {
                ListElement linked = cmdOpenPropreties[i].GetComponentInParent<ListElement>();
                cmdOpenPropreties[i].onClick.AddListener(delegate { OpenPropreties(linked.ID); });
            }
        }
        //Initialises or updates the list if new items are added
        _listManager.Init();

        //Close the  frame
        UIManager.Get.CloseFrame();
        yield return null;
    }

    public void OpenPropreties(int i)
    {
        switch (UIManager.Get.CurrentPanel.name)
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