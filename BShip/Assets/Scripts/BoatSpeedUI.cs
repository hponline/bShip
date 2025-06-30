using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoatSpeedUI : MonoBehaviour
{
    public TextMeshProUGUI boatSpeed;
    public ShipController boatScript;
    public Image fillImage;
    private void Update()
    {
        float currentSpeed = boatScript.GetSpeed();
        float fillAmount = Mathf.Clamp01(currentSpeed/150f);
        fillImage.fillAmount = fillAmount;
        boatSpeed.text = Mathf.RoundToInt(currentSpeed).ToString();
    }
}
