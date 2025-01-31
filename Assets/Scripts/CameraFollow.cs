﻿using System;
using UnityEngine;
using System.Collections;
using Numetry.Tools.Lerper;

public class CameraFollow : MonoBehaviour,ILerpeable
{
    [SerializeField] private PlayerMovement player = null;
    [SerializeField] private float posLerperSpeed = 0;
    [SerializeField] private float rotLerperSpeed = 0;
    [SerializeField] private Vector3 angleToRot = default;
    [SerializeField] private Vector3 posToLerp = default;
    public static Action OnExitEvent;
    private float secondsToWait = 0;
    private Vector3 initialRot = default;
    private Vector3 initialPos = default;
    private Vector3Lerper posLerper = null;
    private Vector3Lerper rotLerper = null;
    private bool lerping = false;

    [Header("Camera Properties")]
    private float DistanceAway;                     //how far the camera is from the player.

    public float minDistance = 1;                //min camera distance
    public float maxDistance = 2;                //max camera distance

    public float DistanceUp = -2;                    //how high the camera is above the player
    public float smooth = 4.0f;                    //how smooth the camera moves into place
    public float rotateAround = 70f;            //the angle at which you will rotate the camera (on an axis)

    [Header("Player to follow")]
    public Transform target;                    //the target the camera follows

    [Header("Layer(s) to include")]
    public LayerMask CamOcclusion;                //the layers that will be affected by collision

    [Header("Map coordinate script")]
    //    public worldVectorMap wvm;
    RaycastHit hit;
    float cameraHeight = 55f;
    float cameraPan = 0f;
    float camRotateSpeed = 180f;
    Vector3 camPosition;
    Vector3 camMask;
    Vector3 followMask;

    private float HorizontalAxis;
    private float VerticalAxis;

    // Use this for initialization
    void Start()
    {
        posLerper = new Vector3Lerper(Time.deltaTime, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
        rotLerper = new Vector3Lerper(Time.deltaTime, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
        //the statement below automatically positions the camera behind the target.
        rotateAround = target.eulerAngles.y - 45f;
        if (player)
        {
            player.OnTalkingToNpc += MoveCamera;
        }
    }

    void LateUpdate()
    {
        if (!lerping)
        {
            HorizontalAxis = Input.GetAxis("Horizontal");
            VerticalAxis = Input.GetAxis("Vertical");

            rotateAround = target.eulerAngles.y - 45f;
            //Offset of the targets transform (Since the pivot point is usually at the feet).
            Vector3 targetOffset = new Vector3(target.position.x, (target.position.y + 2f), target.position.z);
            Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, cameraPan);
            Vector3 vectorMask = Vector3.one;
            Vector3 rotateVector = rotation * vectorMask;
            //this determines where both the camera and it's mask will be.
            //the camMask is for forcing the camera to push away from walls.
            camPosition = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;
            camMask = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;

            occludeRay(ref targetOffset);
            smoothCamMethod();

            transform.LookAt(target);

            rotateAround = target.eulerAngles.y - 45f;


            rotateAround += HorizontalAxis * camRotateSpeed * Time.deltaTime;
            DistanceAway = Mathf.Clamp(DistanceAway += VerticalAxis, minDistance, maxDistance);
        }
    }
    private void MoveCamera(float secondsToDespawn)
    {
        secondsToWait = secondsToDespawn;
        StartCoroutine(Lerp(initialPos.x, posToLerp.x, posLerperSpeed));
    }
    void smoothCamMethod()
    {
        transform.position = Vector3.Lerp(transform.position, camPosition, Time.deltaTime * smooth);
    }
    void occludeRay(ref Vector3 targetFollow)
    {
        #region prevent wall clipping
        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();
        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.
        if (Physics.Linecast(targetFollow, camMask, out wallHit, CamOcclusion))
        {
            //the smooth is increased so you detect geometry collisions faster.
            smooth = 10f;
            //the x and z coordinates are pushed away from the wall by hit.normal.
            //the y coordinate stays the same.
            camPosition = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.5f);
        }
        #endregion
    }

    public IEnumerator Lerp(float firstPos, float endPos, float speed)
    {
        lerping = true;
        initialRot = transform.eulerAngles;
        initialPos = transform.position;
        rotLerper.SetValues(initialRot, angleToRot, rotLerperSpeed,true);
        posLerper.SetValues(initialPos, posToLerp, posLerperSpeed, true);
        while (rotLerper.On && posLerper.On)
        {
            rotLerper.Update();
            posLerper.Update();
            transform.position = posLerper.CurrentValue;
            transform.eulerAngles = rotLerper.CurrentValue;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(secondsToWait);
        OnExitEvent?.Invoke();
        lerping = false;
    }

}