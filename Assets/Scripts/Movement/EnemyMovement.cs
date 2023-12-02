using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;



public class EnemyMovement : MonoBehaviour
{
	[field: SerializeField] public GameObject Player { get; private set; }
	[field: SerializeField] public float MoveSpeed { get; set; } = 2f;
	[field: SerializeField] public ObjectDetection ObjectDetection { get; private set; }
	[field: SerializeField] public bool IsMoving { get; private set; }
	[field: SerializeField] public int RandomInt { get; private set; } = 0;
	[field: SerializeField] public Transform GoToPosition { get; set; }

	public bool _hasDecidedMovement = false;
	private bool _hasPosition;

	private SegmentSpot _spot;

	//public Vector3 gotoPos = default;

	private void Start()
	{
		GoToPosition = transform;
	}
	private void LateUpdate()
	{
		if (IsMoving)
		{
			if (!_hasDecidedMovement)
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
			// Debug.Log(step);
			transform.LookAt(Player.transform);

			//if (!_hasPosition) GetPlayerPosition();
			transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, step);
			//transform.position = Vector3.MoveTowards(transform.position, GoToPosition.transform.position, 0f * Time.deltaTime);
			//transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 0f);
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
		Debug.Log($"Movement: {RandomInt}");
		_hasDecidedMovement = true;
	}
	public IEnumerator CheckForMovement()
	{
		//Need to put something in here to trigger movement again and make it look natural 
		yield return new WaitForSeconds(1);
		IsMoving = true;
	}
	private SegmentSpot GetPlayerPosition()
	{
		_spot = Player.GetComponent<ObjectDetection>().SegmentSpots.FirstOrDefault(x => !x.HasSpotBeenClaimed);
		_spot.HasSpotBeenClaimed = true;
		_hasPosition = true;
		/*
		Vector3 pos = default; 
		
		foreach (var item in Player.GetComponent<ObjectDetection>().SegmentSpots)
		{
			if(!item.HasSpotBeenClaimed)
			{
				item.HasSpotBeenClaimed = true;
				pos = item.Position;
				break;
			}
		}*/
		return _spot;
	}
}
