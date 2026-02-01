using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Shop shop;
    public CardStatusUI statusUI; // 상태창 UI 스크립트 연결

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // 0: 좌클릭, 1: 우클릭
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick(0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleMouseClick(1);
        }
    }

    void HandleMouseClick(int mouseButton)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject clickedObj = hit.collider.gameObject;

            // 1. 카드 클릭 처리
            if (hit.collider.CompareTag("Card"))
            {
                if (mouseButton == 0) // 좌클릭
                {
                    SFXManager.instance.PlaySFX(0);
                    if (shop.IsShopCard(clickedObj))
                    {
                        shop.BuyCard(clickedObj);
                    }
                    else
                    {
                        CardManager.cardManager.SelectingCard(clickedObj);
                    }
                }
            }
            // 2. 슬롯(Place) 클릭 처리
            else if (hit.collider.CompareTag("Slot"))
            {
                Place place = hit.collider.GetComponent<Place>();

                if (mouseButton == 0) // 좌클릭
                {
                    if (CardManager.cardManager.IsSelect)
                    {
                        CardManager.cardManager.TryPlaceCardOnSlot(place);
                    }
                    else if (GameManager.gameManager.currentPhase == Phase.PlayerTurn)
                    {
                        // 선택된 카드가 없을 때 슬롯 클릭 시 해당 위치 카드 실행
                        CardManager.cardManager.RunCard(place.x, place.y);
                    }
                }
                else if (mouseButton == 1) // 우클릭
                {
                    Card card = CardManager.cardManager.GetCard(place.x, place.y);
                    if (statusUI != null) statusUI.Open(card);
                }
            }
            // 3. 그 외 오브젝트 클릭 시 선택 해제
            else
            {
                if (mouseButton == 0 && CardManager.cardManager.IsSelect)
                {
                    CardManager.cardManager.DeselectCard();
                }
            }
        }
        // 4. 빈 공간 클릭 시 선택 해제
        else
        {
            if (mouseButton == 0 && CardManager.cardManager.IsSelect)
            {
                CardManager.cardManager.DeselectCard();
            }
        }
    }
}