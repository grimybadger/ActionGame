using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    [Space(10)]
    [Header("Target Movement")]
    [SerializeField] private bool _isInCover;
    [SerializeField] private string _coverName;

    [field: Header("Serialized Fields")]
    // [SerializeField] private EnemyVisibility _enemyVisibility;
    [field: SerializeField] public Animator Animator { get; private set; }
    // [SerializeField] private Firearm _firearm;


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

        // Log when we enter the state
        idle.onEnter = delegate { Debug.Log("Currently idling"); };
        idle.onFrame = delegate
        {

            if (EnemyMovement.IsMoving)
            {
                  //_stateMachine.TransitionTo("aiming");
                  if(EnemyMovement.RandomInt == 1)
                  {
                     Animator.SetBool("isWalking", true);
                  }
                  else if(EnemyMovement.RandomInt == 2)
                  {
                    EnemyMovement.MoveSpeed = 6f;
                    Animator.SetBool("isRunning", true);
                  }
                   
            }
            else if(!EnemyMovement.IsMoving)
            {
                Animator.SetBool("isWalking", false);
                Animator.SetBool("isRunning", false);
                EnemyMovement.MoveSpeed = 3f;
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

        var aiming = _stateMachine.CreateState("aiming");

        // Every Frame, keep the enemy aimed at the target. Detect 
        // when the target leaves range. 
        aiming.onFrame = delegate
        {
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

        aiming.onEnter = delegate
        {

            Debug.Log("Target is in range");
        };

        aiming.onExit = delegate
        {

            Debug.Log("Target went out of range!");
        };

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
