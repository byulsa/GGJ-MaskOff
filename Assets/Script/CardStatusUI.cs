using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardStatusUI : MonoBehaviour
{
    public GameObject statusPanel;
    public Image unitImage;      // 유닛 기본 이미지
    public Image maskImage;      // 마스크 이미지 (따로 띄울 경우)

    public Text unitDescriptionText;

    public void Open(Card card)
    {
        if (card == null)
        {
            statusPanel.SetActive(false);
            CardManager.cardManager.showInfoCard = null;
            return;
        }

        CardManager.cardManager.showInfoCard = card;

        statusPanel.SetActive(true);

        // 1. 유닛 기본 외형 가져오기
        cardskin skin = card.GetComponent<cardskin>();
        if (card.isWear)
        {
            maskImage.gameObject.SetActive(true);
            maskImage.sprite = skin.Mask.sprite; // cardskin의 Mask(SpriteRenderer) 스프라이트 복사

            unitDescriptionText.text = card.maskDescription; // 마스크 설명으로 변경
        }
        else
        {
            maskImage.gameObject.SetActive(false); // 마스크 없으면 숨김
        }
    }
}