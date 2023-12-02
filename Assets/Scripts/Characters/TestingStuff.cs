using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;


public class Kinematic
{
	[field: SerializeField] public Vector3 Position { get; set; }
	[field: SerializeField] public float Orientation { get; set; }
	[field: SerializeField] public Vector3 Velocity { get; set; }
	[field: SerializeField] public float Rotation { get; set; }
}

public class SteeringOutput
{
	public Vector3 Linear { get; set; }
	public float Angular { get; set; }
	public Vector3 Velocity { get; set; } = default;
}


public class TestingStuff : MonoBehaviour
{
	[field: SerializeField] public float MaxAcceleration { get; private set; } = 5f;
	[field: SerializeField] public GameObject Target { get; private set; }
	[field: SerializeField] public List<GameObject> TargetList { get; private set; }
	[field: SerializeField] public float Threshold { get; private set; } = 1.5f;
	[field: SerializeField] public float DecayCoefficient { get; private set; } = 1f;
	[field: SerializeField] public Rigidbody Rigidbody { get; private set; }
	[field: SerializeField] public float Radius{ get; private set; }
	[field: SerializeField] public float TimeToTarget{ get; private set; }

	private SteeringOutput _steeringOutput;
	private KinematicSeek ks; 
	
	private void Start()
	{
		Rigidbody = GetComponent<Rigidbody>();
		_steeringOutput = new();
		ks = new(){character = gameObject, maxSpeed = MaxAcceleration, target = Target};
	}
	private void FixedUpdate()
	{
		//ks.GetSteering(Rigidbody); 
		//SeperationBehavior();
		KinematicArrive();
	}
	private void KinematicArrive()
	{
		Rigidbody.velocity = Target.transform.position - transform.position;
		if(Rigidbody.velocity.magnitude < Radius)
		{
			return; 	
		}
		Rigidbody.velocity /= TimeToTarget;
		
		if(Rigidbody.velocity.magnitude > MaxAcceleration)
		{
			Rigidbody.velocity.Normalize();
			Rigidbody.velocity *= MaxAcceleration;
		}
		
		Rigidbody.rotation = Quaternion.Slerp(Rigidbody.rotation, 
			Quaternion.LookRotation(Rigidbody.velocity), Time.deltaTime * 10f);

		//Rigidbody.rotation = Quaternion.identity;
		
	}
	private void SteeringMoveBehavior()
	{
		/*
		transform.position += _steeringOutput.Velocity * Time.deltaTime;
		_steeringOutput.Velocity = _steeringOutput.Linear * Time.deltaTime;
		if(_steeringOutput.Velocity.magnitude > MaxAcceleration)
		
		{
			_steeringOutput.Velocity.Normalize();
			_steeringOutput.Velocity *= MaxAcceleration;
		}
		
		_steeringOutput.Linear = transform.position - Target.transform.position;
		_steeringOutput.Linear.Normalize();
		_steeringOutput.Linear *= MaxAcceleration;
		

		_steeringOutput.Angular = 0;
		*/
	}
	private void SeperationBehavior()
	{
		Vector3 direction;
		float strength;
		float distance;
		Vector3 linear = default;
		float min = 0;
		//Vector3 previousPosition = default;
		foreach (var target in TargetList)
		{
			direction = target.transform.position - transform.position;
			distance = direction.magnitude;
			//direction.Normalize();
			if (distance < Threshold)
			{
				// strength = Vector3.Min(DecayCoefficient / (distance * distance), MaxAcceleration);
				
				strength = MaxAcceleration * (Threshold - distance) / Threshold;
				direction.Normalize();
				//direction *= MaxAcceleration;


				//_steeringOutput.Linear += strength * direction;
				//_steeringOutput.Linear = (transform.position - previousPosition) / Time.deltaTime;
				//_steeringOutput.Linear += strength * direction;
				//Rigidbody.MovePosition(Rigidbody.position + _steeringOutput.Linear);
				//GetComponent<Rigidbody>().AddForce() += strength * direction;
				//transform.position = Vector3.MoveTowards(transform.position, _steeringOutput.Linear, 5f * Time.deltaTime);
				
				
				linear += strength * direction;
				//transform.position += strength * direction * Time.deltaTime;
				 target.transform.Translate(linear * Time.deltaTime, Space.World);
				//Rigidbody.AddForce(linear);
				
				//Rigidbody.AddForce(transform.forward * strength);
				Debug.Log($"Force:{strength * direction}");
			}
		}
		//Debug.Log(direction);
	}
}
