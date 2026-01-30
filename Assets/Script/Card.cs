using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cost;
    public int x;
    public int y;
    public Coin coin;
    public Food food;

    public List<IAct> acts = new List<IAct>();
    private readonly List<Type> actTypes = new List<Type>
    {
        typeof(TargetAmount),
        typeof(CountAdjacentCard),
        typeof(LessCoin),
        typeof(LessFood),
        typeof(LinkFood),
        typeof(LinkCoin),
        typeof(EarnFood),
        typeof(EarnCoin),
        typeof(WhileCostDone)
    };

    private void Awake()
    {
        acts = GetComponents<IAct>().OrderBy(a => actTypes.IndexOf(a.GetType())).ToList();
    }

    public void Run()
    {
        if (cost <= 0)
        {
            return;
        }

        foreach (var act in acts)
        {
            act.Run(this);
        }

        cost --;
    }

    public void UpdateCard(Card changedCard)
    {
        foreach (var act in acts)
        {
            act.UpdateCard(changedCard);
        }
    }

    public void AddRunActionToQueue()
    {
        GameManager.gameManager.runQueue.Add(Run);
    }

    public void Select()
    {
        Debug.Log($"선택됨{gameObject}");
        GameManager.gameManager.SelectingCard(gameObject);
    }

    public void AddAct(IAct newAct)
    {
        acts.Add(newAct);
        acts = acts.OrderBy(a => actTypes.IndexOf(a.GetType())).ToList();
    }
}
