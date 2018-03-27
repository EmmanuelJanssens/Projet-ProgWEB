using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages User input 
/// 
/// </summary>
public class UserInput : PlayerController
{

    // Use this for initialization
    public override void OnStart()
    {
        base.OnStart();
    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_controler.isGrounded)
        {
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= _CurrentSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                _moveDirection.y = _JumpForce;
            }

            if (Input.GetKey(KeyCode.LeftShift))
                Sprint(true);
            if (Input.GetKeyUp(KeyCode.LeftShift))
                Sprint(false);
        }

        _moveDirection.y += Physics.gravity.y * Time.deltaTime;
        _controler.Move(_moveDirection * Time.deltaTime);

     
    }



    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

    }
}