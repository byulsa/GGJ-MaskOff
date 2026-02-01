using UnityEngine;

public class CardHover : MonoBehaviour
{
    public bool isHovering = false;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private float hoverScaleMultiplier = 1.25f;
    private float lerpSpeed = 10f;

    void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        if (CardManager.cardManager != null && CardManager.cardManager.SelectCard == gameObject)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale * hoverScaleMultiplier, Time.deltaTime * lerpSpeed);
            return;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * lerpSpeed);
    }

    void OnMouseEnter()
    {
        targetScale = originalScale * hoverScaleMultiplier;
        isHovering = true;
    }

    void OnMouseExit()
    {
        targetScale = originalScale;
        isHovering = false;
    }
}