using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject CardPrefab;
    public List<GameObject> SalesCard; // 현재 진열된 카드들
    public List<Transform> SpawnPoints; // 카드가 스폰될 위치들 (에디터에서 설정)

    public List<GameObject> soldPanel; // 각 카드에 대응하는 SOLD 패널 리스트
    public Image[] SoldIcon;
    public Sprite coinNormal;
    public Sprite coinWear;
    bool IsStore = false;
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

    void HandleSelection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Card"))
        {
            BuyCard(hit.collider.gameObject);
        }
    }

    void BuyCard(GameObject cardObj)
    {
        
        int index = SalesCard.IndexOf(cardObj);
        if (index == -1) return;
        if (soldPanel[index].activeSelf) return;
        Card card = cardObj.GetComponent<Card>();
        cardskin skin = cardObj.GetComponent<cardskin>();

        if (skin.maskWear == MaskWear.Wear)
        {
            GameManager.gameManager.Food -= card.cost;
        }
        else
        {
            GameManager.gameManager.Coin -= card.cost;
        }
        cardObj.transform.position = new Vector3(0, -10, 0);
        card.Select();
        //card.enabled = true;
        soldPanel[index].SetActive(true);
    }

    public void CardStore()
    {
        if (IsStore) return;

        foreach (GameObject card in SalesCard) Destroy(card);
        SalesCard.Clear();

        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            GameObject newCard = Instantiate(CardPrefab, SpawnPoints[i].position, Quaternion.identity);
            cardskin skin = newCard.GetComponent<cardskin>();
            
            SalesCard.Add(newCard);

            soldPanel[i].SetActive(false);
            if (skin.maskWear == MaskWear.Wear)
            {
                Debug.Log("마스크");
                SoldIcon[i].sprite = coinWear;
            }
            else
            {
                Debug.Log("마스크 아님");
                SoldIcon[i].sprite = coinNormal;
            }
        }

        IsStore = false;
    }
}