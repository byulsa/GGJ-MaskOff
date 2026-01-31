using UnityEngine;

public class WhileCostDone : MonoBehaviour, IAct
{
    public void Run(Card card)
    {
        if (card.cost > 0)
        {
            GameManager.gameManager.AddRunActionToQueue(card.Run);
        }
    }

    public void UpdateCard(Card changedCard)
    {
        // No implementation needed for this act
    }
}