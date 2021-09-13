using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CollectableA : MonoBehaviour
{
    // Start is called before the first frame update
    public static event Action<string> OnPlayerCollect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
            OnPlayerCollect?.Invoke(tag);
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right);
    }
}
