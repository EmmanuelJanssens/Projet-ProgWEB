using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookCamera : MonoBehaviour
{
    float xrot;
    float yrot;

	// Update is called once per frame
	void Update ()
    {
        //Get the mouse coordinates
        xrot += Input.GetAxis("Mouse X");
        yrot -= Input.GetAxis("Mouse Y");

        //Dont move the camera arround when the left controll key is pressed
        if(!Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.W))
            { 
                transform.position += transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position -= transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right;
            }

            //Rotate the camera with the mouse coordinates
            transform.eulerAngles = new Vector3(yrot, xrot, 0);
        }


    }
}
