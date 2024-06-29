using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlatformRiding : MonoBehaviour
{

	CharacterController controller;
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		var capsulePoint1 = transform.position + new Vector3(0, (controller.height / 2) - controller.radius, 0);
		var capsulePoint2 = transform.position - new Vector3(0, (controller.height / 2) + controller.radius, 0);
		Collider[] overLappingColliders = new Collider[10]; //if we encounter more colliders make note here to take this into account

		var overlapCount = Physics.OverlapCapsuleNonAlloc(
			capsulePoint1, capsulePoint2, controller.radius, overLappingColliders
		);
		
		for (int i = 0; i < overlapCount; i ++)
		{
			var overlappingCollider = overLappingColliders[i];
			if (overlappingCollider == controller)
			{
				continue;
			}
				
			Vector3 direction;
			float distance;
			Physics.ComputePenetration(
				controller,
				transform.position,
				transform.rotation,
				overlappingCollider,
				overlappingCollider.transform.position,
				overlappingCollider.transform.rotation,
				out direction,
				out distance
			);
			direction.y = 0;
		 	//Debug.Log($"Distance:{distance}, Direction:{direction}");
			transform.position += direction * distance;	
		}
		var ray = new Ray(transform.position, Vector3.down);
		RaycastHit hit;

		float maxDistance = (controller.height / 2f) + 0.1f; 
		
		if(Physics.Raycast(ray, out hit, maxDistance))
		{
			var platform = hit.collider.gameObject.GetComponent<Elevator>();
			
			if(platform != null)
			{
				transform.position += platform.Velocity * Time.deltaTime;
			}
		}
	}
}
