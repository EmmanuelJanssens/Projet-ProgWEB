using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages a list of objects
/// move them up and down
/// remove them
/// </summary>
[RequireComponent(typeof(GridLayoutGroup))]
public class ListManager : MonoBehaviour
{
    //List of elements as children from the attached game object
    [SerializeField]
    private List<ListElement> _elements;

    //Accessible list 
    public List<ListElement> Elements { get { return _elements; } set { _elements = value; } }

    public ListEvent onEvent;

    public void Start()
    {
        onEvent = new ListEvent();
    }
    // Use this for initialization
    public void Init ()
    {

        _elements = new List<ListElement>();

        for (int i = 0; i < gameObject.GetComponentsInChildren<ListElement>().Length; i++)
        {
            int temp = i;
            _elements.Add(gameObject.GetComponentsInChildren<ListElement>()[i]);
            _elements[i].ID = i;
            _elements[i].Init();

        }
        onEvent.onDelete += Remove;
        onEvent.onMoveDown += MoveDown;
        onEvent.onMoveUp += MoveUp;
    }


    /*
     *  \\Basic procedure to move an object up or down
     *  
     *  Remove the element from the list at his specified index
     *  Insert the element in to the list at index +1 for up and -1 for down
     */

    /// <summary>
    /// Move an element up in the list
    /// </summary>
    /// <param name="index">Index of the element to be moved</param>
    public virtual void MoveUp(int index)
    {
        _elements[index].gameObject.transform.SetSiblingIndex(index - 1);

        Init();
    }

    /// <summary>
    /// Moves an element down in the list
    /// </summary>
    /// <param name="index">Index of the element to be moved</param>
    public virtual void MoveDown(int index)
    {
        _elements[index].gameObject.transform.SetSiblingIndex(index + 1);

        Init();
    }
	
    /// <summary>
    /// Updates Identifers list for sorting
    /// </summary>
    public virtual void UpdateIdentifiers()
    {
        for(int i = 0; i < _elements.Count; i++)
        {
            _elements[i].ID = i;
            _elements[i].Init();
        }
    }


    /// <summary>
    /// Removes an element from the hierarchy 
    /// </summary>
    /// <param name="index"></param>
    public virtual void Remove(int index)
    {
        Debug.Log(index);
        Destroy(_elements[index].gameObject);
        Elements.RemoveAt(index);
        Elements.Clear();

        Init();
    }
}
