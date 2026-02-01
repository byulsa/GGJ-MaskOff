using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework;

public class Shop : MonoBehaviour
{
    public static Shop Instance;
    [Header("카드 설정")]
    public List<GameObject> CardPool; // 모든 카드 프리팹 리스트
    public List<GameObject> SalesCard; // 현재 진열된 카드 인스턴스
    public List<Transform> SpawnPoints; // 카드 생성 위치

    [Header("UI 설정")]
    public List<GameObject> soldPanel;
    public Image[] SoldIcon;
    public List<Text> CostText;
    
    [Header("코인/음식 아이콘 설정")]

    public Sprite coinNormal;
    public Sprite coinWear;
    [Header("Lock UI 설정")]
    public Image lockButtonImage;
    public int ReRollCost;
    public bool IsLockStore;
    public Sprite[] IsLockIcon;

    private List<bool> cardLockStates; // 각 카드의 잠금 상태

    void Awake()
    {
        Instance = this;
        cardLockStates = new List<bool>();
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            cardLockStates.Add(false);
            SalesCard.Add(null);
        }
    }
    void Start()
    {
        CardStore();
    }

    void Update()
    {

    }

    public void RefreshStore()
    {
        if (ReRollCost >= 0)
        {
            ReRollCost--;
            CardStore();
        }
    }

    public void LockRefresh()
    {
        IsLockStore = !IsLockStore;
        lockButtonImage.sprite = IsLockStore ? IsLockIcon[1] : IsLockIcon[0];
    }

    public void CardStore()
    {
        SFXManager.instance.PlaySFX(7);
        // 1. 기존 카드 제거 (잠금 상태가 아닌 카드만)
        for (int i = 0; i < SalesCard.Count; i++)
        {
            GameObject card = SalesCard[i];
            
            // 잠금되지 않은 카드만 삭제
            if (!cardLockStates[i] && card != null && card.transform.parent == null)
            {
                Destroy(card);
                SalesCard[i] = null;
            }
        }

        // 2. 잠금되지 않은 자리에 새로운 카드 생성
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            // 잠금된 카드는 건너뛰기
            if (cardLockStates[i] && SalesCard[i] != null)
            {
                continue;
            }

            int randomIndex = Random.Range(0, CardPool.Count);
            GameObject newCard = Instantiate(CardPool[randomIndex], SpawnPoints[i].position, Quaternion.identity);

            Card card = newCard.GetComponent<Card>();
            SalesCard[i] = newCard;

            if (soldPanel[i].activeSelf)
            {
                soldPanel[i].SetActive(false);
                cardLockStates[i] = false; // 구매된 카드는 잠금 해제
            }

            // 마스크 여부에 따라 아이콘 설정
            if (card.unitType == UnitType.Mask)
            {
                SoldIcon[i].sprite = coinWear;
                CostText[i].text = card.food.ToString();
            }
            else
            {
                SoldIcon[i].sprite = coinNormal;
                CostText[i].text = card.coin.ToString();
            }
        }
    }

    public bool IsShopCard(GameObject cardObj)
    {
        return SalesCard.Contains(cardObj);
    }

    /// <summary>
    /// 카드를 잠금/해제하는 토글 함수
    /// </summary>
    public void ToggleLockCard(GameObject cardObj)
    {
        int index = SalesCard.IndexOf(cardObj);
        if (index == -1) return; // 샵의 카드가 아니면 무시

        cardLockStates[index] = !cardLockStates[index];
        SalesCard[index].GetComponent<Card>().ToogleLock();
    }

    /// <summary>
    /// 특정 인덱스의 카드가 잠금 상태인지 확인
    /// </summary>
    public bool IsCardLocked(GameObject cardObj)
    {
        int index = SalesCard.IndexOf(cardObj);
        if (index == -1) return false;
        return cardLockStates[index];
    }

    public void BuyCard(GameObject cardObj)
    {
        if (IsLockStore)
        {
            ToggleLockCard(cardObj);
            return;
        }

        int index = SalesCard.IndexOf(cardObj);
        if (index == -1 || soldPanel[index].activeSelf) return;

        Card card = cardObj.GetComponent<Card>();

        if (card.isLock)
        {
            Debug.Log(cardObj.name + " 카드는 잠금 상태입니다. 구매할 수 없습니다.");
            return;
        }

        if (card.unitType == UnitType.Mask)
        {
            if (GameManager.gameManager.Food < card.food) return;
            GameManager.gameManager.Food -= card.food;
        }
        else
        {
            if (GameManager.gameManager.Coin < card.coin) return;
            GameManager.gameManager.Coin -= card.coin;
        }

        CardManager.cardManager.AddCard(card);
        // cardObj.transform.position = new Vector3(0, -10, 0); 
        soldPanel[index].SetActive(true);
        SalesCard[index] = null; // 구매된 카드를 더 이상 판매하지 않도록 설정
        SFXManager.instance.PlaySFX(1);
        Debug.Log(cardObj.name + " 구매 완료. 인벤토리에 보관됨.");
    }
}