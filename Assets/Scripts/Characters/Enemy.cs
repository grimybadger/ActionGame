using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyLogic
{
	public class Enemy : MonoBehaviour
	{
		[field: SerializeField] public EnemyMovement Movement { get; private set; }
		[field: SerializeField] public EnemyBehavior Behavior { get; private set; }
		[field: SerializeField] public EnemyTactics Tactics { get; private set; }
		[field: SerializeField] public Health Health { get; private set; }
	}
}
