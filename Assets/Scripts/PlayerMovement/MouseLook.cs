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
    
        private void Update()
        {

        // var horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * TurnSpeed;
        //var vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * TurnSpeed;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        movementDirection.Normalize();
        // _yaw += horizontal;
        //_pitch += vertical;
       /// transform.Translate(movementDirection* 1f * Time.deltaTime,Space.World);
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
           Quaternion a = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
  
            transform.rotation = Quaternion.RotateTowards(transform.rotation, a, 750f * Time.deltaTime);
        }
            //var bodyrotation = Quaternion.AngleAxis(_yaw, Vector3.up);

            //  var bodyrotation = Quaternion.AngleAxis
           // transform.localRotation = bodyrotation * _bodyStartOrientation;
           // transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

            //transform.forward = Camera.main.transform.forward;

            //transform.localRotation = Camera.main.transform.rotation;
            // transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }
    
    //private void Update()
   // {
       // Quaternion q = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
       // Quaternion a = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
       // transform.rotation = Quaternion.Lerp(q, a, 2f * Time.deltaTime);
        //transform.rotation =  Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
   // }
}
