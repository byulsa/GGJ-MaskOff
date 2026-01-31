using UnityEngine;

public class CountMyCostCard : MonoBehaviour, IAct
{
    public TargetAmount targetAmount;

    public void Run(Card card)
    {
        foreach (Card c in CardManager.cardManager.GetAllCardsOnBoard())
        {
            if (c != null && c.cost == card.cost)
            {
                targetAmount.amount++;
            }
        }
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}