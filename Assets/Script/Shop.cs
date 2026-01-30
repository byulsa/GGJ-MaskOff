using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public bool IsLockStore;
    [Header("카드 설정")]
    public List<GameObject> CardPool; // 모든 카드 프리팹 리스트
    public List<GameObject> SalesCard; // 현재 진열된 카드 인스턴스
    public List<Transform> SpawnPoints; // 카드 생성 위치

    [Header("UI 설정")]
    public List<GameObject> soldPanel;
    public Image[] SoldIcon;
    public Sprite coinNormal;
    public Sprite coinWear;

    void Start()
    {
        CardStore();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
    }

    public void RefreshStore()
    {
        CardStore();
    }

    public void CardStore()
    {
        // 1. 기존 카드 제거
        foreach (GameObject card in SalesCard)
        {
            if (card != null && card.transform.parent == null) // 배정 안 된 카드만 삭제
                Destroy(card);
        }
        SalesCard.Clear();

        // 2. 랜덤 카드 생성
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            int randomIndex = Random.Range(0, CardPool.Count);
            GameObject newCard = Instantiate(CardPool[randomIndex], SpawnPoints[i].position, Quaternion.identity);
            
            cardskin skin = newCard.GetComponent<cardskin>();
            SalesCard.Add(newCard);

            soldPanel[i].SetActive(false);

            // 마스크 여부에 따라 아이콘 설정
            if (skin.maskWear == MaskWear.Wear)
                SoldIcon[i].sprite = coinWear;
            else
                SoldIcon[i].sprite = coinNormal;
        }
    }

    void HandleSelection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Card") && SalesCard.Contains(hit.collider.gameObject))
            {
                BuyCard(hit.collider.gameObject);
            }
            else if (hit.collider.CompareTag("Card") && !SalesCard.Contains(hit.collider.gameObject))
            {
                hit.collider.GetComponent<Card>().Select();
            }
            else if (hit.collider.CompareTag("Slot") && GameManager.gameManager.IsSelect)
            {
                TryPlaceCard(hit.collider.gameObject);
            }
        }
    }

    void BuyCard(GameObject cardObj)
    {
        int index = SalesCard.IndexOf(cardObj);
        if (index == -1 || soldPanel[index].activeSelf) return;

        Card card = cardObj.GetComponent<Card>();
        cardskin skin = cardObj.GetComponent<cardskin>();

        if (skin.maskWear == MaskWear.Wear)
            GameManager.gameManager.Food -= card.cost;
        else
            GameManager.gameManager.Coin -= card.cost;

        cardObj.transform.position = new Vector3(0, -10, 0); 
        soldPanel[index].SetActive(true);
        
        Debug.Log(cardObj.name + " 구매 완료. 인벤토리에 보관됨.");
    }

    void TryPlaceCard(GameObject slotObj)
    {
        Place targetPlace = slotObj.GetComponent<Place>();
        if (targetPlace == null) return;
        if (GameManager.gameManager.GetCard(targetPlace.x, targetPlace.y) == null)
        {
            GameObject selectedCard = GameManager.gameManager.SelectCard;
            Card cardScript = selectedCard.GetComponent<Card>();
            cardScript.x = targetPlace.x;
            cardScript.y = targetPlace.y;
            GameManager.gameManager.SetCard(targetPlace.x, targetPlace.y, cardScript);
            selectedCard.transform.position = targetPlace.transform.position;
            GameManager.gameManager.PlaceCard(); // 선택 해제
        }
    }
}