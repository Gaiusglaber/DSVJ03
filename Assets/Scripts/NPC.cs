using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Action<bool,GameObject> OnGetCloseFromNPC = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnGetCloseFromNPC?.Invoke(true,gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnGetCloseFromNPC?.Invoke(false, gameObject);
        }
    }
}
