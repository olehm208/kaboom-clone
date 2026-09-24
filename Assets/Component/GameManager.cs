using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Session Rules (Правила гри)")]
    public float sessionDuration = 60f;
    public int maxHP = 1;

    [Header("Current State (Стан гри)")]
    public int score = 0;
    public int currentHP;
    public float sessionTimer;
    public int objectsSpawned = 0;
    public bool isSessionActive = false;
    public bool konamiCode = false;

    [Header("UI")]
    public GameObject GameOver;
    public GameObject YouWin;
    public TMP_Text scoreTxt;
    public TMP_Text timer;

    void Awake()
    {
        // Кожна сцена тепер матиме свій унікальний GameManager. 
        // Старий автоматично знищуватиметься при зміні сцени.
        Instance = this; 
    }

    void Start()
    {
        // Ховаємо екрани перемоги/поразки на початку сцени
        if (GameOver != null) GameOver.SetActive(false);
        if (YouWin != null) YouWin.SetActive(false);
        
        StartSession();
    }

    public void StartSession()
    {
        score = 0;
        currentHP = maxHP;
        sessionTimer = sessionDuration;
        objectsSpawned = 0;
        isSessionActive = true;
        
        Debug.Log("Session Started! Catch the items!");
    }

    void Update()
    {
        if (!isSessionActive) return;

        sessionTimer -= Time.deltaTime;
        
        if (sessionTimer <= 0)
        {
            EndSession("Time's up!");
        }
        
        if (score >= 500)
        {
            EndSession("You win!");
        }
        
        if (scoreTxt != null) scoreTxt.SetText($"Score: {score}");
        if (timer != null) timer.SetText($"Timer: {Math.Round(sessionTimer, 4)}");
    }

    public void ItemCaught(ItemType type)
    {
        if (!isSessionActive) return;

        switch (type)
        {
            case ItemType.Normal:
                score += 10;
                break;
            case ItemType.Valuable:
                score += 50;
                break;
            case ItemType.Dangerous:
                TakeDamage();
                break;
        }
        
        Debug.Log($"Score: {score} | HP: {currentHP}");
    }

    private void TakeDamage()
    {
        currentHP--;
        if (currentHP <= 0)
        {
            EndSession("Game Over: Lost all HP!");
        }
    }

    public void EndSession(string reason)
    {
        isSessionActive = false;
        Debug.Log($"--- SESSION ENDED ---");
        Debug.Log($"Reason: {reason}");
        
        if (reason == "You win!")
        {
            StartCoroutine(youWinCoroutine());
        }
        else
        {
            if (GameOver != null) GameOver.SetActive(true);
            StartCoroutine(youLoseCoroutine());
        }
    }
    
    private IEnumerator youWinCoroutine()
    {
        if (YouWin != null) YouWin.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(3); // Перехід на сцену з індексом 3
    }
    
    private IEnumerator youLoseCoroutine()
    {
        yield return new WaitForSeconds(3f);
        // Перезапускає поточну сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}