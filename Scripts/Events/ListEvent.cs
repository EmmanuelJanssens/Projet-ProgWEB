using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Delegate system for handling list events
/// Handles all events related to a list
/// Move elements up,down
/// delete elements
/// add elements
/// </summary>
public class ListEvent
{

    public delegate void OnMoveUp(int id);
    public delegate void OnMoveDown(int id);
    public delegate void OnDelete(int ID);

    public delegate void OnDrag();
    public delegate void OnAdd();

    public event OnMoveUp onMoveUp;
    public event OnMoveDown onMoveDown;
    public event OnDelete onDelete;

    public event OnDrag onDrag;
    public event OnAdd onAdd;

    public void MovedUp(int id)
    {
        if (onMoveUp != null)
            onMoveUp(id);
    }

    public void MovedDown(int id)
    {
        if (onMoveDown != null)
            onMoveDown(id);
    }

    public void Delete(int ID)
    {

        if (onDelete != null)
            onDelete(ID);
    }


}
