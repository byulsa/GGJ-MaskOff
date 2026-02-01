using UnityEngine;

public class MultipleResources : MonoBehaviour, IAct
{
    public bool Run(Card card)
    {
        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }

    public void Adjustment(Card card)
    {
        card.coin *= 2;
        card.food *= 2;
        card.water *= 2;
    }
}