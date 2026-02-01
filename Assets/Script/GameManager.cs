using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum Phase
{
    PlayerTurn,
    Execution,
    Adjustment
}

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    [Header("Day")]
    public int currentDay = 1;
    public int currentWeek = 0;
    public Text dayText;
    public GameObject dayNextPanel;
    private Text DayNextText;

    [Header("자원")]
    public int Coin;
    public int Food;
    public int Water;

    public Phase currentPhase = Phase.PlayerTurn;
    public List<Action> runQueue = new List<Action>();
    private float nextActionTime = 0f;

    // TODO : add game

    void Awake()
    {
        gameManager = this;
    }
    void Start()
    {
        dayText.text = $"{currentWeek}주차 {currentDay}일차";
        DayNextText = dayNextPanel.GetComponentInChildren<Text>();
    }

    async void Update()
    {
        if (Time.time < nextActionTime)
        {
            return;
        }

        if (currentPhase != Phase.Execution)
        {
            return;
        }

        if (runQueue.Count > 0)
        {
            if (Time.time >= nextActionTime)
            {
                runQueue[0]();
                runQueue.RemoveAt(0);
                nextActionTime = Time.time + 0.01f;
            }
        }
        else
        {
            await StartNextDay();
        }
    }

    public void AddRunActionToQueue(Action action)
    {
        Debug.Log("Action added to run queue");
        runQueue.Add(action);
    }

    async Task StartNextDay()
    {
        currentPhase = Phase.Adjustment;

        // 자원 수급
        await RunAdjustment();

        currentPhase = Phase.PlayerTurn;

        // 7일차가 끝났을 때 자원 수금
        if (currentDay >= 7)
        {
            if (GoalManager.instance.CollectResourcesAtWeekEnd())
            {
                Debug.Log($"{currentWeek}주차로 넘어감");
                currentWeek++;
                currentDay = 0;
                // 새로운 주의 목표 생성
                GoalManager.instance.StartNewWeek(currentWeek);
            }
        }

        CardManager.cardManager.InitializeAllCardsCost();
        currentDay++;
        Shop.Instance.CardStore();
        Shop.Instance.ReRollCost = 2;
        Debug.Log($"{currentDay}일차 로 넘어감");
        dayText.text = $"{currentWeek}주차 {currentDay}일";
        DayNextText.text = dayText.text;
        dayNextPanel.SetActive(true);
    }

    public void RunCards()
    {
        currentPhase = Phase.Execution;
    }

    public async Task RunAdjustment()
    {
        float delay = 0.5f;  // 1초부터 시작
        const float minDelay = 0.01f;  // 최소 0.01초
        const float decreaseRate = 0.05f;  // 매번 0.05씩 감소

        foreach (Card card in CardManager.cardManager.GetAllCardsOnBoard())
        {
            card.Adjustment();
        }

        Card selectedCard;
        while ((selectedCard = CardManager.cardManager.GetRandomCardWithResources()) != null)
        {
            // 카드에서 가진 자원 중 하나 선택
            List<ResourceType> availableResources = new List<ResourceType>();
            if (selectedCard.coin > 0)
                availableResources.Add(ResourceType.Coin);
            if (selectedCard.food > 0)
                availableResources.Add(ResourceType.Food);
            if (selectedCard.water > 0)
                availableResources.Add(ResourceType.Water);

            if (availableResources.Count == 0) continue;  // 자원이 없으면 다시

            ResourceType chosenResource = availableResources[UnityEngine.Random.Range(0, availableResources.Count)];

            // Play Particle 함수 실행
            if (chosenResource == ResourceType.Coin)
            {
                selectedCard.PlayCoinParticle();
                selectedCard.coin--;
                Coin++;
            }
            else if (chosenResource == ResourceType.Food)
            {
                selectedCard.PlayFoodParticle();
                selectedCard.food--;
                Food++;
            }
            else if (chosenResource == ResourceType.Water)
            {
                selectedCard.PlayWaterParticle();
                selectedCard.water--;
                Water++;
            }

            // 대기
            await Task.Delay((int)(delay * 1000));

            // 지연 시간 감소
            delay -= decreaseRate;
            if (delay < minDelay)
                delay = minDelay;
        }
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
    public void GameRESET()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
