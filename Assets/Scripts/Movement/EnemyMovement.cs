using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [field: SerializeField] public GameObject Player { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; set; } = 3f;
    [field: SerializeField] public ObjectDetection ObjectDetection { get; private set; }
    [field: SerializeField] public bool IsMoving { get; private set; }
    [field: SerializeField] public int RandomInt { get; private set; } = 0;

    private bool _hasDecidedMovement = false;
    private void Update()
    {
        if (IsMoving)
        {
            if(!_hasDecidedMovement)
            {
                GetRandomInt();
            }
            Movement();
        }
    }
    private void Movement()
    {
        if (!ObjectDetection.IsWithinArc)
        {
            var step = MoveSpeed * Time.deltaTime; // calculate distance to move
            Debug.Log(step);
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
            CheckForPlayer();
        }
    }
    private void CheckForPlayer()
    {
        if (ObjectDetection.DetectedItems.Contains(Player))
        {
            IsMoving = false;
        }
    }

    private void GetRandomInt()
    {
        RandomInt = Random.Range(1, 3);
        Debug.Log($"Movement: { RandomInt}");
        _hasDecidedMovement = true;
    }

}
