using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Handles navigation into the main editor
/// Wich button open wich panel
/// 
/// when a panel is opened the main editor panel is hidden/deactivated until we close the opened panel
/// </summary>
public class EditorNavigation : MonoBehaviour
{

    //There will be only one panel open at a time
    public static EditorNavigation current;

    //List of active buttons
    private Button[] _navButtons;

    //GameObject of the terrainpanel in the hierarchy
    public GameObject TerrainPanel;


    //Keep track of the current openPanel
    [HideInInspector] public GameObject ActivePanel;

    //
    public Text Title;


    public GameObject GONoiseMap;
    public GameObject GOTerrain;

	// Use this for initialization
	void Start ()
    {
        if (current == null)
            current = this;

        _navButtons = gameObject.GetComponentsInChildren<Button>();

        AddListeners();
	}
	
    /// <summary>
    /// Get the instance of the editorPanel
    /// </summary>
    public static EditorNavigation Get { get { return current; } }

    /// <summary>
    /// Add the event listeners to the navigation buttons
    /// </summary>
    void AddListeners()
    {
        for(int i = 0; i < _navButtons.Length; i++)
        {
            if (_navButtons[i].name == "Terrain")
            {
                _navButtons[i].onClick.AddListener(delegate { Open(TerrainPanel); });
                Title.text = "Terrain";
            }
        }
    }

    /// <summary>
    /// Opens a panel
    /// </summary>
    /// <param name="panel">Wich panel to Open</param>
    public void Open(GameObject panel)
    {

        panel.SetActive(true);
        ActivePanel = panel;

        //Deactivate the mainPanel
        gameObject.SetActive(false);
    }


	
}
