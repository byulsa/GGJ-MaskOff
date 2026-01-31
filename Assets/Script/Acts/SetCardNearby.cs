using UnityEngine;

public class SetCardNearby : MonoBehaviour, IAct
{
    public Vector2Int offset;
    public TargetCard targetCard;

    public void Run(Card card)
    {
        targetCard.card = CardManager.cardManager.GetCard(card.x + offset.x, card.y + offset.y);
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}