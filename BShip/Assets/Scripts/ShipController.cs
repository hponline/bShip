using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Tekne Değişkenler")]
    public float maxMoveSpeed = 150f;
    public float minMoveSpeed = 50f;
    public float laneChangeSpeed = 90f;
    public float acceleration = 70f;
    public float deceleration = 100f;
    //public int desiredLane = 0; // Şerit 0 ortada demektir.
    readonly int laneDistance = 53; // Yatay Şerit uzunlugu
    float currentForwardSpeed;

    //Vector3 inputKey;
    Vector3 inputKeyHorizontal;
    //public float rotationSpeed = 10f;
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


        //// Dönüş
        //float speedFactor = currentForwardSpeed / maxMoveSpeed;
        //if (horizontalInput != 0f)
        //{
        //    Vector3 targetDirection = new Vector3(horizontalInput, 0f, 1f).normalized;
        //    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * speedFactor * Time.deltaTime);
        //}
        //else
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * speedFactor * Time.deltaTime);
        //}

        #region Eski Kod
        /*
         //transform.position += minMoveSpeed * Time.deltaTime * transform.forward;

        // Şerit geçiş
        //if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        //    desiredLane++;

        //if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        //    desiredLane--;
        //desiredLane = Mathf.Clamp(desiredLane, -1, 1);

        // İleri
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            //inputKey = new Vector3(0, 0, Input.GetAxis("Vertical"));
            //transform.position += maxMoveSpeed * Time.deltaTime * transform.forward;
        }

        // Şerit sınır
        //Vector3 targetPosition = transform.position; // Hedef yol
        //targetPosition.x = desiredLane * laneDistance;
        //transform.position = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);

         */
        #endregion
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
