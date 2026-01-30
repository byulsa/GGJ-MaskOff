using UnityEngine;
using UnityEngine.EventSystems;

public class CardSold : MonoBehaviour, IPointerClickHandler
{
    public GameObject soldPanel;
    public GameObject coinIconNormal;
    public GameObject coinIconSpecial; // MaskWear.Wear일 때 보일 아이콘
    
    private cardskin skinData;
    private bool isSold = false;

    void Start()
    {
        skinData = GetComponent<cardskin>();
        UpdateCoinIcon();
    }

    void UpdateCoinIcon()
    {
        // Enum 상태에 따라 아이콘 활성화/비활성화
        bool isWearing = (skinData.maskWear == MaskWear.Wear);
        coinIconNormal.SetActive(!isWearing);
        coinIconSpecial.SetActive(isWearing);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSold) return;

        Purchase();
    }

    void Purchase()
    {
        isSold = true;
        if (soldPanel != null) soldPanel.SetActive(true);
        
        Debug.Log(gameObject.name + " 구매 완료!");
    }
}
