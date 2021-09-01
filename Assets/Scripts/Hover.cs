using UnityEngine;
public class Hover : MonoBehaviour
{
    public Transform followTransform;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, followTransform.position, ref velocity, smoothTime);
        transform.forward = Vector3.SmoothDamp(transform.forward, followTransform.forward, ref velocity, smoothTime);
        float amplitude = 0.03f;
        float frequency = 0.3f;
        Vector3 tempPos = transform.position;

        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }
}