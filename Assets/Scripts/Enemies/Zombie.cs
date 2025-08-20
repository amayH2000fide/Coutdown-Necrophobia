using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int vida = 200;
    public int vidaMaxima = 200;

    public Animator ani;
    public GameObject target;
    public bool atacando;
    public bool estaMuerto = false;

    private float tiempoEntreAtaques = 1f;
    private float tiempoUltimoAtaque = 0f;
    public int danioPorSegundo = 15;

    private SpawnZombieScript spawnZombie;

    void Awake()
    {
        spawnZombie = FindObjectOfType<SpawnZombieScript>();
        if (ani == null) ani = GetComponent<Animator>();
    }

    void Start()
    {
        if (ani == null) ani = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
        vida = Mathf.Clamp(vida, 0, vidaMaxima);
    }

    void Update()
    {
        Comportamiento_Enemigo();
    }

    public void Comportamiento_Enemigo()
    {
        if (target == null || estaMuerto) return;

        float distancia = Vector3.Distance(transform.position, target.transform.position);

        Vector3 direccion = target.transform.position - transform.position;
        direccion.y = 0f;
        if (direccion.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(direccion);

        if (distancia <= 2f)
        {
            ani.SetBool("run", false);
            ani.SetBool("attack", true);
            atacando = true;

            if (Time.time - tiempoUltimoAtaque >= tiempoEntreAtaques)
            {
                var player = target.GetComponent<PlayerStatController>();
                if (player != null)
                {
                    player.DamageTaken(danioPorSegundo);
                }
                else
                {
                    Debug.LogWarning("El objeto con tag 'Player' no tiene PlayerStatController.");
                }

                tiempoUltimoAtaque = Time.time;
            }
        }
        else
        {
            ani.SetBool("attack", false);
            ani.SetBool("run", true);
            atacando = false;

            transform.Translate(Vector3.forward * 2f * Time.deltaTime);
        }
    }

    public void RecibirDano(int cantidad)
    {
        if (estaMuerto) return;

        vida -= cantidad;
        Debug.Log($"Zombie recibió {cantidad} de daño. Vida restante: {vida}");

        if (vida <= 0)
        {
            vida = 0;
            Morir();
        }
    }

    void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;

        if (ani != null)
        {
            ani.SetBool("attack", false);
            ani.SetBool("run", false);

            ani.SetTrigger("die");
        }

        //var col = GetComponent<Collider>();
       // if (col != null) col.enabled = false;

        Debug.Log("Zombie eliminado");

        if (spawnZombie != null)
            spawnZombie.ZombieDied();

        Destroy(gameObject, 3f);
    }

    public void Final_Ani()
    {
        ani.SetBool("attack", false);
        atacando = false;
    }
}