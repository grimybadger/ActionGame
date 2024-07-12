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
	[field: SerializeField] public float Distance { get; private set; }
	[field: SerializeField] public GameObject OtherGameObject { get; private set; } = default;
	[field: SerializeField] public bool isGoingUp { get; set; }
	[field: SerializeField] public float YDirection { get; set; }
	[field: SerializeField] public bool isOnPlatform { get; set; }
	//[field: SerializeField] public Vector3 MoveDirection { get; set; } = Vector3.zero;
	public Vector3 _moveDirection = Vector3.zero;

	CharacterController _controller;

	private void Start()
	{
		_controller = GetComponent<CharacterController>();
	}
	/*
	private void Update()
	{
		Distance = Vector3.Distance(transform.position, OtherGameObject.transform.position);
	}*/

	private void FixedUpdate()
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
				Debug.Log("jumping");
			}
			else
			{
				//_moveDirection.y = YDirection;
				_moveDirection.y = 0;
			}
			Debug.Log("Grounded");
		}
		else
		{
			input.y = _moveDirection.y;
			_moveDirection = Vector3.Lerp(_moveDirection, input, AirControl * Time.deltaTime);
		}

		if (!isOnPlatform)// And going up
		{
			_moveDirection.y -= Gravity * Time.fixedDeltaTime;
			MoveSpeed = 0;
		}
		_controller.Move(_moveDirection * Time.fixedDeltaTime);


	}

}

