using System.Collections;
using System.Collections.Generic;
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
        else
        {
        }
        JumpMechanic();
    }
    void JumpMechanic()
    {
        float YVelocity = rigidBody.velocity.y;
        if (YVelocity > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && bounceCounter < 1)
            {
                rigidBody.AddForce(new Vector3(0, jumpSpeed * 50, 0));
                bounceCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                rigidBody.AddForce(transform.forward * jumpSpeed * 25);
            }
        }
        else if (YVelocity == 0)
        {
            bounceCounter = 0;
        }
    }
}