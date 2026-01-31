using UnityEngine;

public class CountTargetCardCost : MonoBehaviour, IAct
{
    public TargetCard targetCard;
    public TargetAmount targetAmount;

    public void Run(Card card)
    {
        if (targetCard.card != null)
        {
            targetAmount.amount = targetCard.card.cost;
        }
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}