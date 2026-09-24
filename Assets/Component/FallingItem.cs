using UnityEngine;

public enum ItemType { Normal, Valuable, Dangerous }

public class FallingItem : MonoBehaviour
{
    public ItemType type;
    [HideInInspector] public float fallSpeed;
    [HideInInspector] public AreaSpawner spawner;

    void Update()
    {
        // Рух вниз без використання фізики (ідеально для Kaboom)
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Знищення об'єкта, якщо він впав за межі екрану
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Перевіряємо, чи зіткнулися з гравцем
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ItemCaught(type);
            Destroy(gameObject);
        }
    }
}