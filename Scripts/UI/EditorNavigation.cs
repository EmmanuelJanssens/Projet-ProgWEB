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
    public GameObject NoisePanel;
    public GameObject TexturePanel;
    public GameObject OceanPanel;
    public GameObject TreePanel;
    public GameObject PlantPanel;
    public GameObject RockPanel;
    public GameObject RiverPanel;
    public GameObject SimulationPanel;

    //Keep track of the current openPanel
    [HideInInspector] public GameObject ActivePanel;

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
            if (_navButtons[i].name == "Noise")
            {
                _navButtons[i].onClick.AddListener(delegate { UIManager.Get.SwitchPanel(NoisePanel); });
            }
            if(_navButtons[i].name == "Texture")
            {
                _navButtons[i].onClick.AddListener(delegate { UIManager.Get.SwitchPanel(TexturePanel); });
            }
            if(_navButtons[i].name == "Trees")
            {
                _navButtons[i].onClick.AddListener(delegate { UIManager.Get.SwitchPanel(TreePanel); });
            }
            if (_navButtons[i].name == "Water")
            {
                _navButtons[i].onClick.AddListener(delegate { UIManager.Get.SwitchPanel(OceanPanel); });
            }
            if (_navButtons[i].name == "Plants")
            {
                _navButtons[i].onClick.AddListener(delegate { UIManager.Get.SwitchPanel(PlantPanel); });
            }
            if (_navButtons[i].name == "Rocks")
            {
                _navButtons[i].onClick.AddListener(delegate { UIManager.Get.SwitchPanel(RockPanel); });
            }
            if (_navButtons[i].name == "Rivers")
            {
                _navButtons[i].onClick.AddListener(delegate { UIManager.Get.SwitchPanel(RiverPanel); });
            }
            if(_navButtons[i].name == "Simulation")
            {
                _navButtons[i].onClick.AddListener(delegate { UIManager.Get.SwitchPanel(SimulationPanel); });
            }
        }
    }



	
}
