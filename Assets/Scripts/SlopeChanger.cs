using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlopeChanger : MonoBehaviour
{
    private float aux;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player") return;
        CharacterController controller = col.GetComponent<CharacterController>();
        aux = controller.slopeLimit;
        controller.slopeLimit = 0;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag != "Player") return;
        CharacterController controller = col.GetComponent<CharacterController>();
        controller.slopeLimit = aux;
    }
}