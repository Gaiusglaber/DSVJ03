using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
            //añadir al score
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right);
    }
}
