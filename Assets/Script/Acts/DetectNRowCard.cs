using UnityEngine;

public class DetectNRowCard : MonoBehaviour, IAct
{
    public int row;

    Card card;

    public void Run(Card card)
    {
        this.card = card;
    }

    public void UpdateCard(Card changedCard)
    {
        if (changedCard.y == row)
        {
            GameManager.gameManager.AddRunActionToQueue(card.Run);
        }
    }
}