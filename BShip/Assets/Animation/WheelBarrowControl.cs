using UnityEngine;

public class WheelBarrowControl : MonoBehaviour
{
    public Animator npc;
    public Animator wheelBarrow;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Animasyon oynatýldý çöp dökme");

            npc.SetBool("isPush", true);
            wheelBarrow.SetBool("isDump", true);
        }
        
    }
}
