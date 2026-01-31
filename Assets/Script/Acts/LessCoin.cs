using UnityEngine;

public class LessCoin : MonoBehaviour, IAct
{
    public int amount;

    public void Run(Card card)
    {
        if (card.coin >= amount)
        {
            card.coin = Mathf.Max(0, card.coin - amount);
            return;
        }
        GameManager.gameManager.Coin = Mathf.Max(0, GameManager.gameManager.Coin - amount);
    }

    public void UpdateCard(Card changedCard)
    {
        // No implementation needed for this act
    }
}