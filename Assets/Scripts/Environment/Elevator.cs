using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	[field: Header("General Properties")]
	[field: SerializeField] public GameObject Player { get; set; }
	[field: SerializeField] public Vector3 TopPos { get; set; }
	[field: SerializeField] public Vector3 BottomPos { get; set; }
	[field: SerializeField] public float MoveSpeed { get; set; }
	[field: SerializeField] public bool IsMoving { get; set; }
	[field: SerializeField] public bool isAtTop { get; set; } = true;
	[field: SerializeField] public Vector3 Velocity { get; private set; }


	private void OnTriggerEnter(Collider collider)
	{
		if (IsMoving) return;
		if (collider.GetComponent<CharacterController>() == Player.GetComponent<CharacterController>())
		{
			Debug.Log("We ON");
			IsMoving = true;
			StartCoroutine(Move());
		}
	}
	/*
 	private void OnCollisionEnter(Collision other) 
	{
		if(other.gameObject == Player)
		{
			Debug.Log("We ON");
			IsMoving = true;
				//Player.transform.SetParent(this.transform.parent); 
			StartCoroutine(Move());
			//Player.transform.SetParent(this.transform.parent); 
		}	
	}*/
	/*
	private void OnTriggerExit(Collider other) {
		
		if (other.gameObject == Player)
		{StopAllCoroutines();
		Debug.Log("We Off");
		}
	}*/

	


	private IEnumerator Move()
	{
		Vector3 pos = GetPos();
		//Player.transform.SetParent(this.transform);
		//Vector3 playerPosY;
		//Rigidbody rb = gameObject.GetComponent<Rigidbody>();
		Vector3 direction;
		float distance;
		//CharacterController cc = Player.GetComponent<CharacterController>();
		//float verticalSpeed = cc.velocity.y;
		//Vector3 horizontalVelocity = cc.velocity;
		//horizontalVelocity = new Vector3(cc.velocity.x, 0, cc.velocity.z);
		Vector3 newPos;
		var playerMovement = Player.GetComponent<Movement>();
		Animator animator = Player.GetComponent<Animator>();
		
		yield return new WaitForSeconds(1.5f); 
		while (IsMoving)
		{
			Debug.Log("We Moving");

			//playerPosY = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
			//Player.transform.SetParent(this.transform.parent); 
			//Player.gameObject.transform.position = playerPosY;
			//Player.transform.position += rb.velocity * Time.deltaTime;

			//cc.velocity = new Vector3()

			var step = MoveSpeed * Time.deltaTime;
			//transform.position = Vector3.MoveTowards(transform.position, pos, step);
			newPos = Vector3.MoveTowards(transform.position, GetPos(), step * Time.deltaTime);
			if (Vector3.Distance(newPos, transform.position) < 0.001f)
				transform.position = newPos;

			Velocity = newPos - transform.position / Time.deltaTime;
			transform.position = newPos;
			//Player.GetComponent<Animator>().applyRootMotion = false;
			//transform.position = newPos;
			//cc.move
			/*
			Physics.ComputePenetration(Player.GetComponent<CharacterController>(),
			Player.transform.position, Player.transform.rotation,
			gameObject.GetComponent<Collider>(), gameObject.transform.position, gameObject.transform.rotation, out direction, out distance);
			direction.y = 0;
			Player.transform.position += direction * distance;
			*/
			//Player.transform.position += Velocity * Time.deltaTime;
			//cc.gameObject.transform.position = new Vector3(0,0,0);
			//transform.position = newPos; 
			//Player.gameObject.transform.position = playerPosY;



			if(!isAtTop)
			{
				playerMovement._moveDirection.y = MoveSpeed  * Time.fixedDeltaTime;
				animator.applyRootMotion = false;
				playerMovement.MoveSpeed = 2; 	 
				playerMovement.isOnPlatform = true; // Going up
			}

			//Player.transform.position += Velocity * Time.fixedDeltaTime;
			//Player.transform.position = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z); 
			//Player.transform.SetParent(this.transform.parent); 
			if (transform.position == pos) IsMoving = false;
			yield return new WaitForFixedUpdate();
			//yield return null;
		}
		//Player.transform.SetParent(null); 
		//isAtTop = false;
		playerMovement.isOnPlatform = false;
		animator.applyRootMotion = true;
		if(transform.position == BottomPos)
		{
			isAtTop = false;			
		}
		else
		{
			isAtTop = true;
		}
		//Player.GetComponent<Animator>().applyRootMotion = true;
		Debug.Log("Elevator is done moving");
	}
	
	

	private Vector3 GetPos() => isAtTop ? BottomPos : TopPos;
	/*
	private Vector3 GetPos(){

		//Vector3 pos;
		//return isAtTop ? TopPos : BottomPos;
		
		if(isAtTop)
		{
			isAtTop = false;
			return BottomPos; 
		}
		isAtTop = true;
		return TopPos; 
	}*/
}
