using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float HealthPool { get; private set; }
    [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
    //[field: SerializeField] public Color DamageColor { get; private set; }
    //[field: SerializeField] public Material Material{ get; private set;}
    [field: SerializeField] public Character Character { get; private set; }
    Color _defaultMatColor;
    const float KatanaDamage = 15;

    private void Start()
    {
        //_defaultMatColor = Material.color;
        Character = GetComponent<Character>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Weapon>() && GetComponent<Character>().Weapon != other.GetComponent<Weapon>().gameObject)
        {
            TakeDamage(other.GetComponent<Weapon>().WeaponType);
            //gunFlash.Emit(1);
            ParticleSystem.Emit(20);
            //ParticleSystem.Stop();
            //ParticleSystem.Play();
            //StartCoroutine(FlashColor());
        }
    }
    private void TakeDamage(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.katana:
                HealthPool -= KatanaDamage;
                break;
            default:
                break;
        }
    }
    private IEnumerator FlashColor()
    {
        // Material.SetColor("_EmissionColor", DamageColor);
        //  Material.color = DamageColor;
        yield return new WaitForSeconds(0.1f);
        // Material.color = _defaultMatColor;
    }
}
