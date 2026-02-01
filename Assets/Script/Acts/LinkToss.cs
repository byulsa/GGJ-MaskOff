using UnityEngine;

public class LinkToss : MonoBehaviour, IAct
{
    public TargetCard targetCard;

    public bool Run(Card card)
    {
        if (targetCard.card == null)
        {
            return false;
        }

        targetCard.card.coin += card.coin;
        targetCard.card.food += card.food;

        card.coin = 0;
        card.food = 0;

        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}