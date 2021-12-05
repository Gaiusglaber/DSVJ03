using UnityEngine;
public class SlopeChanger : MonoBehaviour
{
    private float auxSlope;
    private float auxCamera;
    [SerializeField] private CameraFollow cameraFollow;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player") return;
        CharacterController controller = col.GetComponent<CharacterController>();
        auxSlope = controller.slopeLimit;
        controller.slopeLimit = 0;
        auxCamera = cameraFollow.DistanceUp;
        cameraFollow.DistanceUp = 1;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag != "Player") return;
        CharacterController controller = col.GetComponent<CharacterController>();
        controller.slopeLimit = auxSlope;
        cameraFollow.DistanceUp = auxCamera;
    }
}