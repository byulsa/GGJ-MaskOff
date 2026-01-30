using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public bool IsSelect = false;
    public GameObject SelectCard;
    [Header("자원")]
    public int Coin;
    public int Food;
    void Awake()
    {
        gameManager = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SelectingCard(GameObject Obejct)
    {
        if (!IsSelect)
        {
            IsSelect = true;
            SelectCard = Obejct;
        }
    }
    public void PlaceCard()
    {
        if (IsSelect)
        {
            IsSelect = false;
        }
    }
}
