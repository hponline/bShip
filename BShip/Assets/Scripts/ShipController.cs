using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float moveSpeed = 100f;
    public float minMoveSpeed = 50f;
    public float laneChangeSpeed; // Şerit değiştirme hızı
    public int desiredLane = 0; // Şerit 0 ortada demektir.
    int laneDistance = 53; // Yatay Şerit uzunlugu

    Vector3 inputKey;
    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.position += minMoveSpeed * Time.deltaTime * transform.forward;

        // Şerit geçiş
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            desiredLane++;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            desiredLane--;
        desiredLane = Mathf.Clamp(desiredLane, -1, 1);

        // İleri
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            inputKey = new Vector3(0, 0, Input.GetAxis("Vertical"));
            transform.position += moveSpeed * Time.deltaTime * inputKey;
        }

        // Şerit sınır
        Vector3 targetPosition = transform.position; // Hedef yol
        targetPosition.x = desiredLane * laneDistance;
        transform.position = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);

        float maxX = laneDistance;
        float minX = -laneDistance;
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            GameManager.gameManagerInstance.coinText.text = "Coin: " + CoinManager.coinManagerInstance.GetCoinCount();
            other.gameObject.SetActive(false);
            CoinManager.coinManagerInstance.AddCoin(1);
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.gameManagerInstance.GameOver();
        }

    }
}
