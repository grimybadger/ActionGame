using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

public class Movement : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; set; }
    [field: SerializeField] public float JumpHeight { get; private set; }
    [field: SerializeField] public float Gravity { get; private set; }
    [field: SerializeField] public float AirControl { get; private set; }
    //[field: SerializeField] public Vector3 MoveDirection { get; set; } = Vector3.zero;
    private Vector3 _moveDirection;

    CharacterController _controller;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }
    private void Update()
    {



        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input *= MoveSpeed;
        input = transform.TransformDirection(input);

        if (_controller.isGrounded)
        {
            _moveDirection = input;
            if (Input.GetButton("Jump"))
            {
                _moveDirection.y = Mathf.Sqrt(2 * Gravity * JumpHeight);
            }
            else
            {
                _moveDirection.y = 0;
            }
        }
        else
        {
            input.y = _moveDirection.y;
            _moveDirection = Vector3.Lerp(_moveDirection, input, AirControl * Time.deltaTime);
        }
        _moveDirection.y -= Gravity * Time.deltaTime;
        _controller.Move(_moveDirection * Time.deltaTime);

    }

}
