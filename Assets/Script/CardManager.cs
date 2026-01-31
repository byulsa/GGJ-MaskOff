using UnityEngine;
using System.Collections.Generic;
using System;

public class CardManager : MonoBehaviour
{
    public static CardManager cardManager;

    public bool IsSelect = false;

    public List<Card> handCards = new List<Card>();
    public GameObject SelectCard;
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
        for (int i = 0; i < handCards.Count; i++)
        {
            if (SelectCard != null && handCards[i].gameObject == SelectCard)
            {
                continue;
            }

            float xPos = i * 2.5f - (handCards.Count - 1) * 1.25f;
            Vector3 targetPosition = new Vector3(xPos, -4f, 0);
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
        if (x < 0 || x >= 3 || y < 0 || y >= 3)
        {
            return null;
        }
        return cards[x, y];
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

        cards[x, y] = card;
    }

    public void AddCard(Card card)
    {
        handCards.Add(card);
    }
    
    public void RunCard(int x, int y)
    {
        if(cards[x, y] == null) return;
        GameManager.gameManager.AddRunActionToQueue(cards[x, y].Run);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if(cards[i, j] == null) continue;
                GameManager.gameManager.AddRunActionToQueue(() => cards[i, j].UpdateCard(cards[x, y]));
            }
        }
    }

    public void TryPlaceCardOnSlot(Place slot)
    {
        if (GetCard(slot.x, slot.y) == null)
        {
            Card cardScript = SelectCard.GetComponent<Card>();

            SetCard(slot, cardScript);
            DeselectCard();

            Debug.Log($"보드 {slot.x}, {slot.y}에 배치 완료");
        }
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
}
