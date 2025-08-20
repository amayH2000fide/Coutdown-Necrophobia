using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController : MonoBehaviour
{

    public int incrementStats;
    public int incrementPercentageStats;
    public int MaxLevelUp;
    public Transform spawnPoint;

    public event Action<float> OnHealthPercentageChanged;
    public event Action<int, int> OnHealthchanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnExperienceChanged;

    public int experience = 0;
    public int baseXP = 100;
    public float xpMultiplier = 1.5f;

    public static PlayerStatController Instance { get; private set; }
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.maxHealth:
                return stats[StatType.maxHealth];
            case StatType.health:
                return stats[StatType.health];
            case StatType.level:
                return stats[StatType.level];
            case StatType.damage:
                return stats[StatType.damage];
            case StatType.crit:
                return stats[StatType.crit];
            case StatType.critDamage:
                return stats[StatType.critDamage];
            case StatType.shootingSpeed:
                return stats[StatType.shootingSpeed];
            case StatType.Speed:
                return stats[StatType.Speed];
            case StatType.MaxSpeed:
                return stats[StatType.MaxSpeed]; 
            default:
                return 0;
        }
    }

    public void addstats(StatType stat)
    {
        switch (stat)
        {
            case StatType.damage:
                stats[StatType.damage] += incrementStats;
                break;
            case StatType.Speed:
                stats[StatType.Speed] += incrementStats;
                stats[StatType.MaxSpeed] += 3;
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

    public void LevelUp()
    {
        OnLevelChanged?.Invoke(stats[StatType.level]);
        stats[StatType.level] += 1;

        if (stats[StatType.level] < MaxLevelUp) {
            float healthPercent = (float)stats[StatType.health] / stats[StatType.maxHealth];
            stats[StatType.maxHealth] = (int)(stats[StatType.maxHealth] * 1.5f);
            stats[StatType.health] = Mathf.RoundToInt(stats[StatType.maxHealth] * healthPercent);
        } else {
            Debug.Log("max health levelUp reached");
        }

        if (LevelUpManager.Instance != null)
        {
            LevelUpManager.Instance.ShowLevelUp();
        }

        OnHealthchanged?.Invoke(stats[StatType.health], stats[StatType.maxHealth]);
    }

    public void ResetStats()
    {
        stats[StatType.maxHealth] = 150;
        stats[StatType.MaxSpeed] = 10;
        stats[StatType.health] = 150;
        stats[StatType.Speed] = 20;
        stats[StatType.shootingSpeed] = 10;
        stats[StatType.damage] = 10;
        stats[StatType.crit] = 5; 
        stats[StatType.critDamage] = 10; 
        stats[StatType.level] = 1;
    }

    public void DamageTaken(int damage)
    {

        int healthReduction = GetStat(StatType.health) - damage;

        if (healthReduction < 0)
        {
            stats[StatType.health] = 0;
        } else {
            stats[StatType.health] = healthReduction;
        }

        OnHealthchanged?.Invoke(stats[StatType.health], stats[StatType.maxHealth]);
        Debug.Log("ataque de zombie vida a:" + healthReduction);
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
        OnHealthchanged?.Invoke(stats[StatType.health], stats[StatType.maxHealth]);
    }

    public int ExperienceToLevelUp
    {
        get
        {
            int currentLevel = stats[StatType.level];
            return Mathf.RoundToInt(baseXP * Mathf.Pow(xpMultiplier, currentLevel - 1));
        }
    }


    public void AddExperience(int amount)
    {
        experience += amount;
        OnExperienceChanged?.Invoke(experience);

        while (experience >= ExperienceToLevelUp)
        {
            experience -= ExperienceToLevelUp;
            LevelUp();
        }

        Console.WriteLine("experience now : " + experience);
        Console.WriteLine("experience till levelup: " + ExperienceToLevelUp);
    }

    void Start()
    {
        ResetStats();
        transform.position = spawnPoint.position;
    }
}
