using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Vida")]
    public int vida = 200;
    public int vidaMaxima = 200;

    [Header("Refs")]
    public Animator ani;
    public GameObject target;

    [Header("Estado")]
    public bool atacando;
    public bool estaMuerto = false;

    [Header("Ataque")]
    private float tiempoEntreAtaques = 1f;
    private float tiempoUltimoAtaque = 0f;
    public int danioPorSegundo = 15;

    private SpawnZombieScript spawnZombie;

    [Header("Loot Prefabs")]
    public GameObject guaranteedDrop;       
    public GameObject[] rareDrops;         

    [Header("Loot Drop Chances")]
    [Range(0f, 1f)] public float rareDropChance1 = 0.1f;
    [Range(0f, 1f)] public float rareDropChance2 = 0.1f;

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

        Debug.Log("Zombie eliminado");

        if (ani != null)
        {
            ani.SetBool("attack", false);
            ani.SetBool("run", false);
            ani.SetTrigger("die"); 
        }


        DropLoot();
        if (spawnZombie != null)
            spawnZombie.ZombieDied();

        Destroy(gameObject, 3f);
    }

    public void Final_Ani()
    {
        if (ani != null) ani.SetBool("attack", false);
        atacando = false;
    }

    private void DropLoot()
    {
        if (guaranteedDrop != null)
            Instantiate(guaranteedDrop, transform.position, Quaternion.identity);

        if (rareDrops != null && rareDrops.Length > 0)
        {
            if (rareDrops.Length >= 1 && rareDrops[0] != null && Random.value <= rareDropChance1)
                Instantiate(rareDrops[0], transform.position, Quaternion.identity);

            if (rareDrops.Length >= 2 && rareDrops[1] != null && Random.value <= rareDropChance2)
                Instantiate(rareDrops[1], transform.position, Quaternion.identity);
        }
    }
}