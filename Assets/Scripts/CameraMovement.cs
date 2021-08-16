using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 distance;
    public Player player;
    void Start()
    {
        player.OnFollowingPlayer += FollowPlayer;
    }
    private void OnDestroy()
    {
        player.OnFollowingPlayer -= FollowPlayer;
    }
    void FollowPlayer()
    {
        Vector3 pos;
        pos = player.transform.position;
        pos.z += distance.x;
        pos.y -= distance.y;
        transform.position = pos;
    }
}
