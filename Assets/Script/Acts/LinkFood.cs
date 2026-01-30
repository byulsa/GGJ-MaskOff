using UnityEngine;

public class LinkFood : MonoBehaviour, IAct
{
    public TargetAmount targetAmount;

    public void Run(Card card)
    {
        GameManager.gameManager.Food += targetAmount.amount;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}