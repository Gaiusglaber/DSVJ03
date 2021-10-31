using System.Collections;
using System.Collections.Generic;
using Numetry.Tools.Lerper;
using UnityEngine;

public class Plataform : MonoBehaviour
{
    public enum PLATAFORMTYPES {MOVE,BREAK,MOVEANDDAMAGE,JUMPABLE }
    [SerializeField] public PLATAFORMTYPES plataformTypes;
    [SerializeField] private float jumpingForce = 0;
    public Vector3 DestPos1 = Vector3.zero;
    private Vector3 InitialPosition = Vector3.zero;
    public Vector3Lerper PosLerper = new Vector3Lerper(0f, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
    public float speedLerper = 0;
    public float timeToBreak;
    public float secondsToStop = 0;
    private bool hited = false;
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
                StartCoroutine(GoToPos1());
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            switch (plataformTypes)
            {
                case PLATAFORMTYPES.MOVE:
                    collision.transform.parent = transform;
                    break;
                case PLATAFORMTYPES.MOVEANDDAMAGE:
                    hited = true;

                    break;
                case PLATAFORMTYPES.BREAK:
                    StartCoroutine(PlataformStartBreak(transform.position));
                    break;
                case PLATAFORMTYPES.JUMPABLE:
                    collision.transform.GetComponent<Rigidbody>().velocity= new Vector3(collision.transform.GetComponent<Rigidbody>().velocity.x,
                        collision.transform.GetComponent<Rigidbody>().velocity.y*jumpingForce, collision.transform.GetComponent<Rigidbody>().velocity.z);
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
