using UnityEngine;

public class SetCardUp : MonoBehaviour, IAct
{
    public TargetCard targetCard;

    public void Run(Card card)
    {
        targetCard.card = CardManager.cardManager.GetCard(card.x, card.y - 1);
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}