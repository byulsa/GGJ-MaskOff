using UnityEngine;

public class DetectSameMaskCard : MonoBehaviour, IAct
{
    public bool Run(Card card)
    {
        foreach (Card otherCard in CardManager.cardManager.GetAllCardsOnBoard())
        {
            if (otherCard != card && otherCard.MaskRend.sprite == card.MaskRend.sprite)
            {
                otherCard.AddRunActionToQueue();
                return true;
            }
        }

        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}