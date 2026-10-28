using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 8f;
    public float jumpForce = 5f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public TextMeshProUGUI timerText;
    public float timer = 30f;

    private float movementX;
    private float movementY;
    private int count;
    private Rigidbody rb;
    private bool gameEnded = false;
    private int lastShownSeconds = -1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Smooth rendering between physics steps, and stop the sphere tumbling against our facing.
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.freezeRotation = true;
        // Lock frames to the monitor refresh; uncapped frames arrive unevenly and look like stutter.
        QualitySettings.vSyncCount = 1;
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        EnemyAI.stopAllEnemies = false;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void Update()
    {
        // Checked before the gameEnded return so restart works on the win/lose screen.
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (gameEnded) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        timer -= Time.deltaTime;
        // Only rebuild the text when the shown second changes; doing it every frame makes garbage that causes hitches.
        int shownSeconds = Mathf.CeilToInt(timer);
        if (shownSeconds != lastShownSeconds)
        {
            lastShownSeconds = shownSeconds;
            timerText.text = "Time: " + shownSeconds;
        }

        if (timer <= 0)
        {
            timer = 0;
            EndGame(false);
        }
    }

    // Move through the Rigidbody in the physics step; moving the transform directly fights physics and stutters.
    void FixedUpdate()
    {
        if (gameEnded) return;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        Vector3 movement = (forward.normalized * movementY + right.normalized * movementX);

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        if (movement != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.LookRotation(movement));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameEnded) return;

        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }

        if (other.CompareTag("Enemy"))
        {
            EndGame(false);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 12)
        {
            EndGame(true);
        }
    }

    // Single exit point so win and lose can never both show.
    void EndGame(bool won)
    {
        if (gameEnded) return;
        gameEnded = true;
        (won ? winTextObject : loseTextObject).SetActive(true);
        EnemyAI.stopAllEnemies = true;
    }
}