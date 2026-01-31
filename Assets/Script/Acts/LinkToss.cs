using UnityEngine;

public class LinkToss : MonoBehaviour, IAct
{
    public TargetCard targetCard;

    public void Run(Card card)
    {
        if (targetCard.card == null)
        {
            return;
        }

        targetCard.card.coin += card.coin;
        targetCard.card.food += card.food;

        card.coin = 0;
        card.food = 0;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}