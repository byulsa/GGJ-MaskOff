using UnityEngine;

public class EarnWater : MonoBehaviour, IAct
{
    public int amount;

    public bool Run(Card card)
    {
        Debug.Log("EarnWater Act Triggered: +" + amount + " water");
        card.water += amount;

        return true; 
    }
    
    public void UpdateCard(Card changedCard)
    {
        // Do nothing
    }
}