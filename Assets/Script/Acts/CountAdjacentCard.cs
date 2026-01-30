using UnityEngine;

public class CountAdjacentCard : MonoBehaviour, IAct
{
    public TargetAmount targetAmount;

    public void Run(Card card)
    {
        int[] xx = { -1, 1, 0, 0 };
        int[] yy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            Card adjacentCard = GameManager.gameManager.GetCard(card.x + xx[i], card.y + yy[i]);

            if (adjacentCard != null)
            {
                targetAmount.amount++;
            }
        }
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}