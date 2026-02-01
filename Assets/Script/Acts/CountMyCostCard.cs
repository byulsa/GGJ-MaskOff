using UnityEngine;

public class CountMyCostCard : MonoBehaviour, IAct
{
    public TargetAmount targetAmount;

    public bool Run(Card card)
    {
        foreach (Card c in CardManager.cardManager.GetAllCardsOnBoard())
        {
            if (c != null && c.cost == card.cost)
            {
                targetAmount.amount++;
            }
        }
        
        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}