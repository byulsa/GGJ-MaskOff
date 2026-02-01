using UnityEngine;

public class WhileCostDone : MonoBehaviour, IAct
{
    public bool Run(Card card)
    {
        if (card.cost > 0)
        {
            Debug.Log("WhileCostDone Act Triggered");
            card.AddRunActionToQueue();

            return true;
        }

        return false;
    }

    public void UpdateCard(Card changedCard)
    {
        // No implementation needed for this act
    }
}