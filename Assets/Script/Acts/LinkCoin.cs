using UnityEngine;

public class LinkCoin : MonoBehaviour, IAct
{
    public TargetAmount targetAmount;

    public void Run(Card card)
    {
        GameManager.gameManager.Coin += targetAmount.amount;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}