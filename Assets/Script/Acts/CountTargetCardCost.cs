using UnityEngine;

public class CountTargetCardCost : MonoBehaviour, IAct
{
    public TargetCard targetCard;
    public TargetAmount targetAmount;

    public bool Run(Card card)
    {
        if (targetCard.card != null)
        {
            targetAmount.amount = targetCard.card.cost;

            return true;
        }

        return false;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}