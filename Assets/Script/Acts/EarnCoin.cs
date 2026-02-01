using UnityEngine;

public class EarnCoin : MonoBehaviour, IAct
{
    public int amount;

    public bool Run(Card card)
    {
        Debug.Log("EarnCoin Act Triggered: +" + amount + " coins");
        // GameManager.gameManager.Coin += amount;
        card.coin += amount;

        return true; 
    }
    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}