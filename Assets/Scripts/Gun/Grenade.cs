using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 2f;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public GameObject explosionEffect;

    private int damage;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    void Explode()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            Zombie enemy = nearby.GetComponent<Zombie>();
            if (enemy != null)
                enemy.RecibirDano(damage);
        }

        Destroy(gameObject);
    }
}
