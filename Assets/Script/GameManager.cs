using System;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    PlayerTurn,
    Execution
}

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    
    [Header("자원")]
    public int Coin;
    public int Food;

    public Phase currentPhase = Phase.PlayerTurn;
    public List<Action> runQueue = new List<Action>();
    private float nextActionTime = 0f;

    // TODO : add game
    
    void Awake()
    {
        gameManager = this;
    }

    void Update()
    {
        if (currentPhase == Phase.Execution)
        {
            if (runQueue.Count > 0)
            {
                if (Time.time >= nextActionTime)
                {
                    runQueue[0]();
                    runQueue.RemoveAt(0);
                    nextActionTime = Time.time + 1f;
                }
            }
            else
            {
                currentPhase = Phase.PlayerTurn;
            }
        }
    }
    
    public void AddRunActionToQueue(Action action)
    {
        Debug.Log("Action added to run queue");
        runQueue.Add(action);
    }

    public void RunCards()
    {
        currentPhase = Phase.Execution;
    }
}
