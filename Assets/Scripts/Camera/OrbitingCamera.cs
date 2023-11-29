using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OrbitingCamera : MonoBehaviour
{

    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; } = 120f;
    [field: SerializeField] public float ElevationSpeed { get; private set; } = 120f;
    [field: SerializeField] public float ElevationMinLimit { get; private set; } = -20f;
    [field: SerializeField] public float ElevationMaxLimit { get; private set; } = 80f;
    [field: SerializeField] public float Distance { get; private set; } = 5.0f;
    [field: SerializeField] public float DistanceMin { get; private set; } = 0.5f;
    [field: SerializeField] public float DistanceMax { get; private set; } = 15;
    [field: SerializeField] public float RotationAroundTarget { get; private set; } = 0.0f;
    [field: SerializeField] public float ElevationToTarget { get; private set; } = 0.0f;
    [field: SerializeField] public Vector3 Offset{ get; private set; }


    float _previousDistance; 
    // Start is called before the first frame update
    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        RotationAroundTarget = angles.y;
        ElevationToTarget = angles.x;
        if(Target)
        {
            float currentDistance = (transform.position - Target.position).magnitude;
            Distance = Mathf.Clamp(currentDistance, DistanceMin, DistanceMax);
        }
    }

    private void LateUpdate() 
   
    {

        if(Input.GetMouseButton(2))
        {
            Distance = DistanceMin;
        } else if(Input.GetMouseButtonUp(2))
        {
            Distance = DistanceMax;
        }

        if(Target)
        {
            RotationAroundTarget += Input.GetAxis("Mouse X") * RotationSpeed * Distance * 0.02f;//Change .02f to a variable name
            ElevationToTarget -= Input.GetAxis("Mouse Y") * ElevationSpeed * 0.02f;//Change .02f to a variable name
            ElevationToTarget = ClampAngle(ElevationToTarget, ElevationMinLimit, ElevationMaxLimit);

            Quaternion rotation = Quaternion.Euler(ElevationToTarget, RotationAroundTarget, 0);

            Distance = Distance - Input.GetAxis("Mouse ScrollWheel") * 5;//Change 5 to a variable name
            Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
          

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -Distance);
            Vector3 position = rotation * negDistance + new Vector3(Target.position.x + Offset.x, Target.position.y + Offset.y, Target.position.z + Offset.z);

            transform.position = position;
            transform.rotation = rotation;

        }

    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
