using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ShapeData
{
    public int shapeID;       // Unique ID for this shape (assign in Inspector)
    public int cost;          // Money required to spawn this shape
    public int redReward;     // Money gained when red & synced
    public int scoreReward;   // Score gained when red & synced
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text moneyText;
    public TMP_Text timerText;
    public GameObject gameOverPanel;

    [Header("Floating Text")]
    public GameObject floatingTextPrefab;

    [Header("Gameplay Settings")]
    public int startMoney = 5;
    public float gameDuration = 30f; // seconds
    [Header("Gameplay Limits")]
    public int maxSpheres = 10;   // max total spheres allowed at once

    [Header("Shape Settings")]
    public ShapeData[] shapes; // assign in Inspector

    private int score = 0;
    private int money;
    private int lastScoredSecond = -1;
    private float timeRemaining;
    private bool isGameOver = false;

    void Awake()
    {
        Application.targetFrameRate = 60; // prevents GPU from going 100% in Play Mode
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        money = startMoney;
        timeRemaining = gameDuration;
        UpdateScoreUI();
        UpdateMoneyUI();
        UpdateTimerUI();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        // ⏱ Countdown
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            GameOver();
        }
        UpdateTimerUI();

        // 🟥 Red shape scoring (once per second)
        int currentSecond = Mathf.FloorToInt(Time.time);

        if (currentSecond != lastScoredSecond)
        {
            lastScoredSecond = currentSecond;

            PlantableLifetime[] spheres = Object.FindObjectsByType<PlantableLifetime>(FindObjectsSortMode.None);

            int totalRedCount = 0;
            int totalScore = 0;
            int totalMoney = 0;

            foreach (var s in spheres)
            {
                if (!s.IsRed()) continue;

                totalRedCount++;

                ShapeData shape = GetShapeData(s.ShapeID);
                if (shape != null)
                {
                    totalScore += shape.scoreReward;
                    totalMoney += shape.redReward;
                }
            }

            if (totalRedCount >= 2) // only reward if at least 2 are red
            {
                AddScore(totalScore);
                AddMoney(totalMoney, Vector3.zero);
            }
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    // 🌱 Check if player can spawn another shape
    public bool CanSpawnSphere()
    {
        PlantableLifetime[] spheres = Object.FindObjectsByType<PlantableLifetime>(FindObjectsSortMode.None);
        return spheres.Length < maxSpheres;
    }

    // 💰 Attempt to pay for a shape
    public bool TrySpendMoney(int amount, Vector3 position)
    {
        if (isGameOver) return false;

        if (money >= amount)
        {
            money -= amount;
            UpdateMoneyUI();
            ShowFloatingText("-$" + amount, position, Color.red);
            return true;
        }
        else
        {
            Debug.Log("Not enough money!");
            return false;
        }
    }


    // 💵 Rewards
    public void AddMoney(int amount, Vector3 position)
    {
        if (isGameOver) return;

        money += amount;
        UpdateMoneyUI();
        ShowFloatingText("+$" + amount, position, Color.green);
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    // 🔎 Look up shape data
    ShapeData GetShapeData(int shapeID)
    {
        foreach (var s in shapes)
        {
            if (s.shapeID == shapeID)
                return s;
        }
        return null;
    }

    // 🖥 UI
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "$" + money;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
    }

    // ✨ Floating feedback
    void ShowFloatingText(string text, Vector3 position, Color color)
    {
        if (floatingTextPrefab == null) return;

        GameObject go = Instantiate(floatingTextPrefab, position, Quaternion.identity);
        TMP_Text tmp = go.GetComponent<TMP_Text>();
        if (tmp != null)
        {
            tmp.text = text;
            tmp.color = color;
        }

        FloatingText floatScript = go.GetComponent<FloatingText>();
        if (floatScript != null) floatScript.StartFloating();
    }
}
