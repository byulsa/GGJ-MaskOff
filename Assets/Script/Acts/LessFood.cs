using UnityEngine;

public class LessFood : MonoBehaviour, IAct
{
    public int amount;

    public void Run(Card card)
    {
        if (card.food != null && card.food.food >= amount)
        {
            card.food.food = Mathf.Max(0, card.food.food - amount);
            return;
        }
        GameManager.gameManager.Food = Mathf.Max(0, GameManager.gameManager.Food - amount);
    }

    public void UpdateCard(Card changedCard)
    {
        // No implementation needed for this act
    }
}