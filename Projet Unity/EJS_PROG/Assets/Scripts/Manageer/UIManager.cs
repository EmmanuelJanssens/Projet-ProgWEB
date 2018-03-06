using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// manages UI Navigation
/// </summary>
public class UIManager : MonoBehaviour
{

    public static UIManager current;

    [HideInInspector]
    public GameObject PreviousPanel;

    public GameObject CurrentPanel;

    public GameObject DarkOverlay;


    public GameObject PreviousFrame;
    public GameObject CurrentFrame;

    public Text Title;

    public static UIManager Get
    {
        get { return current; }
    }

    public UIManager()
    {
        if (current == null)
            current = this;
    }

    public void SwitchPanel(GameObject panelToOpen)
    {
        PreviousPanel = CurrentPanel;
        PreviousPanel.SetActive(false);

        CurrentPanel = panelToOpen;
        CurrentPanel.SetActive(true);

        Title.text = CurrentPanel.name;
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
