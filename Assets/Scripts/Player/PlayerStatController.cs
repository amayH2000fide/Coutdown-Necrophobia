using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{

    //stats para incrementar por level up o bonus
    public int incrementStats;
    public int incrementPercentageStats;
    public int MaxLevelUp;

    public enum StatType
    {
        health,
        damage,
        Speed,
        MaxSpeed,
        shootingSpeed,
        crit,
        critDamage,
        maxHealth,
        level
    }

    [SerializeField] private Dictionary<StatType, int> stats = new Dictionary<StatType, int>();


    //para agarrar el stat de aqui por si se necesita para el ui o demas.
    public int GetStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.maxHealth:
                return stats[StatType.maxHealth];
            case StatType.health:
                return stats[StatType.maxHealth];
            case StatType.level:
                return stats[StatType.maxHealth];
            case StatType.damage:
                return stats[StatType.maxHealth];
            case StatType.crit:
                return stats[StatType.maxHealth];
            case StatType.critDamage:
                return stats[StatType.maxHealth];
            case StatType.shootingSpeed:
                return stats[StatType.maxHealth];
            case StatType.Speed:
                return stats[StatType.maxHealth];
            default:
                return 0;
        }
    }

    //para los bonuses de level up para stats, automaticamente agrega los stats
    public void addstats(StatType stat)
    {
        switch (stat)
        {
            case StatType.damage:
                stats[StatType.damage] += incrementStats;
                break;
            case StatType.Speed:
                stats[StatType.Speed] += incrementStats;
                stats[StatType.MaxSpeed] += incrementPercentageStats;
                break;
            case StatType.shootingSpeed:
                stats[StatType.shootingSpeed] += incrementStats;
                break;
            case StatType.crit:
                if (stats[StatType.crit] < 100)
                {
                    stats[StatType.crit] += incrementPercentageStats;
                };
                break;
            case StatType.critDamage:
                stats[StatType.critDamage] += incrementPercentageStats;
                break;
            default:
                Debug.Log("stat no se puede agregar, o se utiliza otro metodo");
                break;
        };
    }

    //sube el nivel del jugador y le mulptiplica la vida, puede subir de nivel mas de del max level up, pero no le sube la vida.
    public void LevelUp()
    {

        stats[StatType.level] += 1;

        if (stats[StatType.level] < MaxLevelUp) {
            float healthPercent = (float)stats[StatType.health] / stats[StatType.maxHealth];
            stats[StatType.maxHealth] = (int)(stats[StatType.maxHealth] * 1.5f);
            stats[StatType.health] = Mathf.RoundToInt(stats[StatType.maxHealth] * healthPercent);
        } else {
            Debug.Log("max health levelUp reached");
        }
    }

    public void ResetStats()
    {
        stats[StatType.maxHealth] = 100;
        stats[StatType.health] = 100;
        stats[StatType.Speed] = 10;
        stats[StatType.shootingSpeed] = 10;
        stats[StatType.damage] = 10;
        stats[StatType.crit] = 5; //este stat esta en porcentaje
        stats[StatType.critDamage] = 10; //este stat esta en porcentaje
    }


    //para bajarle la vida al jugador
    public void DamageTaken(int damage)
    {
        int healthReduction = GetStat(StatType.health) - damage;

        if (healthReduction < 0)
        {
            stats[StatType.health] = 0;
        } else {
            stats[StatType.health] = healthReduction;
        }
    }


    //para subir la vida
    public void RestoreHealth(int health)
    {
        int HealthSum = GetStat(StatType.health) + health;

        if (HealthSum > stats[StatType.maxHealth])
        {
            stats[StatType.health] = stats[StatType.maxHealth];
        } else {
            stats[StatType.health] = HealthSum;
        }
    }

    void Start()
    {
        ResetStats();
    }

}
