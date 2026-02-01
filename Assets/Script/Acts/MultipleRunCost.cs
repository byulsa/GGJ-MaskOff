using UnityEngine;

public class MultipleRunCost : MonoBehaviour, IAct
{
    public bool Run(Card card)
    {
        if (card.GetCurrentCost() > 1)
        {
            return true;
        }

        foreach (Card otherCard in CardManager.cardManager.GetAllCardsOnBoard())
        {
            if (otherCard == card) continue;
            otherCard.SetCurrentCost(otherCard.GetCurrentCost() * 2);
        }

        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}