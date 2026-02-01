using UnityEngine;

public class TargetAmount : MonoBehaviour, IAct
{
    public int amount;

    public bool Run(Card card)
    {
        amount = 0;

        return true;
    }

    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}