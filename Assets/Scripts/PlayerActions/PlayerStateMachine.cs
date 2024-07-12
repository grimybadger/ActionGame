using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private Movement _movement;
	public List<string> _states;
	public List<string> _extraMovements;
	public List<string> _attacks;

	private List<KeyCode> _movementKeyCodes = new(){
		KeyCode.W, KeyCode.D, KeyCode.S, KeyCode.A
	};
	private Dictionary<KeyCode, KeyCode> _movementExtra = new()
	{
		{ KeyCode.W, KeyCode.LeftShift }
	};

	private Dictionary<KeyCode, string> _movements = new();
	private float pressTimeStamp;
	private float previousPressTime = 2f;
	public bool isChainAttacking;
	public bool isChainAtTwo;
	public bool isChainAtOne;
	public bool isAttacking;
	
	private void Start()
	{
		_movements.Add(KeyCode.W, "isWalking");
		_movements.Add(KeyCode.D, "isStrafeRight");
		_movements.Add(KeyCode.S, "isBackwards");
		_movements.Add(KeyCode.A, "isStrafeLeft");
		// _movements.Add(KeyCode.LeftShift, "isRunning");   
	}

	private void LateUpdate()
	{

		if (isChainAttacking)
		{
			//previousPressTime = 2f;
			previousPressTime -= Time.deltaTime;
			//Debug.Log(previousPressTime);

			if (previousPressTime <= 0)
			{
				isChainAttacking = false;
				previousPressTime = 2f;
				_animator.SetBool("isChain", false);
				isAttacking = false;
			}

		}

		if (Input.GetMouseButtonDown(0))
		{
			// timeAtpress = Time.deltaTime;
			// _mchainAttack = true;
			// float timeElasped = previousPressTime - timeAtpress)
			//_animator.Play(_attacks[0]);
			isAttacking = true;
			_movement.MoveSpeed = 0f;
			if (isChainAttacking)
			{
				
				if(isChainAtOne)
				{
				   // _animator.SetBool("isChainTwo", true);
					_animator.Play(_attacks[2]);
				}
			   
					// _animator.SetBool("isChain", _chainAtOne = true);
					isChainAtOne = true;
					_animator.Play(_attacks[1]);
				  
			   
			}
			else
			{
				_animator.Play(_attacks[0]);
				_animator.SetBool("isChain", false);
				_animator.SetBool("isChainTwo", false);
				isChainAttacking = true;
				isChainAtOne = false;
				isChainAtTwo = false;
			}

			// _animator.Play(_attacks[1]);


			// _animator.SetBool("isChain", false);
			// timeAtpress = 0;
			//  previousPressTime = timeAtpress;

		}

		foreach (KeyValuePair<KeyCode, string> item in _movements)
		{
			if (isAttacking)
			{
				_animator.SetBool("isRunning", false);
				_animator.SetBool("isWalking", false);
				_animator.SetBool("isStrafeRight", false);
				_animator.SetBool("isBackwards", false);
				_animator.SetBool("isStrafeLeft", false);
				return;
			}
			if (Input.GetKey(item.Key))
			{

				if (Input.GetKey(KeyCode.LeftShift))
				{

					_animator.SetBool(item.Value, true);
					_animator.SetBool("isLeftShift", true);

					//_movement.MoveSpeed = 15f;
					return;
				}

				_animator.SetBool(item.Value, true);
				_animator.SetBool("isLeftShift", false);
				//_movement.MoveSpeed = 3f;

			}
			else
			{
				_animator.SetBool(item.Value, false);
				_animator.SetBool("isLeftShift", false);
			}

		}
		// _animator.Play($"{_states[0]}");
		/*
		  for(int i = 0; i < _movements.Count; i ++)
	   {
			//KeyCode keycode = _movementKeyCodes[i];
			//if(Input.GetKey(_movementKeyCodes[i]) && Input.GetKey(_movementExtra[keycode]))
			if(Input.GetKey(_movements.Keys.))
			{
			  //    _animator.Play($"{_extraMovements[i] }");
			//}
			//else if(Input.GetKey(_movementKeyCodes[i]))
			//{
				 _animator.Play($"{_states[i] }");
			}
	   }*/
	   
		/*
	   for(int i = 0; i < _movementKeyCodes.Count; i ++)
	   {
			KeyCode keycode = _movementKeyCodes[i];
			//if(Input.GetKey(_movementKeyCodes[i]) && Input.GetKey(_movementExtra[keycode]))
			if(Input.GetKey(_movementKeyCodes[i]))
			{
			  //    _animator.Play($"{_extraMovements[i] }");
			//}
			//else if(Input.GetKey(_movementKeyCodes[i]))
			//{
				 _animator.Play($"{_states[i] }");
			}
	   }*/

		// _animator.Play($"{_states[0]}");

		/*
		if(Input.GetKey(KeyCode.W))
		{
			// _animator.SetBool("isTrue", true);
			_animator.Play($"{_states[1]}");
		}
		else 
		{
			_animator.Play($"{_states[0]}");
			//_animator.SetBool("isTrue", false);
		}
		//  _animator.
		//_animator.GetBehaviours();
	  */
	}


}
