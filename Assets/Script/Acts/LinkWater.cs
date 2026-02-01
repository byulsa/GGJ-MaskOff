using UnityEngine;

public class LinkWater : MonoBehaviour, IAct
{
    public TargetAmount targetAmount;

    public bool Run(Card card)
    {
        if (targetAmount.amount <= 0)
        {
            return false;
        }

        // GameManager.gameManager.Food += targetAmount.amount;
        card.water += targetAmount.amount;

        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}