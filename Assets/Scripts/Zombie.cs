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

    SpawnZombieScript spawnZombie;


    void Awake()
    {
        spawnZombie = FindObjectOfType<SpawnZombieScript>();
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
    }

    public void Comportamiento_Enemigo()
    {
        if (target == null || estaMuerto) return;

        float distancia = Vector3.Distance(transform.position, target.transform.position);

        Vector3 direccion = target.transform.position - transform.position;
        direccion.y = 0;
        if (direccion != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direccion);
        }

        if (distancia <= 2f)
        {
            ani.SetBool("run", false);
            ani.SetBool("attack", true);
            atacando = true;

            if (Time.time - tiempoUltimoAtaque >= tiempoEntreAtaques)
            {
                PlayerStatController player = target.GetComponent<PlayerStatController>();
                if (player != null)
                {
                    player.DamageTaken(danioPorSegundo);
                }
                else
                {
                    Debug.LogWarning("El objeto no tiene componente Jugador");
                }

                tiempoUltimoAtaque = Time.time;
            }
        }
        else
        {
            ani.SetBool("attack", false);
            ani.SetBool("run", true);
            atacando = false;

            transform.Translate(Vector3.forward * 2 * Time.deltaTime);
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
        Debug.Log("Zombie eliminado");

        if (ani != null)
        {
            ani.SetTrigger("die");
        }
        spawnZombie.ZombieDied();
        Destroy(gameObject, 2f);
    }

    public void Final_Ani()
    {
        ani.SetBool("attack", false);
        atacando = false;
    }

    void Update()
    {
        Comportamiento_Enemigo();
    }
}