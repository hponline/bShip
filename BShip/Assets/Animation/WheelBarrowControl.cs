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
            Debug.Log("Animasyon oynat�ld� ��p d�kme");

            npc.SetBool("isPush", true);
            wheelBarrow.SetBool("isDump", true);
        }
        
    }
}
