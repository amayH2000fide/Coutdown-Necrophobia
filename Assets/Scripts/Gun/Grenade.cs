using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 2f;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;


    public GameObject sparks;
    public GameObject flash;

    private int baseDamage;
    private int gunLevel = 1;
    private float damageMultiplier = 1f;

    public void SetDamage(int dmg, int level = 1, float multiplier = 1f)
    {
        baseDamage = dmg;
        gunLevel = level;
        damageMultiplier = multiplier;
    }

    private void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    void Explode()
    {
        if (flash != null)
            Instantiate(flash, transform.position, transform.rotation);

        if (sparks != null)
            Instantiate(sparks, transform.position, Quaternion.identity);


        int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier * gunLevel);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            Zombie enemy = nearby.GetComponent<Zombie>();
            if (enemy != null)
                enemy.RecibirDano(finalDamage);
        }

        Destroy(gameObject);
    }
}
