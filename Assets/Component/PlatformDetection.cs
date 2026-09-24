using UnityEngine;

public class PlatformDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            Destroy(gameObject);
            GameManager.Instance.score -= 1;
        }
        if (other.tag == "Good")
        {
            GameManager.Instance.score += 1;
        }
        if (other.tag == "Better")
        {
            GameManager.Instance.score += 3;
        }
    }
}
