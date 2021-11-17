using UnityEngine;
public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] float jumpSpeed = 12f;
    [SerializeField] private float distance = 5;
    [SerializeField] private int bounceCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
        {
            if (hit.transform.CompareTag("Door"))
            {
            }
        }
    }
}
