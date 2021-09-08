using System.Collections;
using System.Collections.Generic;
using Numetry.Tools.Lerper;
using UnityEngine;

public class Plataform : MonoBehaviour
{
    public enum PLATAFORMTYPES {MOVE,BREAK,MOVEANDDAMAGE }
    [SerializeField] public PLATAFORMTYPES plataformTypes;
    public Vector3 DestPos1;
    private Vector3 InitialPosition;
    public Vector3Lerper PosLerper = new Vector3Lerper(0f, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
    public float speedLerper = 0;
    public float timeToBreak;
    public float secondsToStop = 0;
    private void Start()
    {
        InitialPosition = transform.position;
        switch (plataformTypes) {
            case PLATAFORMTYPES.MOVE:
                StartCoroutine(GoToPos1());
                break;
            case PLATAFORMTYPES.BREAK:
                break;
            case PLATAFORMTYPES.MOVEANDDAMAGE:
                break;
            default:
                Debug.LogError("Invalid plataform type!");
                break;
        }
    }
    private IEnumerator GoToPos1()
    {
        PosLerper.SetValues(InitialPosition, DestPos1, speedLerper, true);
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
        yield return new WaitForSeconds(secondsToStop);
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
        yield return new WaitForSeconds(secondsToStop);
        StartCoroutine(GoToPos1());
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            switch (plataformTypes)
            {
                case PLATAFORMTYPES.MOVE:
                    collision.transform.parent = transform;
                    break;
                case PLATAFORMTYPES.MOVEANDDAMAGE:
                    break;
                case PLATAFORMTYPES.BREAK:
                    StartCoroutine(PlataformStartBreak(transform.position));
                    break;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
    IEnumerator PlataformStartBreak(Vector3 initialPos)
    {
        GetComponent<Animator>().SetTrigger("Break");
        yield return new WaitForSeconds(timeToBreak);
        Destroy(gameObject);
    }
}
