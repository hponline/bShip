using UnityEngine;

public class Coin : MonoBehaviour
{
    public float spinSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
