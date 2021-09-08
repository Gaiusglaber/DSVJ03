using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spikes : MonoBehaviour
{
    public static event Action OnPlayerHit;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OnPlayerHit?.Invoke();
        }
    }
}
