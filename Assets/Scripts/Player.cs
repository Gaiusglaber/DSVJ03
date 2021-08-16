using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public event Action OnFollowingPlayer;
    // Update is called once per frame
    private void FixedUpdate()
    {
        OnFollowingPlayer?.Invoke();
    }
}
