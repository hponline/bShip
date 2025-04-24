using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float laneChangeSpeed; // Şerit değiştirme hızı
    public int desiredLane = 0; // Şerit 0 ortada demektir.
    int laneDistance = 53; // Yatay Şerit uzunlugu
  

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            desiredLane++;            
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            desiredLane--;            
        }
        desiredLane = Mathf.Clamp(desiredLane, -1, 1);
        
        Vector3 targetPosition = transform.position; // Hedef yol
        targetPosition.x = desiredLane * laneDistance;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);       
        transform.position += moveSpeed * Time.deltaTime * -transform.forward;       
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
