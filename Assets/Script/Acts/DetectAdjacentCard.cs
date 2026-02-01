using UnityEngine;

public class DetectAdjacentCard : MonoBehaviour, IAct
{
    Card card;

    public void Awake()
    {
        card = GetComponent<Card>();
    }

    public void UpdateCard(Card changedCard)
    {
        int[] xx = { -1, 1, 0, 0 };
        int[] yy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            if (changedCard.x == card.x + xx[i] && changedCard.y == card.y + yy[i])
            {
                Debug.Log("Adjacent card detected");
                card.AddRunActionToQueue();
                return;
            }
        }
    }
}