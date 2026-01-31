using UnityEngine;

public class CountNCostCard : MonoBehaviour, IAct
{
    public TargetAmount targetAmount;
    public int costN;

    public void Run(Card card)
    {
        foreach (Card c in CardManager.cardManager.GetAllCardsOnBoard())
        {
            if (c != null && c.cost == costN)
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