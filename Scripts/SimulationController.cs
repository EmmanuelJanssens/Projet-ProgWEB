﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SimulationController : MonoBehaviour
{
    public Vector3 StartPosition;
    public Button StartSimulation;
    public GameObject Player;
    public Camera MainCamera;


    public GameObject EditorCanvas;

    public void Start()
    {
        StartSimulation.onClick.AddListener(OnSimStart);
    }


    public void OnSimStart()
    {
        Player.transform.position = StartPosition;
        Player.SetActive(true);

        MainCamera.GetComponent<ThirdPersonCamera>().enabled = true;
        MainCamera.GetComponent<FreeLookCamera>().enabled = false;

        EditorCanvas.SetActive(false);
    }


    public void Update()
    {
        string title = UIManager.Get.Title.text.ToLower();
        if(title == "simulation")
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    StartPosition = hit.point;
                    Player.SetActive(true);
                    Player.transform.position = StartPosition;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MainCamera.GetComponent<ThirdPersonCamera>().enabled = false;
                MainCamera.GetComponent<FreeLookCamera>().enabled = false;
                Player.SetActive(false);

                EditorCanvas.SetActive(true);
            }

            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit))
                {
                    StartPosition = hit.point;
                    Player.SetActive(true);
                    Player.transform.position = StartPosition;
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                MainCamera.GetComponent<ThirdPersonCamera>().enabled = false;
                MainCamera.GetComponent<FreeLookCamera>().enabled = false;
                Player.SetActive(false);

                EditorCanvas.SetActive(true);
            }
        }
    }

}