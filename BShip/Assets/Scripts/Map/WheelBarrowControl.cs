using UnityEngine;

public class WheelBarrowControl : MonoBehaviour
{
    public Animator npc;
    public Animator wheelBarrow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npc.SetBool("isPush", true);
            wheelBarrow.SetBool("isDump", true);
        }        
    }
}
