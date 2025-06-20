using System.Collections.Generic;
using UnityEngine;

public class SplashTrigger : MonoBehaviour
{
    public GameObject splashEffekt;
    HashSet<GameObject> splashedObject = new(); // HashSet sadece o ogeden 1 tane saklar

    private void OnTriggerEnter(Collider other)
    {
        if (!splashedObject.Contains(other.gameObject))
        {
            Instantiate(splashEffekt, other.transform.transform.position, Quaternion.identity);
            splashedObject.Add(other.gameObject);
        }
    }
}
