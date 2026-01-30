using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text CoinText;
    public Text FoodText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CoinText.text = GameManager.gameManager.Coin.ToString();
        FoodText.text = GameManager.gameManager.Food.ToString();
    }
}
