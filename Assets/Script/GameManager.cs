using UnityEngine;
using System.Collections.Generic;
using System;

public enum Phase
{
    PlayerTurn,
    Execution
}

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public bool IsSelect = false;
    public GameObject SelectCard;
    public Transform GridParent;
    [SerializeField] private List<Place> PlaceList = new List<Place>();
    [Header("자원")]
    public int Coin;
    public int Food;

    public Phase currentPhase = Phase.PlayerTurn;
    public List<Action> runQueue = new List<Action>();
    private float nextActionTime = 0f;

    private Card[,] cards = new Card[3, 3];

    void Awake()
    {
        gameManager = this;
        InitializeGrid();
    }
    void InitializeGrid()
    {
        //PlaceList = GridParent.GetComponentsInChildren<Place>().ToList();

        for (int i = 0; i < PlaceList.Count; i++)
        {
            PlaceList[i].x = i % 3;
            PlaceList[i].y = i / 3;
            PlaceList[i].gameObject.tag = "Slot";
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log($"코인: {Coin}, 음식: {Food}");

        if (currentPhase == Phase.Execution)
        {
            if (runQueue.Count > 0)
            {
                if (Time.time >= nextActionTime)
                {
                    runQueue[0]();
                    runQueue.RemoveAt(0);
                    nextActionTime = Time.time + 1f;
                }
            }
            else
            {
                currentPhase = Phase.PlayerTurn;
            }
        }
        if (IsSelect && Input.GetMouseButtonDown(0))
        {
            TryPlaceCard();
        }
        //Debug.Log(cards[0, 0]);
    }

    public Card GetCard(int x, int y)
    {
        if (x < 0 || x >= 3 || y < 0 || y >= 3)
        {
            return null;
        }
        return cards[x, y];
    }

    public void SetCard(int x, int y, Card card)
    {
        if (x < 0 || x >= 3 || y < 0 || y >= 3)
        {
            return;
        }
        cards[x, y] = card;
    }

    public void RunCards()
    {
        currentPhase = Phase.Execution;
    }

    public void RunCard(int x, int y)
    {
        cards[x, y].Run();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if(cards[i, j] == null) continue;
                cards[i, j].UpdateCard(cards[x, y]);
            }
        }
    }
    void TryPlaceCard()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Slot"))
        {
            Place slot = hit.collider.GetComponent<Place>();

            // 2. 해당 칸이 비어있는지 확인 (GetCard 활용)
            if (GetCard(slot.x, slot.y) == null)
            {
                Card cardScript = SelectCard.GetComponent<Card>();

                // 3. 카드 데이터에 좌표 저장 및 배열에 등록 (SetCard 활용)
                cardScript.x = slot.x;
                cardScript.y = slot.y;
                SetCard(slot.x, slot.y, cardScript);

                // 4. 물리적 위치 이동 및 선택 상태 해제
                SelectCard.transform.position = hit.collider.transform.position;
                PlaceCard(); // IsSelect = false, SelectCard = null 처리

                Debug.Log($"보드 {slot.x}, {slot.y}에 배치 완료");
                
            }
            else
            {
                
            }
        }
        if (hit.collider != null && hit.collider.CompareTag("Slot"))
        {
            Place slot = hit.collider.GetComponent<Place>();
            RunCard(slot.x, slot.y);
            Debug.Log("asdf");
        }
    }

    public void SelectingCard(GameObject Obejct)
    {
        if (currentPhase != Phase.PlayerTurn) return;
        if (!IsSelect)
        {
            IsSelect = true;
            SelectCard = Obejct;
        }
    }

    public void PlaceCard()
    {
        if (currentPhase != Phase.PlayerTurn) return;
        if (IsSelect)
        {
            IsSelect = false;
        }
    }
}
