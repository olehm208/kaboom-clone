using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    [Header("Prefabs (Префаби)")]
    public GameObject normalPrefab;
    public GameObject valuablePrefab;
    public GameObject dangerousPrefab;

    [Header("Spawn Settings (Налаштування спавну)")]
    public float spawnWidth = 7f; 
    public float spawnY = 6f;     
    public float initialSpawnInterval = 1.5f;
    public float minSpawnInterval = 0.3f;
    public float intervalDecreaseRate = 0.05f;

    [Header("Speed Settings (Швидкість падіння)")]
    public float initialSpeed = 4f;
    public float maxSpeed = 12f;
    public float speedIncreaseRate = 0.2f;

    // Внутрішні змінні
    private float currentSpawnInterval;
    private float currentSpeed;
    private float nextSpawnTime;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        currentSpeed = initialSpeed;
        nextSpawnTime = Time.time + currentSpawnInterval;
    }

    void Update()
    {
        // Перевіряємо стан гри через GameManager
        if (GameManager.Instance == null || !GameManager.Instance.isSessionActive) return;

        // Логіка спавну
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            UpdateDifficulty();
            nextSpawnTime = Time.time + currentSpawnInterval;
        }
    }

    void SpawnObject()
    {
        float randomX = Random.Range(-spawnWidth, spawnWidth);
        Vector2 spawnPos = new Vector2(randomX, spawnY);

        GameObject prefabToSpawn = ChoosePrefab();
        GameObject spawnedObj = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        FallingItem item = spawnedObj.GetComponent<FallingItem>();
        if (item != null)
        {
            item.fallSpeed = currentSpeed;
        }

        // Реєструємо появу об'єкта в GameManager
        GameManager.Instance.objectsSpawned++;
    }

    GameObject ChoosePrefab()
    {
        float roll = Random.value;
        
        if(!GameManager.Instance.konamiCode)
        {
            if (roll < 0.25f) return dangerousPrefab; 
        }
        if (roll < 0.45f) return valuablePrefab;  
        return normalPrefab;                      
    }

    void UpdateDifficulty()
    {
        if (currentSpawnInterval > minSpawnInterval)
            currentSpawnInterval -= intervalDecreaseRate;

        if (currentSpeed < maxSpeed)
            currentSpeed += speedIncreaseRate;
    }
}