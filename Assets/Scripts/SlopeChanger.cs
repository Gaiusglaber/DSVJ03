using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlopeChanger : MonoBehaviour
{
    [SerializeField] private bool _up;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player") return;
        CharacterController controller = col.GetComponent<CharacterController>();
        if (_up)
            controller.slopeLimit = 20;
        else if (controller.slopeLimit == 20)
            controller.slopeLimit = 35;
        else
            controller.slopeLimit = 20;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag != "Player") return;
        if (!_up)
        {
            CharacterController controller = col.GetComponent<CharacterController>();
            controller.slopeLimit = 35;
        }
    }
}