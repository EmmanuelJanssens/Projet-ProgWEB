using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Gui inerface script
/// That handles switching between an amount of tabs
/// </summary>
public class SwitchTab : MonoBehaviour
{

    [SerializeField]
    private TabItem[] _tabs;

    public Toggle[] _tabToggles;

    public TabItem ActiveTab;

    public TabItem[] GetTabs { get { return _tabs; } }
	// Use this for initialization
	void Start ()
    {
        _tabs = gameObject.GetComponentsInChildren<TabItem>(true);

        for(int i = 0; i < _tabToggles.Length; i++)
        {
            int temp = i;
            _tabToggles[i].onValueChanged.AddListener(delegate { Switch(true, temp); });
        }

        //Default tab is always the first tab
        for (int i = 0; i < _tabToggles.Length; i++)
        {
            if (_tabs[0].Name == _tabToggles[i].name)
            {
                _tabs[0].gameObject.SetActive(true);
                _tabToggles[i].isOn = true;
                ActiveTab = _tabs[0];
            }
            else
            {
                _tabs[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set a specific tab active via its name
    /// </summary>
    /// <param name="tabName"></param>
	public void SetActive( string tabName)
    {
        for(int i = 0; i < _tabs.Length; i++)
        {
            if(_tabs[i].Name == tabName)
            {
                _tabs[i].gameObject.SetActive(true);
                _tabToggles[i].isOn = true;
                ActiveTab = _tabs[i];
            }
            else
            {
                _tabToggles[i].isOn = false;
                _tabs[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Listener method for the tab toggles
    /// </summary>
    /// <param name="on">Must have to be able to add to the toggle listener</param>
    /// <param name="index">Index of the tab that has to be switchd</param>
    public void Switch(bool on,int index)
    {
        if(_tabToggles[index].isOn)
        {
            _tabs[index].gameObject.SetActive(true);
            ActiveTab = _tabs[index];
        }
        else
        {
            _tabs[index].gameObject.SetActive(false);
        }
    }

}
