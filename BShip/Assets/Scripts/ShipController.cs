using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Tekne Değişkenler")]
    public float maxMoveSpeed = 150f;
    public float minMoveSpeed = 50f;
    public float laneChangeSpeed = 90f;
    public float acceleration = 70f;
    public float deceleration = 100f;
    public float rotationSpeed = 10f;
    public float NoseAngle = 1f;
    float currentForwardSpeed;

    public Transform boatNose;
    Vector3 inputKeyHorizontal;
    readonly int laneDistance = 53; // Yatay Şerit uzunlugu

    private void Start()
    {
        currentForwardSpeed = minMoveSpeed;
    }
    void Update()
    {
        Move();
    }

    public void Move()
    {   
        float targetSpeed = Input.GetKey(KeyCode.W) ? maxMoveSpeed : minMoveSpeed;
        float rate = (targetSpeed < currentForwardSpeed) ? currentForwardSpeed : acceleration;

        currentForwardSpeed = Mathf.MoveTowards(currentForwardSpeed, targetSpeed, rate * Time.deltaTime);
        transform.position += currentForwardSpeed * Time.deltaTime * transform.forward;

        // Yatay Move
        float horizontalInput = Input.GetAxis("Horizontal");
        inputKeyHorizontal = new Vector3(horizontalInput * laneChangeSpeed, 0f, 0f);
        Vector3 move = inputKeyHorizontal * Time.deltaTime;
        transform.position += move;

        // Yatay Sınır
        float maxX = laneDistance;
        float minX = -laneDistance;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;

        // Dönüş
        float speedFactor = currentForwardSpeed / maxMoveSpeed;
        if (horizontalInput != 0f)
        {
            Vector3 targetDirection = new Vector3(horizontalInput, 0f, NoseAngle).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * speedFactor * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * speedFactor * Time.deltaTime);
        }        
    }
    public float GetSpeed()
    {
        return currentForwardSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.gameManagerInstance.coinText.text = "Coin: " + CoinManager.coinManagerInstance.GetCoinCount();
            other.gameObject.SetActive(false);
            CoinManager.coinManagerInstance.AddCoin(1);
        }
        if (other.CompareTag("Obstacle"))
        {
            GameManager.gameManagerInstance.GameOver();
        }
    }
}
