using UnityEngine;

public class LinkCoin : MonoBehaviour, IAct
{
    public TargetAmount targetAmount;

    public bool Run(Card card)
    {
        // GameManager.gameManager.Coin += targetAmount.amount;
        card.coin += targetAmount.amount;

        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}