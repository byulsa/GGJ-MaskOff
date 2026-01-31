using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Shop shop;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    void HandleMouseClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Card"))
            {
                GameObject cardObject = hit.collider.gameObject;
                if (shop.IsShopCard(cardObject))
                {
                    shop.BuyCard(cardObject);
                }
                else
                {
                    // This handles selecting cards on the board or in hand (if they have colliders)
                    cardObject.GetComponent<Card>().Select();
                }
            }
            else if (hit.collider.CompareTag("Slot"))
            {
                if (CardManager.cardManager.IsSelect)
                {
                    Place place = hit.collider.GetComponent<Place>();
                    CardManager.cardManager.TryPlaceCardOnSlot(place);
                }
            }
            else
            {
                // Clicked on something else, deselect card
                if (CardManager.cardManager.IsSelect)
                {
                    CardManager.cardManager.DeselectCard();
                }
            }
        }
        else
        {
            // Clicked on empty space, deselect card
            if (CardManager.cardManager.IsSelect)
            {
                CardManager.cardManager.DeselectCard();
            }
        }
    }
}
