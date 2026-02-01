using UnityEngine;

public class DetectNRowCard : MonoBehaviour, IAct
{
    public int row;

    Card card;

    public void Awake()
    {
        card = GetComponent<Card>();
    }

    public void UpdateCard(Card changedCard)
    {
        if (changedCard.y == row)
        {
            Debug.Log("N Row card detected");
            card.AddRunActionToQueue();
        }
    }
}