using UnityEngine;

public class EarnFood : MonoBehaviour, IAct
{
    public int amount;

    public void Run(Card card)
    {
        GameManager.gameManager.Food += amount;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}