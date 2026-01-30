using UnityEngine;

public class EarnCoin : MonoBehaviour, IAct
{
    public int amount;

    public void Run(Card card)
    {
        GameManager.gameManager.Coin += amount;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}