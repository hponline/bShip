using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Rigidbody rb; // Geminin Rigidbody bile�eni
    public float waterLevel = 0f; // Su y�zeyinin y�ksekli�i
    public float buoyancyForce = 10f; // Kald�rma kuvveti
    public float damping = 1f; // Dalgalanma yumu�atma
    public float waveFrequency = 0.5f;
    public float waveAmplitude = 1f;

    private void FixedUpdate()
    {
        // **Dalgalanan su y�zeyini hesapla**
        float currentWaterLevel = waterLevel + Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;

        // **Nesnenin suya batma miktar�n� hesapla**
        float depth = currentWaterLevel - transform.position.y;

        if (depth > 0)
        {
            // **Kald�rma kuvvetini uygula**
            float forceAmount = buoyancyForce * depth - rb.velocity.y * damping;
            rb.AddForce(Vector3.up * forceAmount, ForceMode.Acceleration);
        }
    }
}

