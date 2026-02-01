using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ResourceType { Coin, Food, Water }

public class GoalManager : MonoBehaviour
{
    public static GoalManager instance;

    // Goal
    public string stageGoalName;
    public ResourceType currentResourceType;
    public int targetAmount;
    public bool isCompleted;

    [Header("UI")]
    public Text goalTextPrefab;
    public Transform goalContent;
    public Text currentGoalText; // 현재 주 목표 표시용
    public GameObject GameOverPanel;
    public GameObject GamewinPanel;

    private const int COIN_BASE = 10;
    private const int FOOD_BASE = 8;
    private const int WATER_BASE = 1;

    void Awake() => instance = this;

    void Start()
    {
        GenerateGoalsForWeek(GameManager.gameManager.currentWeek + 1);
    }

    // Week에 따라 목표 생성
    void GenerateGoalsForWeek(int weekNumber)
    {
        Debug.Log("Generating goals for week " + weekNumber);
        int coinTarget = (int)(COIN_BASE * Mathf.Pow(1.5f, weekNumber - 1));
        int foodTarget = (int)(FOOD_BASE * Mathf.Pow(1.5f, weekNumber - 1));
        int waterTarget = (int)(WATER_BASE * Mathf.Pow(1.5f, weekNumber - 1));

        int enumCount = System.Enum.GetValues(typeof(ResourceType)).Length;
        currentResourceType = (ResourceType)Random.Range(0, enumCount);

        if (currentResourceType == ResourceType.Coin)
        {
            stageGoalName = "코인 수집";
            targetAmount = coinTarget;
        }
        else if (currentResourceType == ResourceType.Food)
        {
            stageGoalName = "음식 수집";
            targetAmount = foodTarget;
        }
        else if (currentResourceType == ResourceType.Water)
        {
            stageGoalName = "물 수집";
            targetAmount = waterTarget;
        }

        isCompleted = false;

        UpdateCurrentGoalText();
    }

    // 7일차 끝났을 때 자원 수금
    public bool CollectResourcesAtWeekEnd()
    {
        int collectedAmount = 0;

        // 현재 자원에 따라 수금
        if (currentResourceType == ResourceType.Coin)
        {
            collectedAmount = GameManager.gameManager.Coin;
        }
        else if (currentResourceType == ResourceType.Food)
        {
            collectedAmount = GameManager.gameManager.Food;
        }
        else if (currentResourceType == ResourceType.Water)
        {
            collectedAmount = GameManager.gameManager.Water;
        }
        if (collectedAmount >= targetAmount)
        {
            isCompleted = true;
            
            // 목표 달성 시 자원 차감
            if (currentResourceType == ResourceType.Coin)
            {
                GameManager.gameManager.Coin -= targetAmount;
            }
            else if (currentResourceType == ResourceType.Food)
            {
                GameManager.gameManager.Food -= targetAmount;
            }
            else if (currentResourceType == ResourceType.Water)
            {
                GameManager.gameManager.Water -= targetAmount;
            }
            SFXManager.instance.PlaySFX(5);
            GenerateGoalsForWeek(GameManager.gameManager.currentWeek + 1);
            GamewinPanel.SetActive(true);
            return true;
        }
        else
        {
            GameOverPanel.SetActive(true);
            isCompleted = false;
            return false;
        }   
    }

    // 새로운 주 시작시 목표 재생성
    public void StartNewWeek(int weekNumber)
    {
        GenerateGoalsForWeek(weekNumber);
    }

    void UpdateCurrentGoalText()
    {
        if (currentGoalText == null) return;
        currentGoalText.text = $"목표: {stageGoalName} {targetAmount}개 모으기";
    }
}