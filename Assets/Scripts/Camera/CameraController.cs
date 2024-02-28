using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public GameObject Player { get; private set; }
    [field: SerializeField] public Vector3 Offset { get; private set; }

    // Update is called once per frame
    void Update()
    {
        Camera.transform.position = new Vector3(Player.transform.position.x + Offset.x, Player.transform.position.y
            + Offset.y, Player.transform.position.z + Offset.z);
    }
}
