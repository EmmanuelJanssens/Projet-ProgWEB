using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Used to move an object arround into the world
/// </summary>
public class PlayerController : MonoBehaviour
{
    protected Rigidbody _rb;

    [SerializeField]
    protected float _JumpForce = 3f;
    [SerializeField]
    protected float _BaseSpeed = 1f;
    [SerializeField]
    protected float _SprintSpeedBonus = 2f;
    [SerializeField]
    protected float _CrouchSpeedMalus = .4f;

    protected float _CurrentSpeed;
    protected bool _isSprinting = false;


    protected CharacterController _controler;

    protected Vector3 _moveDirection;

    public virtual void OnStart()
    {
        //Gets the attached rigidbody
        _rb = gameObject.GetComponent<Rigidbody>();

        //Set the current speed to the base speed
        _CurrentSpeed = _BaseSpeed;

        _controler = gameObject.GetComponent<CharacterController>();
    }
    // Initialize components
    public void Start() { OnStart(); }


    public virtual void OnUpdate()
    { }

    public void Update() { OnUpdate(); }

    public virtual void OnFixedUpdate()
    { }
    public void FixedUpdate() { OnFixedUpdate(); }

    public virtual void OnDestroy() { }


    /// <summary>
    /// Makes the player move faster
    /// </summary>
    /// <param name="sprinting">Determines if the player started sprinting</param>
    public void Sprint(bool sprinting)
    {
        if (sprinting)
            _CurrentSpeed = _BaseSpeed + _SprintSpeedBonus;
        else
            _CurrentSpeed = _BaseSpeed;
    }

    /// <summary>
    /// Aplies a force to the rigidbody to make im go upwards
    /// </summary>
    public void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, _JumpForce, _rb.velocity.z);
    }

}