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

        targetCard.card.coin.coin += card.coin.coin;
        targetCard.card.food.food += card.food.food;

        card.coin.coin = 0;
        card.food.food = 0;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}