using System.Collections;
using System.Collections.Generic;
using EnemyLogic;
using UnityEngine;

public enum Formation
{
	circle,
	wedge,
	line,
	none
}
public class EnemyTactics : MonoBehaviour
{
	[field: SerializeField] public List<Enemy> Squad { get; private set; }
	[field: SerializeField] public int SlotNumber { get; private set; }
	[field: SerializeField] public GameObject Target { get; private set; }
	[field: SerializeField] public Vector3 SlotPosition { get; private set; }
	[field: SerializeField] public Formation Formation { get; private set; }
	[field: SerializeField] public float CharacterRadius { get; private set; }
	[field: SerializeField] public int AssignedSlot { get; private set; }
	[field: SerializeField] public EnemyBehavior Behavior { get; private set; }

 	private void Start() 
	{
		for (int i = 0; i < Squad.Count; i++)
		{
			if(Squad[i] == GetComponent<Enemy>())
			{
				SlotNumber = i;
			}
		}
		Behavior = GetComponent<EnemyBehavior>();
	}
	private void FixedUpdate()
	{
		DefensiveCirclePattern();
		/*
		foreach (var enemy in Squad)
		{
			if (enemy.Behavior.IsAssistanceRequired)
			{
				for (int i = 0; i < Squad.Count; i++)
				{
					if (!Squad[i].Behavior.IsRefusingToEngage)
					{
						enemy.Behavior.IsTargetAThreat = true;
					}
				};
				break;
			}
		}*/
	}
	
	private void DefensiveCirclePattern()
	{
		float angleAroundCircle;
		//float radius = 5f;
		float radius;
		List<Vector3> slotLists = new(); 
	
		for (int i = 0; i < Squad.Count; i ++)
		{
			//angleAroundCircle = i / Squad.Count * Mathf.PI * 2;
			angleAroundCircle = 2 * Mathf.PI * i / Squad.Count;
			radius = CharacterRadius / Mathf.Sin(Mathf.PI / Squad.Count);
			Vector3 pos; 
			pos = new Vector3(Target.transform.position.x + radius * Mathf.Cos(angleAroundCircle), 0,
				Target.transform.position.z + radius * Mathf.Sin(angleAroundCircle));
			slotLists.Add(pos);
			/*
			//Debug.Log($"Enemy: {gameObject},Angle:{angleAroundCircle}, Radius{radius}");
			//Squad[i].gameObject.transform.position = new Vector3(Target.transform.position.x + radius * Mathf.Cos(angleAroundCircle), 0,
			SlotPosition = new Vector3(Target.transform.position.x + radius * Mathf.Cos(angleAroundCircle), 0,
				Target.transform.position.z + radius * Mathf.Sin(angleAroundCircle)); 
			Squad[i].Movement.GoToPosition = SlotPosition;
			//gameObject.transform.LookAt(Target.transform);
			*/
			
		}
		if (Behavior.IsAttacking) return;
		AssignPosition(SlotNumber, slotLists);
		Squad[SlotNumber].Movement.GoToPosition = SlotPosition;
				
	}
	
	private void AssignPosition(int index, List<Vector3> posList)
	{
		switch (index)
		{
			case 0:
				SlotPosition = posList[2];
				AssignedSlot = 2;
				break;
			case 1:
				SlotPosition = posList[4];
				AssignedSlot = 4;
				break;
			case 2:
				SlotPosition = posList[5];
				AssignedSlot = 5;
				break;
			case 3:
				SlotPosition = posList[3];
				AssignedSlot = 3;
				break;
			case 4:
				SlotPosition = posList[1];
				AssignedSlot = 1;
				break;
			case 5:
				SlotPosition = posList[0];
				AssignedSlot = 0; 
				break;
			default:
				break;
		}
		
	}
	
	
}
