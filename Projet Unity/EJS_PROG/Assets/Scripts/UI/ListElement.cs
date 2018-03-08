using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///Data container for an element from a basic list
/// </summary>
public class ListElement : MonoBehaviour
{

    /// <summary>
    /// List element identifier
    /// </summary>
    public int ID;

    //Commands to move them up or down
    public Button cmdUp;
    public Button cmdDown;

    //Command to remove the element from a list
    public Button cmdRemove;


    /// <summary>
    /// Manager for this specific list element
    /// </summary>
    public ListManager manager;


    public virtual void Init()
    {
        //List of alll button children
        Button[] cmds = gameObject.GetComponentsInChildren<Button>();

        manager = gameObject.GetComponentInParent<ListManager>();

        //Game objects have to be named like the the string
        for (int i = 0; i < cmds.Length; i++)
        {
            
            if (cmds[i].name == "Up")
            {
                cmdUp = cmds[i];
                cmdUp.onClick.RemoveAllListeners();
                cmdUp.onClick.AddListener(delegate { manager.MoveUp(ID); });
            }
            else if (cmds[i].name == "Down")
            {
                cmdDown = cmds[i];
                cmdDown.onClick.RemoveAllListeners();
                cmdDown.onClick.AddListener(delegate { manager.MoveDown(ID); });
            }
            else if (cmds[i].name == "Remove")
            {
                cmdRemove = cmds[i];
                cmdRemove.onClick.RemoveAllListeners();
                cmdRemove.onClick.AddListener(delegate { manager.Remove(ID); });
            }
            else
            {
            }
        }     
    }
}
