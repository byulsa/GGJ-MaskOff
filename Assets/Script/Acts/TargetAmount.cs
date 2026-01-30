using UnityEngine;

public class TargetAmount : MonoBehaviour, IAct
{
    public int amount;

    public void Run(Card card)
    {
        amount = 0;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}