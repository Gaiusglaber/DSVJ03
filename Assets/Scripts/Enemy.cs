using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Numetry.Tools.Lerper;
using System;
public class Enemy : MonoBehaviour
{
    public Vector3 DestPos1;
    public Vector3 DestPos2;
    private Vector3 InitialPosition;
    public Vector3Lerper PosLerper = new Vector3Lerper(0f, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
    public float speedLerper=0;
    public static event Action OnPlayerHit;
    void Start()
    {
        InitialPosition = transform.position;
        StartCoroutine(GoToPos1());
    }
    private IEnumerator GoToPos1()
    {
        PosLerper.SetValues(InitialPosition, DestPos1, speedLerper,true);
        var lookPos = DestPos1 - InitialPosition;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        while (PosLerper.On)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
            PosLerper.Update();
            transform.localPosition = PosLerper.CurrentValue;
            if (PosLerper.Reached)
            {
                PosLerper.SwitchState(false);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        StartCoroutine(GoBackPos1());
    }
    private IEnumerator GoBackPos1()
    {
        PosLerper.SetValues(DestPos1, InitialPosition, speedLerper, true);
        var lookPos = InitialPosition - DestPos1;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        while (PosLerper.On)
        {
            PosLerper.Update();
            transform.position = PosLerper.CurrentValue;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
            if (PosLerper.Reached)
            {
                PosLerper.SwitchState(false);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        StartCoroutine(GoToPos2());
    }
    private IEnumerator GoToPos2()
    {
        PosLerper.SetValues(InitialPosition, DestPos2, speedLerper, true);
        var lookPos = DestPos2 - InitialPosition;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        while (PosLerper.On)
        {
            PosLerper.Update();
            transform.position = PosLerper.CurrentValue;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
            if (PosLerper.Reached)
            {
                PosLerper.SwitchState(false);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        StartCoroutine(GoBackPos2());
    }
    private IEnumerator GoBackPos2()
    {
        PosLerper.SetValues(DestPos2, InitialPosition, speedLerper, true);
        var lookPos = InitialPosition - DestPos2;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        while (PosLerper.On)
        {
            PosLerper.Update();
            transform.position = PosLerper.CurrentValue;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
            if (PosLerper.Reached)
            {
                PosLerper.SwitchState(false);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        StartCoroutine(GoToPos1());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OnPlayerHit?.Invoke();
        }
    }
}
