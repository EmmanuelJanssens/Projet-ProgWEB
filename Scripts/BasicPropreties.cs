using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicPropreties : MonoBehaviour {

    public Button cmdApply;
    public Button cmdCancel;

    public SwitchTab Tabs;

    //General influence
    public InputField Influence;

    //For single Values
    protected InputField Value;

    //For ranged Values
    protected InputField MinValue;
    protected InputField MaxValue;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
