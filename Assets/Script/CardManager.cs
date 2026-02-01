using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CardManager : MonoBehaviour
{
    public static CardManager cardManager;

    public bool IsSelect = false;

    public List<Card> handCards = new List<Card>();
    public GameObject SelectCard;

    public Card showInfoCard;

    [Header("Sorting")]
    public int baseSortingOrder = 0;
    public int selectedOrderOffset = 100;
    [SerializeField] private List<Place> PlaceList = new List<Place>();

    private Card[,] cards = new Card[3, 3];

    void Awake()
    {
        cardManager = this;
        InitializeGrid();
    }

    void InitializeGrid()
    {
        for (int i = 0; i < PlaceList.Count; i++)
        {
            PlaceList[i].x = i % 3;
            PlaceList[i].y = i / 3;
            PlaceList[i].gameObject.tag = "Slot";
        }
    }

    void Update()
    {
        UpdateHandPositions();
        UpdateCardsPositions();
    }

    void UpdateHandPositions()
    {
        if (handCards == null || handCards.Count == 0) return;

        for (int i = 0; i < handCards.Count; i++)
        {
            float xPos = i * 2.5f - (handCards.Count - 1) * 1.25f;

            Vector3 targetPosition = new Vector3(xPos, -4f, 0);
            SortingGroup sg = handCards[i].GetComponent<SortingGroup>();
            CardHover ch = handCards[i].GetComponent<CardHover>();
            bool isSelectedAndHovered = (handCards[i].gameObject == SelectCard) || (ch != null && ch.isHovering);

            if (isSelectedAndHovered)
            {
                // lift selected+hovered card slightly
                targetPosition.y += 0.5f;
            }

            if (sg != null)
            {
                int order = baseSortingOrder + i; // left -> smaller, right -> larger
                if (isSelectedAndHovered)
                {
                    order += selectedOrderOffset; // selected+hovered card goes on top
                }
                sg.sortingOrder = order;
            }

            handCards[i].transform.position = Vector3.Lerp(handCards[i].transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }

    void UpdateCardsPositions()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (cards[i, j] == null)
                {
                    continue;
                }
                if (cards[i, j].place == null)
                {
                    Debug.LogError($"Card at ({i}, {j}) has no assigned place.");
                    continue;
                }

                float xPos = cards[i, j].place.transform.position.x;
                float yPos = cards[i, j].place.transform.position.y;

                Vector3 targetPosition = new Vector3(xPos, yPos, 0);
                cards[i, j].transform.position = Vector3.Lerp(cards[i, j].transform.position, targetPosition, Time.deltaTime * 5f);
            }
        }
    }

    public Card GetCard(int x, int y)
    {
        SFXManager.instance.PlaySFX(1);
        if (x < 0 || x >= 3 || y < 0 || y >= 3)
        {
            return null;
        }
        return cards[x, y];
    }

    public List<Card> GetAllCardsOnBoard()
    {
        List<Card> cardList = new List<Card>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (cards[i, j] != null)
                {
                    cardList.Add(cards[i, j]);
                }
            }
        }
        return cardList;
    }

    public void SetCard(Place place, Card card)
    {
        int x = place.x;
        int y = place.y;

        if (x < 0 || x >= 3 || y < 0 || y >= 3)
        {
            return;
        }

        if (!handCards.Contains(card))
        {
            return;
        }

        handCards.Remove(card);
        card.x = x;
        card.y = y;
        card.place = place;

        card.coin = 0;
        card.food = 0;

        card.DisableCollider();

        cards[x, y] = card;
        //SFXManager.instance.PlaySFX(0);
    }

    public void AddCard(Card card)
    {
        handCards.Add(card);
    }

    public void RunCard(int x, int y)
    {
        if (cards[x, y] == null) return;

        Card card = cards[x, y];

        LessFood lessFood = card.GetComponent<LessFood>();
        LessCoin lessCoin = card.GetComponent<LessCoin>();

        if (lessFood != null && GameManager.gameManager.Food < lessFood.amount)
        {
            return;
        }
        if (lessCoin != null && GameManager.gameManager.Coin < lessCoin.amount)
        {
            return;
        }
        if (card.GetCurrentCost() <= 0)
        {
            return;
        }

        GameManager.gameManager.RunCards();

        GameManager.gameManager.AddRunActionToQueue(cards[x, y].Run);
    }

    public void UpdateCard(Card changedCard)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (cards[i, j] == null) continue;
                cards[i, j].UpdateCard(changedCard);
            }
        }
    }

    public void TryPlaceCardOnSlot(Place slot)
    {
        Card selectedCardScript = SelectCard.GetComponent<Card>();
        Card fieldCard = GetCard(slot.x, slot.y);

        if (selectedCardScript.unitType == UnitType.Mask)
        {
            if (fieldCard != null && fieldCard.unitType == UnitType.None)
            {
                if (fieldCard.isWear)
                {
                    Debug.Log("이미 마스크를 착용하고 있는 유닛입니다.");
                    DeselectCard();
                    return;
                }
                SFXManager.instance.PlaySFX(6);
                fieldCard.AddMask(selectedCardScript);
                fieldCard.Salefood = selectedCardScript.Salefood;
                handCards.Remove(selectedCardScript);
                Destroy(selectedCardScript.gameObject);

                Debug.Log($"{slot.x}, {slot.y} 카드가 마스크를 착용했습니다.");
            }
            else
            {
                Debug.Log("마스크를 착용할 수 없는 대상이거나 빈 칸입니다.");
            }
        }
        else if (selectedCardScript.unitType == UnitType.None)
        {
            if (fieldCard == null)
            {
                SetCard(slot, selectedCardScript);
                Debug.Log($"보드 {slot.x}, {slot.y}에 배치 완료");
            }
            else
            {
                Debug.Log("이미 카드가 있는 칸입니다.");
            }
        }

        DeselectCard();
    }

    public void SelectingCard(GameObject cardObject)
    {
        if (GameManager.gameManager.currentPhase != Phase.PlayerTurn) return;

        Card card = cardObject.GetComponent<Card>();
        if (!handCards.Contains(card)) return;

        if (IsSelect && SelectCard == cardObject)
        {
            DeselectCard();
        }
        else
        {
            IsSelect = true;
            SelectCard = cardObject;
        }
    }

    public void DeselectCard()
    {
        IsSelect = false;
        SelectCard = null;
    }
    public void AddCost()
    {
        Card card = showInfoCard.GetComponent<Card>();
        if (card == null) return;
        card.AddTotalCost(1);
    }

    // Allows assigning an int parameter from Unity's Button inspector
    public void AddCostBy(int amount)
    {
        if (GameManager.gameManager.Water <= 0) return;
        Card card = showInfoCard.GetComponent<Card>();
        if (card == null) return;
        GameManager.gameManager.Water -= amount;
        SFXManager.instance.PlaySFX(3);
        card.AddTotalCost(amount);
    }
    public void RemoveCard()
    {
        Card card = showInfoCard.GetComponent<Card>();
        if (card == null) return;
        int x = card.x;
        int y = card.y;
        if (x < 0 || x >= 3 || y < 0 || y >= 3) return;
        GameManager.gameManager.Coin += card.Salecoin / 2; // 카드에 있는 코인의 절반을 돌려줌
        if(card.Salefood > 0 && card.isWear)
        {
            GameManager.gameManager.Food += card.Salefood / 2; // 카드에 있는 음식의 절반을 돌려줌
        }
        cards[x, y] = null;
        SFXManager.instance.PlaySFX(4);
        Destroy(card.gameObject);
        InputManager.Instance.statusUI.gameObject.SetActive(false);
    }

    public bool HasCardsOnBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (cards[i, j] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void InitializeAllCardsCost()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Card card = cards[i, j];
                if (card != null)
                {
                    card.AddTotalCost(0);
                }
            }
        }
    }

    public Card GetRandomCardWithResources()
    {
        // 보드에 배치된 카드 중 자원을 가진 카드들을 찾기
        List<Card> cardsWithResources = new List<Card>();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Card card = cards[i, j];
                if (card != null && (card.coin > 0 || card.food > 0 || card.water > 0))
                {
                    cardsWithResources.Add(card);
                }
            }
        }

        // 자원을 가진 카드가 없으면 null 반환
        if (cardsWithResources.Count == 0)
        {
            return null;
        }

        // 랜덤하게 하나 선택해서 반환
        return cardsWithResources[Random.Range(0, cardsWithResources.Count)];
    }
}
