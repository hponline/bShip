using UnityEngine;
using UnityEngine.UI;

public class boatTrigger : MonoBehaviour
{
    public GameObject boat;
    public Transform startPos;
    public Transform endPos;
    public float boatSpeed;
    bool deneme = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            deneme = true;
        }
    }
    private void Update()
    {
        if (deneme)
        {
            boat.transform.position = Vector3.MoveTowards(boat.transform.position, endPos.transform.position, boatSpeed * Time.deltaTime);
        }
        if (boat.transform.position == endPos.position)
        {
            deneme = false;
            boat.transform.position = startPos.position;
        }
    }
}
