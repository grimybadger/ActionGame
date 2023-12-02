using System.Collections;
using System.Collections.Generic;
using EnemyLogic;
using UnityEngine;

public class EnemyTactics : MonoBehaviour
{
	[field: SerializeField] public List<Enemy> Squad { get; private set; }

	private void Update()
	{
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
		}
	}

}
