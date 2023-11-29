using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ObjectDetection : MonoBehaviour
{
     [Header("Decision Making")]
    [SerializeField] private DecisionMaking _decisionMaking;
    [SerializeField] private DetectionType _detectionType;
   //[field: SerializeField] public DetectionType DetectionType { get; private set; }

    [Header("Detected Object Properties")]
    [SerializeField] private List<GameObject> _detectedItems = new List<GameObject>();

   
    [Header("Arc Properties")]
    [SerializeField] private float _maxDistance = 5f;
    [Range(-10f, 10f)]
    [SerializeField] private float _centerOffset;
    [Range(0f,1f)]
    [SerializeField] private float _heightOffset;
    [Range(0f, 360f)]
    [SerializeField] private float _angle = 45f; 
    [SerializeField] private bool _isVisualized;
    [SerializeField] private bool _isWithinArc;
    [SerializeField] private float _rotationSpeed = 5;
    [SerializeField] private float _segmentNumbers;
  
    [Header("Detection Properties")]
    [SerializeField] private bool _isDetecting = false;
    [SerializeField] private bool _useRadar;
    [SerializeField] private bool _mDistanceDetection; // Are we using distance to detect instead of collider
    private bool _detectedObject; 
   
    private Coroutine _co;
    private Collider _col;

    private float _timeCounter = 0;

    private void Start() 
    {
        _decisionMaking = new DecisionMaking(this);
        _col = GetComponent<Collider>();
    }

    private void DrawLine()
    {
        _timeCounter += Time.deltaTime * _rotationSpeed;
        float x = Mathf.Cos(_timeCounter);
        float y = Mathf.Sin(_timeCounter);
        RaycastHit hit;
        DetectedObject = false;
        DivideIntoSegments(Angle);
       
        if (_useRadar)
        {  
            Vector3 centerOffset = new Vector3(_col.bounds.center.x, gameObject.transform.position.y + HeightOffset, _col.bounds.center.z + CenterOffset);
            Debug.DrawRay(centerOffset, transform.TransformDirection(x, 0, y) * _maxDistance, Color.red);
            if (Physics.Raycast(centerOffset, transform.TransformDirection(x, 0, y), out hit, _maxDistance))
            {
                if (!_detectedItems.Contains(hit.collider.gameObject))
                {
                    Debug.DrawLine(centerOffset, hit.collider.gameObject.transform.position, Color.yellow, Mathf.Infinity);
                    _detectedItems.Add(hit.collider.gameObject);
                }
                Debug.DrawRay(centerOffset, transform.TransformDirection(x, 0, y) * _maxDistance, Color.yellow);
                Debug.Log(hit.collider.gameObject.name);
                DetectedObject = true;
            }
        }
    }
 
    private void DivideIntoSegments(float angle)
    {   
        Vector3 leftHandle = Quaternion.Euler(0, angle / 2, 0) * transform.forward;
        Vector3 rightHandle = Quaternion.Euler(0, -angle / 2 , 0) * transform.forward;
        Vector3 centerOffset = new Vector3(_col.bounds.center.x, gameObject.transform.position.y + HeightOffset, _col.bounds.center.z + CenterOffset);
        Debug.DrawRay(centerOffset, leftHandle * MaxDistance, Color.yellow);
        Debug.DrawRay(centerOffset, rightHandle * MaxDistance, Color.yellow);
        Debug.DrawRay(centerOffset, transform.forward * MaxDistance, Color.magenta);//Center Line

        float segmentSizes = (angle /2) ;
        DetectedObject = false;
        for (int i = 0; i < SegmentNumber; i++)
        {
            Vector3 direction = Quaternion.Euler(0, GetAngle(segmentSizes,SegmentNumber,i), 0) * transform.forward;
            Vector3 mirrorDirection = Quaternion.Euler(0,GetAngle(-segmentSizes,SegmentNumber,i), 0) * transform.forward;
            //Debug.Log(segmentSizes);
            //Debug.Log(GetAngle(segmentSizes, d.SpokeNumber, i));
            Debug.DrawRay(centerOffset, direction * MaxDistance, Color.green);
            Debug.DrawRay(centerOffset, mirrorDirection * MaxDistance, Color.cyan);
            RaycastHit hit;
           
            if (Physics.Raycast(centerOffset, mirrorDirection,out hit, MaxDistance) || Physics.Raycast(centerOffset, direction,out hit, MaxDistance))
            {
                if (!_detectedItems.Contains(hit.collider.gameObject))
                {
                    _detectedItems.Add(hit.collider.gameObject);
                    if(hit.collider.gameObject.GetComponent<ObjectDetection>())
                    {
                        _decisionMaking.Decision(_detectionType);
                    }
                    else
                    {
                        Debug.Log($"No Detection Class On:{hit.collider.gameObject}");
                    }
                }
                //DetectDecisionMaking.Decision(_detectionType);
                Debug.Log(hit.collider.gameObject.name);
                DetectedObject = true;
            }
        }
    }
    private float GetAngle(float angle, float segmentDivisions, float currentSegment)
    {
        if(segmentDivisions == 1 )
        {
            return angle = angle / 2; 
        }
       float segments =  angle / segmentDivisions;
       float angleDegree = (angle - (segments * (currentSegment + 1)));
       return angleDegree;
    } 

    private IEnumerator Detect()
    {
        _isDetecting = !_isDetecting;

        if(_isDetecting)
        {
           while(true)
            {
                DrawLine();
                DistanceDetection();
                print("Deep in THAT COROUTINE");
                yield return null;
            }
        }
        else if(!_isDetecting)
        {
            StopAllCoroutines();
            print("Stop Detection");
        }
    }
    private void DistanceDetection()
    {
        if(_mDistanceDetection)
        {
            DetectDecisionMaking.Decision(_detectionType);
            _mDistanceDetection = false;
        } 
    }
    public void ToggleDetection() => _co = StartCoroutine(Detect());

    public Vector3 ArcDirection()
    {
        if(gameObject.transform.rotation.x != 0)
        {
            return new Vector3(0,0,-1);
        }
        return Vector3.up;
    }
    #region Properties
    public List <GameObject> DetectedItems
    {
        get => _detectedItems;
        set => _detectedItems = value;
    } 
    public float MaxDistance
    {
        get => _maxDistance;
        set => _maxDistance = value;
    }
    public float Angle
    {
        get => _angle;
        set => _angle = value;
    }
    public bool IsVisualized
    {
        get => _isVisualized;
        set => _isVisualized = value;
    }
    public bool IsWithinArc
    {
        get => _isWithinArc;
        set => _isWithinArc = value;
    }
    public float SegmentNumber
    {
        get => _segmentNumbers;
        set => _segmentNumbers = value;
    }
    public float CenterOffset
    {
        get => _centerOffset;
        set => _centerOffset = value;
    }

    public float HeightOffset
    {
        get => _heightOffset;
        set => _heightOffset = value;
    }

    /// <summary>
    /// Properties having to do with Detection
    /// </summary>
    public DecisionMaking DetectDecisionMaking
    {
        get => _decisionMaking;
        set => _decisionMaking = value;
    }
    /*
    public DetectionType DetectionType
    {
        get => _detectionType;
        set => _detectionType = value;
    }*/
    public bool DetectedObject
    {
        get => _detectedObject;
        set => _detectedObject = value;
    }
    public Collider Collider
    {
        get => _col;
        set => _col = value;
    }
    public bool MDistanceDetection
    {
        get => _mDistanceDetection;
        set => _mDistanceDetection = value;
    }
    #endregion
}
