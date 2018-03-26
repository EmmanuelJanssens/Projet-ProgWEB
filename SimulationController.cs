using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationController : MonoBehaviour
{
    public Vector3 StartPosition;

    public Button StartSimulation;

    public GameObject Player;

    public Camera MainCamera;
    public Camera SimCamera;

    public void Start()
    {
        StartSimulation.onClick.AddListener(OnSimStart);
    }

    public void OnPositionSelect(Vector3 select)
    {

    }
    public void OnSimStart()
    {
        Player.transform.position = StartPosition;
        Player.SetActive(true);

        MainCamera.gameObject.SetActive(false);
        SimCamera.gameObject.SetActive(true);
    }


    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                StartPosition = hit.transform.position;
            }
        }
    }

}
