using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchTab : MonoBehaviour
{


    private TabItem[] _tabs;
    public Toggle[] _tabToggles;

	// Use this for initialization
	void OnEnable ()
    {
        _tabs = gameObject.GetComponentsInChildren<TabItem>(true);

        for(int i = 0; i < _tabToggles.Length; i++)
        {
            int temp = i;
            _tabToggles[i].onValueChanged.AddListener(delegate { Switch(true, temp); });
        }

        for (int i = 0; i < _tabToggles.Length; i++)
        {
            if (_tabs[0].Name == _tabToggles[i].name)
            {
                _tabs[0].gameObject.SetActive(true);
            }
            else
            {
                _tabs[i].gameObject.SetActive(false);
            }
        }
    }
	
    public void Switch(bool on,int index)
    {
        if(_tabToggles[index].isOn)
        {
            _tabs[index].gameObject.SetActive(true);
        }
        else
        {
            _tabs[index].gameObject.SetActive(false);

        }
    }
	// Update is called once per frame
	void Update ()
    {
	    	
	}
}
