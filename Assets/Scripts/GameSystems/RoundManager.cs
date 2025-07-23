using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    public float totalGameDuration = 1200f; // 20 minutes
    public float roundDuration = 60f;
    public float restDuration = 30f;

    private float gameTimer;
    private float phaseTimer;
    private bool inCombatPhase = true;

    public int currentRound = 1;

    public event Action<int> OnRoundStarted;
    public event Action OnRestStarted;
    public event Action OnGameEnded;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameTimer <= 0)
        {
            OnGameEnded?.Invoke();
            return;
        }

        gameTimer -= Time.deltaTime;
        phaseTimer -= Time.deltaTime;

        if (phaseTimer <= 0)
        {
            if (inCombatPhase)
            {
                StartRestPhase();
            }
            else
            {
                currentRound++;
                StartCombatPhase();
            }
        }
    }

    private void StartCombatPhase()
    {
        inCombatPhase = true;
        phaseTimer = roundDuration;

        Debug.Log($" Round {currentRound} started!");
        OnRoundStarted?.Invoke(currentRound);
    }

    private void StartRestPhase()
    {
        inCombatPhase = false;
        phaseTimer = restDuration;

        Debug.Log(" Rest phase started.");
        OnRestStarted?.Invoke();
    }
}
