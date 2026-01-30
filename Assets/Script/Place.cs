using UnityEngine;

public class Place : MonoBehaviour
{
    public bool IsPlace = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Placeing()
    {
        Debug.Log($"선택됨{gameObject}");
        if (IsPlace || !GameManager.gameManager.IsSelect)
        {
            return;
        }   
        GameManager.gameManager.SelectCard.transform.position = transform.position;
        GameManager.gameManager.PlaceCard();
    }
    
}
