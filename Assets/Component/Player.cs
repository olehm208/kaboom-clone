using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Moving speed")]
    public float movingSpeed = 8f;
    [Header("Player's things")]
    public GameObject playform1;
    public GameObject playform2;
    public GameObject playform3;

    private readonly KeyCode[] konamiCode = new KeyCode[]
    {
        KeyCode.UpArrow, KeyCode.UpArrow,
        KeyCode.DownArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.B, KeyCode.A
    };

    private int index = 0;

    private Rigidbody2D rb;
    private float horizontalAxis;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
if (Input.anyKeyDown)
        {
            // Check if the pressed key matches the current step in the sequence
            if (Input.GetKeyDown(konamiCode[index]))
            {
                index++; // Advance to the next key

                // If the entire sequence matches, trigger the easter egg
                if (index >= konamiCode.Length)
                {
                    ActivateCheat();
                    index = 0; // Reset for next time
                }
            }
            else
            {
                // Wrong key pressed. Reset progress.
                index = 0;
                
                // Optional: Check if the wrong key was actually the FIRST key of the code 
                // so the player doesn't accidentally ruin a fresh attempt.
                if (Input.GetKeyDown(konamiCode[0]))
                {
                    index = 1;
                }
            }
        }   
    }

    private void ActivateCheat()
    {
        GameManager.Instance.konamiCode = true;
        SceneManager.LoadScene(2);
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalAxis * movingSpeed, rb.linearVelocityY);
    }
}
