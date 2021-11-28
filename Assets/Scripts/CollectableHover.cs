using UnityEngine;
[ExecuteInEditMode]
public class CollectableHover : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;

    void Update()
    {
        if (Physics.CheckSphere(transform.position, 1, _groundMask))
        {
            transform.position += new Vector3(0, 1, 0);
        }
        else
        {
            transform.position -= new Vector3(0, 1, 0);
        }
    }
}