using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Numetry.Tools.Lerper;

public class Door : MonoBehaviour
{
    [SerializeField] private Trigger trigger = null;
    [SerializeField] private Vector3 rightDoorPos = Vector3.zero;
    [SerializeField] private Vector3 leftDoorPos = Vector3.zero;
    [SerializeField] private GameObject leftDoor = null;
    [SerializeField] private GameObject rightDoor = null;
    [SerializeField]private float timeLerper = 0;

    private Vector3Lerper leftDoorLerper = null;
    private Vector3Lerper rightDoorLerper = null;


    private void Start()
    {
        leftDoorLerper = new Vector3Lerper(Time.deltaTime, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
        rightDoorLerper = new Vector3Lerper(Time.deltaTime, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
        trigger.OnPlayerHitLever += OpenDoors;
    }
    private void OnDestroy()
    {
        trigger.OnPlayerHitLever -= OpenDoors;
    }
    public void OpenDoors()
    {
        leftDoorLerper.SetValues(leftDoor.transform.localPosition, leftDoorPos, timeLerper, true);
        rightDoorLerper.SetValues(rightDoor.transform.localPosition, rightDoorPos, timeLerper, true);
        StartCoroutine(StartOpeningDoors());
    }
    private IEnumerator StartOpeningDoors()
    {
        while (rightDoorLerper.On && leftDoorLerper.On)
        {
            if (rightDoorLerper.Reached)
            {
                rightDoorLerper.SwitchState(false);
            }
            else
            {
                rightDoorLerper.Update();
                rightDoor.transform.localPosition = rightDoorLerper.CurrentValue;
            }
            if (leftDoorLerper.Reached)
            {
                leftDoorLerper.SwitchState(false);
            }
            else
            {
                leftDoorLerper.Update();
                leftDoor.transform.localPosition = leftDoorLerper.CurrentValue;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
