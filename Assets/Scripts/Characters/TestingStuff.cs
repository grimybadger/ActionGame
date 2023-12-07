using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TestingAIBehaviors
{
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
		[field: SerializeField] public Rigidbody TargetRigidbody { get; private set; }
		[field: SerializeField] public float TargetRadius { get; private set; }
		[field: SerializeField] public float SlowRadius { get; private set; }
		[field: SerializeField] public float TimeToTarget { get; private set; }
		[field: SerializeField] public float RotationSpeed { get; private set; } = 10f;
		[field: SerializeField] public float TargetSpeed { get; private set; }
		[field: SerializeField] public float MaxPredictionTime { get; private set; }
		[field: SerializeField] public float PredictionTime { get; private set; }

		private SteeringOutput _steeringOutput;
		private KinematicSeek ks;

		private void Start()
		{
			Rigidbody = GetComponent<Rigidbody>();
			//TargetRigidbody = Target.GetComponent<Rigidbody>();
			_steeringOutput = new();
			ks = new() { character = gameObject, maxSpeed = MaxAcceleration, target = Target };

			//StartCoroutine(KinematicWander());
		}
		private void FixedUpdate()
		{
			//ks.GetSteering(Rigidbody); 
			SeperationBehavior();
			//KinematicArrive();
			//KinematicWander();
			//KinematicArriveSlow();
			KinematicPursue();
		}
		private void KinematicPursue()
		{
			Vector3 direction;
			float distance;
			float speed;
			Vector3 velocity = Rigidbody.velocity;
			direction = Target.transform.position - transform.position;
			distance = direction.magnitude;
			speed = velocity.magnitude;

			if (speed <= distance / MaxPredictionTime)
			{
				PredictionTime = MaxPredictionTime;
			}
			else
			{
				PredictionTime = distance / speed;
			}
			//Target.transform.position += TargetRigidbody.velocity * PredictionTime;
			KinematicSeek();
		}
		private void KinematicSeek()
		{
			Vector3 currentPos = Target.transform.position;
			Vector3 lastPos = Target.transform.position; 
			Vector3 futurespot = Target.transform.position +( TargetRigidbody.velocity * PredictionTime);
			//Vector3 futureSpot = (lastPos - transform.position) / PredictionTime; 
			
			
			//Target.transform.position += ( TargetRigidbody.velocity * PredictionTime);
			Rigidbody.velocity = futurespot - transform.position;
			Debug.Log($"Current Pos: {currentPos}, Future Spot: {futurespot}, Velocity{TargetRigidbody.velocity}");
			
			//Rigidbody.velocity += TargetRigidbody.velocity * PredictionTime;
			Rigidbody.velocity.Normalize();
			Rigidbody.velocity *= MaxAcceleration * Time.fixedDeltaTime;
			//Target.transform.position += TargetRigidbody.velocity * PredictionTime;

		}
		private void KinematicArrive()
		{
			Rigidbody.velocity = Target.transform.position - transform.position;
			Vector3 direction;
			direction = Target.transform.position - transform.position;
			if (Rigidbody.velocity.magnitude < TargetRadius)
			{
				return;
			}
			Rigidbody.velocity /= TimeToTarget;

			if (Rigidbody.velocity.magnitude > MaxAcceleration)
			{
				Rigidbody.velocity.Normalize();
				//Rigidbody.MovePosition(Rigidbody.position + (direction * MaxAcceleration * Time.fixedDeltaTime));
				//Rigidbody.AddForce(Rigidbody.velocity * MaxAcceleration, ForceMode.VelocityChange);
				Rigidbody.velocity *= MaxAcceleration * Time.fixedDeltaTime;
				//transform.position *= MaxAcceleration * Time.fixedDeltaTime;

			}

			transform.rotation = Quaternion.Slerp(Rigidbody.rotation,
				Quaternion.LookRotation(Rigidbody.velocity), Time.deltaTime * 100f);

			//Rigidbody.rotation = Quaternion.identity;

		}
		private void KinematicArriveSlow()
		{
			Vector3 direction;
			float distance;

			direction = Target.transform.position - transform.position;
			distance = direction.magnitude;

			if (distance < TargetRadius)
			{
				return;
			}

			if (distance > SlowRadius)
			{
				TargetSpeed = MaxAcceleration;
			}
			else
			{
				TargetSpeed = MaxAcceleration * distance / SlowRadius;
			}
			TargetRigidbody.velocity = direction;
			TargetRigidbody.velocity.Normalize();
			TargetRigidbody.velocity *= TargetSpeed;

			Rigidbody.velocity = TargetRigidbody.velocity - Rigidbody.velocity;
			Rigidbody.velocity /= TimeToTarget;

			if (Rigidbody.velocity.magnitude > MaxAcceleration)
			{
				Rigidbody.velocity.Normalize();
				Rigidbody.velocity *= MaxAcceleration * Time.deltaTime;
			}

		}
		private IEnumerator KinematicWander()
		{
			while (true)
			{
				Vector3 rotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

				Rigidbody.velocity = MaxAcceleration * transform.forward;

				yield return new WaitForSeconds(2f);
				Rigidbody.rotation = Quaternion.Slerp(Quaternion.Euler(0, RandomBinomial() * RotationSpeed, 0),
					Quaternion.LookRotation(Rigidbody.velocity), Time.deltaTime * RotationSpeed);
				//yield return null;
			}
		}
		private float RandomBinomial()
		{
			return Random.Range(-1, 1);
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
			float distanceFrom;

			Vector3 linear = default;
			float min = 0;
			//Vector3 previousPosition = default;
			foreach (var target in TargetList)
			{
				direction = target.transform.position - transform.position;
				distanceFrom = Vector3.Distance(target.transform.position, transform.position);
				distance = direction.magnitude;
				direction = direction.normalized;
				//direction.Normalize();
				if (distance < Threshold)
				{
					strength = Mathf.Min(DecayCoefficient / (distance * distance), MaxAcceleration);

					//strength = MaxAcceleration * (Threshold - distance) / Threshold;

					//direction *= MaxAcceleration;
					//float forceRate = (DecayCoefficient / 1);

					//_steeringOutput.Linear += strength * direction;
					//_steeringOutput.Linear = (transform.position - previousPosition) / Time.deltaTime;
					//_steeringOutput.Linear += strength * direction;
					//Rigidbody.MovePosition(Rigidbody.position + _steeringOutput.Linear);
					//GetComponent<Rigidbody>().AddForce() += strength * direction;
					//transform.position = Vector3.MoveTowards(transform.position, _steeringOutput.Linear, 5f * Time.deltaTime);

					//	Rigidbody.AddForce(direction * (forceRate / Rigidbody.mass) * -1f);

					Rigidbody.velocity += strength * direction;
					//transform.position += strength * direction * Time.deltaTime;
					//target.transform.Translate(linear * Time.deltaTime, Space.World);
					//Rigidbody.AddForce(linear);

					//Rigidbody.AddForce(transform.forward * strength);
					Debug.Log($"Force:{distance}");
				}
			}
			//Debug.Log(direction);
		}
	}
}
