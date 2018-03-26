using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform _cameraTarget;

    public float _RotateSpeed = 10f;

    [SerializeField]
    private float _maxDistance;
    [SerializeField]
    private float _minDistance;

    private float _xAngle;
    private float _yAngle;


    private Vector3 _offset;

    // Use this for initialization
    void Start()
    {
        _cameraTarget = GameObject.Find("Player").transform;

        _offset = new Vector3(0, -_minDistance, 1f);
    }

    // Update is called once per frame
    void LateUpdate()
    {


        // Read the user input
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        _cameraTarget.transform.rotation = Quaternion.Euler(-_yAngle, _xAngle, 0);

        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
        _xAngle += x * _RotateSpeed;
        _yAngle += y * _RotateSpeed;

        // Rotate the rig (the root object) around Y axis only:
        Quaternion rotation = Quaternion.Euler(-_yAngle, _xAngle, 0);
        transform.position = _cameraTarget.transform.position - (rotation * _offset);

        gameObject.transform.LookAt(_cameraTarget);

    }


    /// <summary>
    /// Changes the target where the camera looks at
    /// </summary>
    /// <param name="t"></param>
    public void SetTarget(Transform t)
    {
        _cameraTarget = t;
    }
}
