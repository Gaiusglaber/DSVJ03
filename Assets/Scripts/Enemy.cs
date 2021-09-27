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
    public Vector3Lerper RotLerper = new Vector3Lerper(0f, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
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
        while (PosLerper.On)
        {
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
        while (PosLerper.On)
        {
            PosLerper.Update();
            transform.position = PosLerper.CurrentValue;
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
        while (PosLerper.On)
        {
            PosLerper.Update();
            transform.position = PosLerper.CurrentValue;
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
        while (PosLerper.On)
        {
            PosLerper.Update();
            transform.position = PosLerper.CurrentValue;
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
