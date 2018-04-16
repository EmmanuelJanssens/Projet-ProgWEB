using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// manages UI Navigation
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Game object that is the frame selection
    /// Display of toggles
    /// </summary>
    public GameObject SelectFrame;

    /// <summary>
    /// Current UIManager instance
    /// </summary>
    public static UIManager current;

    /// <summary>
    /// The panel where the user was before
    /// Usualy the main editor panel
    /// </summary>
    [HideInInspector]
    public GameObject PreviousPanel;

    /// <summary>
    /// The panel in wich the user is now
    /// </summary>
    public GameObject CurrentPanel;

    /// <summary>
    /// overlay that is displayed when a frame is opened
    /// </summary>
    public GameObject DarkOverlay;

    /// <summary>
    /// The last frame that was opened
    /// </summary>
    public GameObject PreviousFrame;

    /// <summary>
    /// The current frame that is openend
    /// </summary>
    public GameObject CurrentFrame;

    //what panel is used
    public string wichPanel;

    /// <summary>
    /// Title of the current panel
    /// </summary>
    public Text Title;

    /// <summary>
    /// Toggle used to display the terrain part of groupbox parent
    /// </summary>
    public Toggle ShowTerrain;

    /// <summary>
    /// Toggle used to display the noise map part of groupbox parent
    /// </summary>
    public Toggle ShowNoiseMap;

    /// <summary>
    /// Activates or deactivates the free  look mode
    /// </summary>
    public Toggle FreeLook;


    public static UIManager Get
    {
        get { return current; }
    }

    public UIManager()
    {
        if (current == null)
            current = this;
    }

    public void Start()
    {
        ShowTerrain.onValueChanged.AddListener(EnableTerrain);
        ShowNoiseMap.onValueChanged.AddListener(EnableNoiseMap);
        FreeLook.onValueChanged.AddListener(EnableFreeLook);
    }

    /// <summary>
    /// Enables the terrain
    /// </summary>
    /// <param name="b">value of the toggle</param>
    public void EnableTerrain(bool b)
    {
        AppManager.Get.SplatMap.terrain.gameObject.SetActive(b);
        AppManager.Get.SplatMap.terrain.gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 100;
    }
    /// <summary>
    /// Enables the noise map
    /// </summary>
    /// <param name="b">value of the toggle</param>
    public void EnableNoiseMap(bool b)
    {
        AppManager.Get.NoiseMap.NoiseRenderer.gameObject.SetActive(b);
    }

    /// <summary>
    /// Enables the freellok
    /// </summary>
    /// <param name="b">value of the toggle</param>
    public void EnableFreeLook(bool b)
    {
        Camera.main.gameObject.GetComponent<FreeLookCamera>().enabled = b;
    }

    /// <summary>
    /// Switch between panels
    /// </summary>
    /// <param name="panelToOpen">Wich panel to be activated</param>
    public void SwitchPanel(GameObject panelToOpen)
    {
        PreviousPanel = CurrentPanel;
        PreviousPanel.SetActive(false);

        CurrentPanel = panelToOpen;
        wichPanel = CurrentPanel.name;
        CurrentPanel.SetActive(true);

        Title.text = CurrentPanel.name[0].ToString().ToUpper() + CurrentPanel.name.Substring(1);
    }

    /// <summary>
    /// Opens a specific frame
    /// </summary>
    /// <param name="toOpen">frame to be opened</param>
    public void OpenFrame(GameObject toOpen)
    {
        Button[] deactivate;
        deactivate = CurrentPanel.GetComponentsInChildren<Button>();

        for(int i = 0; i < deactivate.Length; i++)
        {
            deactivate[i].interactable = false;
        }

        DarkOverlay.SetActive(true);

        CurrentFrame = toOpen;
        CurrentFrame.SetActive(true);

        
    }

    /// <summary>
    /// Closes the curent open frame
    /// </summary>
    public void CloseFrame()
    {
        Button[] deactivate;
        deactivate = CurrentPanel.GetComponentsInChildren<Button>(true);

        for (int i = 0; i < deactivate.Length; i++)
        {
            deactivate[i].interactable = true;
        }

        DarkOverlay.SetActive(false);
        CurrentFrame.SetActive(false);

    }

    /// <summary>
    /// Switches to the previous panel
    /// </summary>
    public void goToPreviousPanel()
    {

        CurrentPanel.SetActive(false);

        CurrentPanel = PreviousPanel;
        CurrentPanel.SetActive(true);

    }

}
