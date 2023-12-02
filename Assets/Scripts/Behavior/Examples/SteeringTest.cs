using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicSeek
{
	public GameObject character;
	public GameObject target;
	public float maxSpeed;
	
	public void GetSteering(Rigidbody rigidbody)
	{
		rigidbody.velocity = target.transform.position - character.transform.position;
		//rigidbody.velocity =character.transform.position - target.transform.position;
		rigidbody.velocity.Normalize();
		rigidbody.velocity *= maxSpeed;

		character.transform.rotation = Quaternion.Slerp(character.transform.rotation, 
			Quaternion.LookRotation(rigidbody.velocity), Time.deltaTime * 10f);
	}
	
	
}
