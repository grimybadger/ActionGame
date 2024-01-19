using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
	// [Header("General Properties"), Tooltip("Are we controlling this character?")]
	// public bool isPlayerControlled;

	[field: Header("Target Properties")]
	[field: SerializeField] public EnemyMovement EnemyMovement { get; private set; }
	[Header("Set In Inspector")]
	public float countdown;

	[Header("Set Dynamically")]
	[SerializeField] private float _threatTimer;
	[SerializeField] Transform target;
	[SerializeField] private bool _isTargetAThreat;
	[SerializeField] private bool _isTargetInRange;
	[SerializeField] private float _targetDistance = 0;

	[Space(10)]
	[Header("Target Movement")]
	[SerializeField] private bool _isInCover;
	[SerializeField] private string _coverName;

	[field: Header("General Settings")]
	// [SerializeField] private EnemyVisibility _enemyVisibility;
	[field: SerializeField] public Animator Animator { get; private set; }
	// [SerializeField] private Firearm _firearm;
	[field: SerializeField] public bool IsAttacking { get; private set; }

	[SerializeField] private List<string> _attacks;

	[field: Header("Logic")]
	[field: SerializeField] public bool IsAssistanceRequired { get; private set; }
	[field: SerializeField] public bool IsOneOnOneDuel { get; private set; }
	[field: SerializeField] public bool IsRefusingToEngage { get; private set; }
	[field: SerializeField] public bool IsEngagingTarget { get; private set; }
	[field: SerializeField] public bool IsTaunting { get; private set; }

	private bool _isAiming;
	private float _defaultCountdown;

	//The state machine that manages this object
	private StateMachine _stateMachine;

	//[SerializeField] private Speech _speech; 
	//[SerializeField] private Squad  _squad;

	private void Awake()
	{
		Animator = GetComponent<Animator>();
	}

	public void Start()
	{
		_defaultCountdown = countdown;

		// _squad = gameObject?.GetComponent<Squad>();  
		// _speech = gameObject?.GetComponent<Speech>();
		// _enemyVisibility = gameObject?.GetComponent<EnemyVisibility>(); 

		var originalRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
		// Create the state machine
		_stateMachine = new StateMachine();

		// The first state we register will be the initial state
		var idle = _stateMachine.CreateState("idle");
		var attacking = _stateMachine.CreateState("attacking");
		var moving = _stateMachine.CreateState("moving");

		idle.onEnter = delegate
		{
			Debug.Log("Idiling");
		};
		idle.onFrame = delegate
		{
			if (EnemyMovement.IsMoving)
			{
				_stateMachine.TransitionTo("moving");
			}
		};
		idle.onExit = delegate { };


		// Log when we enter the state
		moving.onEnter = delegate { Debug.Log("Currently moving"); };
		moving.onFrame = delegate
		{
			if (EnemyMovement.IsMoving)
			{
				if (EnemyMovement.RandomInt == 1)
				{
					Animator.SetBool("isWalking", true);
				}
				else if (EnemyMovement.RandomInt == 2)
				{
					//EnemyMovement.MoveSpeed = 5f;
					Animator.SetBool("isRunning", true);
				}

			}
			else if (!EnemyMovement.IsMoving)
			{
				Animator.SetBool("isWalking", false);
				Animator.SetBool("isRunning", false);
			//	EnemyMovement.MoveSpeed = 3f;
				EnemyMovement._hasDecidedMovement = false;
				_stateMachine.TransitionTo("attacking");
			}

			/*
				if(_enemyVisibility.isTargetVisible)
				{
					if(_squad.IsSomethingUp)
					{
						return;
					}
						_stateMachine.TransitionTo("aiming");  
						Animator.SetBool("isInSight", true);
						_isAiming = true;
						_squad.SquadTarget = target.GetComponent<HealthPool>();

						AlarmClock.onCalledAlarmClock(countdown,transform.position);
						_speech?.SetSpeechProperties();
						_speech?.DisplaySpeech();
						_squad?.AlertSquadMates();
						StartCoroutine(_speech?.StartTimer(countdown));

				} else if(_squad.IsSomethingUp)
				{
					_stateMachine.TransitionTo("aiming");
					Animator.SetBool("isInSight", true);
					_isAiming = true;
				}
	*/

		};
		moving.onExit = delegate { Debug.Log("Exited Moving"); };


		attacking.onEnter = delegate { Debug.Log("Currently Attacking"); };
		// Every Frame, keep the enemy aimed at the target. Detect 
		// when the target leaves range. 
		attacking.onFrame = delegate
		{
			if (_isTargetAThreat)
			{
				IsAttacking = true;
				//EnemyMovement.StartMovement();
				if (IsAttacking)
				{
					//if(!_isTargetInRange)
					//	{
					//StartCoroutine(EnemyMovement.CheckForMovement());
					EnemyMovement.GoToPosition = target.transform.position;
					//EnemyMovement.StartMovement();
					//transform.position = Vector3.MoveTowards(transform.position, EnemyMovement.GoToPosition, 0);
					//StartCoroutine(EnemyMovement.CheckForMovement());
					//	}
					_targetDistance = Vector3.Distance(transform.position, target.transform.position);
					Debug.Log($"Distance to Player: {_targetDistance}");
					if (_targetDistance < 1.6f)
					{
						_isTargetInRange = true;
						Animator.Play($"{_attacks[0]}");
						EnemyMovement.IsMoving = false;
						Debug.Log("Gap has been closed");
						//EnemyMovement.StopCoroutine(EnemyMovement.);
					}
					else if (_targetDistance > 3f)
					{
						//IsAttacking = false;
						//IsTargetAThreat = false;
						//_isTargetInRange = false;
						_stateMachine.TransitionTo("idle");
						//EnemyMovement.ObjectDetection.DetectedItems.Remove(target.gameObject);
						//EnemyMovement.StartMovement();
						
						EnemyMovement.IsMoving = true;
					}
					//Animator.Play($"{_attacks[0]}");
				}
			}

			/*
			transform.LookAt(target.position);
			transform.rotation *= Quaternion.Euler(0, 50, 0);

			if (!_enemyVisibility.isTargetVisible && !_squad.IsSomethingUp)
			{
				_stateMachine.TransitionTo("idleRifle");
				Animator.SetBool("isInSight", false);
				transform.rotation = originalRotation;
				_isAiming = false;
				countdown = _defaultCountdown;
				AlarmClock.onCalledAlarmClock(0, transform.position);
			}

			if (_isTargetAThreat)
			{
				Vector3 targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 1f, target.transform.position.z);
				Ray ray = new Ray(_firearm.barrelLocation.transform.position, targetPos - _firearm.barrelLocation.transform.position);
				_firearm.bulletDirection = ray.direction;
				Animator.SetBool("isThreat", true);
				_firearm.animator.SetTrigger("Fire");
			}
*/
		};
		attacking.onExit = delegate {
			Debug.Log("Exited Attacking");			
		 };

		/*
				attacking.onEnter = delegate
				{

					Debug.Log("Target is in range");
				};

				attacking.onExit = delegate
				{

					Debug.Log("Target went out of range!");
				};
		*/
		/*
		var threat = stateMachine.CreateState("threat"); 

		threat.onEnter = delegate{
			
				Debug.Log("Target Is About To GET CLAPPED."); 

		}; 

		threat.onFrame = delegate{

		   
			transform.LookAt(target.position);
			transform.rotation *= Quaternion.Euler(0, 50, 0);
			stateMachine.TransitionTo("aiming");
			animator.SetBool("isInSight", true);
		   
			}; 


	   
		var firing = stateMachine.CreateState("firing");

		//Every frame we check if we are aiming and if the target is deemed a threat then we start to fire until the target
		// is neutralized
		firing.onFrame = delegate {

			transform.LookAt(target.position);
			transform.rotation *= Quaternion.Euler(0,50,0);
		};*/

	}

	private void Update()
	{
		// Update the state machine current state;
		// if (!isPlayerControlled)
		// {
		/*
	foreach(var item in EnemyMovement.ObjectDetection.DetectedItems)
	{

		if(item.GetComponent<Character>() !=null && item.GetComponent<Character>().CharacterType == CharacterType.enemy)
		{
			if(Vector3.Distance(transform.position, item.transform.position) < 1)
			{
				transform.Translate(Vector3.right * 4f * Time.deltaTime);
			}
		}
	}*/
		_stateMachine.Update();
		//Aiming();
		//  }
	}

	public void Aiming()
	{
		if (_isAiming)
		{
			countdown -= Time.deltaTime;
			_threatTimer = countdown;

			if (_threatTimer < 0)
			{
				_isTargetAThreat = true;
				_isAiming = false;
				countdown = _defaultCountdown;
			}
		}
	}

	#region Properties
	public bool IsTargetAThreat
	{
		get => _isTargetAThreat;
		set => _isTargetAThreat = value;
	}
	public bool IsInCover
	{
		get => _isInCover;
		set => _isInCover = value;
	}
	public string CoverName
	{
		get => _coverName;
		set => _coverName = value;
	}
	#endregion
}
