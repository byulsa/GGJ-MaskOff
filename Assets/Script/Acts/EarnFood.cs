using UnityEngine;

public class EarnFood : MonoBehaviour, IAct
{
    public int amount;

    public bool Run(Card card)
    {
        // GameManager.gameManager.Food += amount;
        card.food += amount;
        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}