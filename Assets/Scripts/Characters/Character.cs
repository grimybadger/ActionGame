using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField] public CharacterType CharacterType { get; private set; }
    [field: SerializeField] public GameObject Weapon { get; private set; }
}
