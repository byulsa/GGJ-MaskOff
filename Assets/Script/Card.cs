using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cost;
    public int x = -1;
    public int y = -1;
    public int coin;
    public int food;

    public Place place;

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
        GameManager.gameManager.AddRunActionToQueue(Run);
    }

    public void Select()
    {
        if (place == null)
        {
            Debug.Log($"선택됨{gameObject}");
            CardManager.cardManager.SelectingCard(gameObject);
            return;
        }

        Debug.Log($"실행됨{gameObject}");
        CardManager.cardManager.RunCard(place.x, place.y);
    }

    public void AddAct(IAct newAct)
    {
        acts.Add(newAct);
        acts = acts.OrderBy(a => actTypes.IndexOf(a.GetType())).ToList();
    }
}
