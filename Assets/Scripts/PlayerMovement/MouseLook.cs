using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [field: SerializeField] public float TurnSpeed { get; private set; } = 90f;

    Quaternion _bodyStartOrientation;
    float _yaw;
    float _pitch;

    // Start is called before the first frame update
    void Start()
    {
        _bodyStartOrientation = transform.localRotation;
    }

    private void FixedUpdate()
    {
        
        var horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * TurnSpeed;
        var vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * TurnSpeed;

        _yaw += horizontal;
        _pitch += vertical;

        var bodyrotation = Quaternion.AngleAxis(_yaw, Vector3.up);

        //  var bodyrotation = Quaternion.AngleAxis
       // transform.localRotation = bodyrotation * _bodyStartOrientation;
       // transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        
        //transform.forward = Camera.main.transform.forward;

        //transform.localRotation = Camera.main.transform.rotation;
    }

    private void Update() 
    {
         transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
    }
}
