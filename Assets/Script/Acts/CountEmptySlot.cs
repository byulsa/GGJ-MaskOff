using UnityEngine;

public class CountEmptySlot : MonoBehaviour, IAct
{
    public TargetAmount targetAmount;

    public bool Run(Card card)
    {
        targetAmount.amount = 9 - CardManager.cardManager.GetAllCardsOnBoard().Count;
        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}