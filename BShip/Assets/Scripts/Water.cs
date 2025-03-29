using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Rigidbody rb; // Geminin Rigidbody bileþeni
    public float waterLevel = 0f; // Su yüzeyinin yüksekliði
    public float buoyancyForce = 10f; // Kaldýrma kuvveti
    public float damping = 1f; // Dalgalanma yumuþatma
    public float waveFrequency = 0.5f;
    public float waveAmplitude = 1f;

    private void FixedUpdate()
    {
        // **Dalgalanan su yüzeyini hesapla**
        float currentWaterLevel = waterLevel + Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;

        // **Nesnenin suya batma miktarýný hesapla**
        float depth = currentWaterLevel - transform.position.y;

        if (depth > 0)
        {
            // **Kaldýrma kuvvetini uygula**
            float forceAmount = buoyancyForce * depth - rb.velocity.y * damping;
            rb.AddForce(Vector3.up * forceAmount, ForceMode.Acceleration);
        }
    }
}

