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


    public static UIManager current;

    [HideInInspector]
    public GameObject PreviousPanel;

    public GameObject CurrentPanel;

    public GameObject DarkOverlay;


    public GameObject PreviousFrame;
    public GameObject CurrentFrame;

    public Text Title;


    public Toggle ShowTerrain;
    public Toggle ShowNoiseMap;
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

    public void EnableTerrain(bool b)
    {
        AppManager.Get.SplatMap.terrain.gameObject.SetActive(b);
        AppManager.Get.SplatMap.terrain.gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 100;
    }
    public void EnableNoiseMap(bool b)
    {
        AppManager.Get.NoiseMap.NoiseImage.gameObject.SetActive(b);
    }
    public void EnableFreeLook(bool b)
    {
        Camera.main.gameObject.GetComponent<FreeLookCamera>().enabled = b;
    }

    public void SwitchPanel(GameObject panelToOpen)
    {
        PreviousPanel = CurrentPanel;
        PreviousPanel.SetActive(false);

        CurrentPanel = panelToOpen;
        CurrentPanel.SetActive(true);

        Title.text = CurrentPanel.name[0].ToString().ToUpper() + CurrentPanel.name.Substring(1);
    }

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

    public void goToPreviousPanel()
    {

        CurrentPanel.SetActive(false);

        CurrentPanel = PreviousPanel;
        CurrentPanel.SetActive(true);

    }

}
