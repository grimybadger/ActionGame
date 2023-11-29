using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    katana,
    spear,
    none
}
public class Weapon : MonoBehaviour
{
    [field: SerializeField] public WeaponType WeaponType { get; private set; } = WeaponType.katana;
}
