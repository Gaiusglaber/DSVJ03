using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObj : MonoBehaviour
{
    [SerializeField] private GameObject prefabCollectable = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject aux = Instantiate(prefabCollectable,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
