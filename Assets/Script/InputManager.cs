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
            GameObject cardObject = hit.collider.gameObject;

            if (hit.collider.CompareTag("Card"))
            {
                if (shop.IsShopCard(cardObject))
                {
                    shop.BuyCard(cardObject);
                }
                else
                {
                    CardManager.cardManager.SelectingCard(cardObject);
                }
            }
            else if (hit.collider.CompareTag("Slot"))
            {
                Place place = hit.collider.GetComponent<Place>();

                if (CardManager.cardManager.IsSelect)
                {
                    CardManager.cardManager.TryPlaceCardOnSlot(place);
                }
                else
                {
                    Debug.Log($"실행됨{gameObject}");
                    CardManager.cardManager.RunCard(place.x, place.y);
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
