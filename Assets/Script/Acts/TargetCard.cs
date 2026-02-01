using UnityEngine;

public class TargetCard : MonoBehaviour, IAct
{
    public Card card;

    public bool Run(Card card)
    {
        return true;
    }

    public void UpdateCard(Card changedCard)
    {
    }
}