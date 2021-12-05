using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Numetry.Tools.Lerper;
public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject leverPivot = null;
    [SerializeField] private Vector3 leverToRot = Vector3.zero;
    [SerializeField] private float lerperTime = 0;

    public event Action OnPlayerHitLever = null;
    private bool isNear = false;
    private bool isTriggered = false;
    private Vector3Lerper leverRotLerper = null;
    private void Start()
    {
        leverRotLerper = new Vector3Lerper(Time.deltaTime, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
        else
        {
            isNear = false;
        }
    }
    private void Update()
    {
        if (isNear&&Input.GetKeyDown(KeyCode.E)&&!isTriggered)
        {
            isTriggered = true;
            OnPlayerHitLever?.Invoke();
            StartCoroutine(RotateLever());
        }
    }
    
    private IEnumerator RotateLever()
    {
        leverRotLerper.SetValues(leverPivot.transform.eulerAngles, leverToRot, lerperTime, true);
        while (leverRotLerper.On)
        {
            if (leverRotLerper.Reached)
            {
                leverRotLerper.SwitchState(false);
            }
            else
            {
                leverRotLerper.Update();
                leverPivot.transform.eulerAngles = leverRotLerper.CurrentValue;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
