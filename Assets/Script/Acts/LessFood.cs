using UnityEngine;

public class LessFood : MonoBehaviour, IAct
{
    public int amount;

    public bool Run(Card card)
    {
        if (card.food >= amount)
        {
            card.food -= amount;
            return true;
        }
        if (GameManager.gameManager.Food >= amount)
        {
            GameManager.gameManager.Food -= amount;
            return true;
        }
        return false;
    }

    public void UpdateCard(Card changedCard)
    {
        // No implementation needed for this act
    }
}