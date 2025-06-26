using UnityEngine;

public class TrashResetter : MonoBehaviour
{
    Rigidbody[] trashRb;
    Vector3[] firstPos;
    Quaternion[] firstRot;

    private void Awake()
    {
        trashRb = GetComponentsInChildren<Rigidbody>();
        firstPos = new Vector3[trashRb.Length];
        firstRot = new Quaternion[trashRb.Length];

        for (int i = 0; i < trashRb.Length; i++)
        {
            firstPos[i] = trashRb[i].transform.localPosition;
            firstRot[i] = trashRb[i].transform.localRotation;
        }
    }

    public void ResetTrash()
    {
        for (int i = 0; i < trashRb.Length; i++)
        {
            trashRb[i].velocity = Vector3.zero;
            trashRb[i].angularVelocity = Vector3.zero;
            trashRb[i].transform.SetLocalPositionAndRotation(firstPos[i], firstRot[i]);
        }
    }

    private void OnEnable()
    {
        ResetTrash();
    }

    private void OnDisable()
    {
        ResetTrash();
    }
}
