using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public List<Text> CostText;

    public Sprite coinNormal;
    public Sprite coinWear;

    void Start()
    {
        CardStore();
    }

    void Update()
    {

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
            {
                Destroy(card);
            }
        }
        SalesCard.Clear();

        // 2. 랜덤 카드 생성
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            int randomIndex = Random.Range(0, CardPool.Count);
            GameObject newCard = Instantiate(CardPool[randomIndex], SpawnPoints[i].position, Quaternion.identity);
            
            Card card = newCard.GetComponent<Card>();
            cardskin skin = newCard.GetComponent<cardskin>();
            SalesCard.Add(newCard);

            soldPanel[i].SetActive(false);

            // 마스크 여부에 따라 아이콘 설정
            if (skin.maskWear == MaskWear.Wear)
            {
                SoldIcon[i].sprite = coinWear;
                CostText[i].text = card.coin.ToString();
            }
            else
            {
                SoldIcon[i].sprite = coinNormal;
                CostText[i].text = card.food.ToString();
            }
        }
    }

    public bool IsShopCard(GameObject cardObj)
    {
        return SalesCard.Contains(cardObj);
    }

    public void BuyCard(GameObject cardObj)
    {
        int index = SalesCard.IndexOf(cardObj);
        if (index == -1 || soldPanel[index].activeSelf) return;

        Card card = cardObj.GetComponent<Card>();
        cardskin skin = cardObj.GetComponent<cardskin>();

        if (skin.maskWear == MaskWear.Wear)
        {
            if(GameManager.gameManager.Food < card.food) return;
            GameManager.gameManager.Food -= card.food;
        }
        else
        {
            if(GameManager.gameManager.Coin < card.coin) return;
            GameManager.gameManager.Coin -= card.coin;
        }

        CardManager.cardManager.AddCard(card);
        // cardObj.transform.position = new Vector3(0, -10, 0); 
        soldPanel[index].SetActive(true);
        SalesCard[index] = null; // 구매된 카드를 더 이상 판매하지 않도록 설정
        
        Debug.Log(cardObj.name + " 구매 완료. 인벤토리에 보관됨.");
    }
}