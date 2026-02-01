using UnityEngine;

public class LessCoin : MonoBehaviour, IAct
{
    public int amount;

    public bool Run(Card card)
    {
        if (card.coin >= amount)
        {
            card.coin -= amount;
            return true;
        }
        if (GameManager.gameManager.Coin >= amount)
        {
            GameManager.gameManager.Coin -= amount;
            return true;
        }
        return false; // 부족할 때만 false
    }

    public void UpdateCard(Card changedCard)
    {
        // No implementation needed for this act
    }
}