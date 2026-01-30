using UnityEngine;
using System.Collections.Generic;
using System;

public enum Phase
{
    PlayerTurn,
    Execution
}

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public bool IsSelect = false;
    public GameObject SelectCard;
    [Header("자원")]
    public int Coin;
    public int Food;

    public Phase currentPhase = Phase.PlayerTurn;
    public List<Action> runQueue = new List<Action>();
    private float nextActionTime = 0f;

    private Card[,] cards = new Card[3, 3];

    void Awake()
    {
        gameManager = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"코인: {Coin}, 음식: {Food}");

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

    public Card GetCard(int x, int y)
    {
        if (x < 0 || x >= 3 || y < 0 || y >= 3)
        {
            return null;
        }
        return cards[x, y];
    }

    public void SetCard(int x, int y, Card card)
    {
        if (x < 0 || x >= 3 || y < 0 || y >= 3)
        {
            return;
        }
        cards[x, y] = card;
    }

    public void RunCards()
    {
        currentPhase = Phase.Execution;
    }

    public void RunCard(int x, int y)
    {
        cards[x, y].Run();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cards[i, j].UpdateCard(cards[x, y]);
            }
        }
    }

    public void SelectingCard(GameObject Obejct)
    {
        if (currentPhase != Phase.PlayerTurn) return;
        if (!IsSelect)
        {
            IsSelect = true;
            SelectCard = Obejct;
        }
    }

    public void PlaceCard()
    {
        if (currentPhase != Phase.PlayerTurn) return;
        if (IsSelect)
        {
            IsSelect = false;
        }
    }
}
