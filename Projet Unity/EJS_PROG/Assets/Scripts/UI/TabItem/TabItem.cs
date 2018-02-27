using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabItem : MonoBehaviour {

    public string Name;

    public void Start()
    {
        Name = gameObject.transform.parent.name;
    }
}
