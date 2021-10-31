using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[ExecuteAlways]
public class CollectableHover : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;

    void Update()
    {
        if (Physics.CheckSphere(transform.position, 1, _groundMask))
        {
            transform.position += new Vector3(0, 1, 0);
        }
    }
}