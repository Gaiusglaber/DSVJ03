using System.Collections;
using System.Collections.Generic;
using Numetry.Tools.Lerper;
using UnityEngine;
using System;

public class Plataform : MonoBehaviour
{
    public enum PLATAFORMTYPES { MOVE, BREAK, MOVEANDDAMAGE, JUMPABLE }
    [SerializeField] public PLATAFORMTYPES plataformTypes;
    [SerializeField] private float jumpingForce = 0;
    [SerializeField] private float secondsToDamage = 0;
    [SerializeField] private float lerperColorSpeed = 0;
    [SerializeField] private Color colorToLerp = Color.white;
    public Vector3 DestPos1 = Vector3.zero;
    private Vector3 InitialPosition = Vector3.zero;
    public Vector3Lerper PosLerper = new Vector3Lerper(0f, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
    private ColorLerper colorLerper = null;
    public float speedLerper = 0;
    public float timeToBreak;
    public float secondsToStop = 0;
    private bool hited = false;
    private bool damagebleMode = false;
    private float timer = 0;
    private Color initialColor;
    public static event Action OnPlayerHit;
    private GameObject player = null;
    private Rigidbody rb = null;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialColor = GetComponent<MeshRenderer>().material.color;
        colorLerper = new ColorLerper(Time.deltaTime, AbstractLerper<Color>.SMOOTH_TYPE.STEP_SMOOTHER);
        InitialPosition = transform.position;
        switch (plataformTypes)
        {
            case PLATAFORMTYPES.MOVE:
                StartCoroutine(GoToPos1());
                break;
            case PLATAFORMTYPES.BREAK:
                break;
            case PLATAFORMTYPES.MOVEANDDAMAGE:
                StartCoroutine(ChangeToRed());
                StartCoroutine(GoToPos1());
                break;
        }
    }
    private void LateUpdate()
    {
        /*if (player)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            Vector3 velocity = transform.position - InitialPosition;
            rb.transform.Translate(velocity, transform);
        }*/
    }
    private IEnumerator GoToPos1()
    {
        PosLerper.SetValues(InitialPosition, DestPos1, speedLerper, true);
        while (PosLerper.On)
        {
            PosLerper.Update();
            rb.MovePosition(PosLerper.CurrentValue);
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
    private IEnumerator ChangeToRed()
    {
        MeshRenderer actualColor = GetComponent<MeshRenderer>();
        colorLerper.SetValues(actualColor.material.color, colorToLerp, lerperColorSpeed, true);
        damagebleMode = true;
        while (colorLerper.On)
        {
            colorLerper.Update();
            actualColor.material.color = colorLerper.CurrentValue;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(ChangeToWhite());
    }
    private IEnumerator ChangeToWhite()
    {
        damagebleMode = false;
        hited = false;
        MeshRenderer actualColor = GetComponent<MeshRenderer>();
        colorLerper.SetValues(actualColor.material.color, initialColor, lerperColorSpeed, true);
        while (colorLerper.On)
        {
            colorLerper.Update();
            actualColor.material.color = colorLerper.CurrentValue;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(secondsToDamage);
        StartCoroutine(ChangeToRed());
    }
    private IEnumerator GoBackPos1()
    {
        PosLerper.SetValues(DestPos1, InitialPosition, speedLerper, true);
        while (PosLerper.On)
        {
            PosLerper.Update();
            rb.MovePosition(PosLerper.CurrentValue);
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
                    collision.transform.parent = transform;
                    if (!hited && damagebleMode)
                    {
                        hited = true;
                        OnPlayerHit?.Invoke();
                    }
                    break;
                case PLATAFORMTYPES.BREAK:
                    StartCoroutine(PlataformStartBreak(transform.position));
                    break;
                case PLATAFORMTYPES.JUMPABLE:
                    collision.transform.GetComponent<Rigidbody>().velocity = new Vector3(collision.transform.GetComponent<Rigidbody>().velocity.x,
                        collision.transform.GetComponent<Rigidbody>().velocity.y * jumpingForce, collision.transform.GetComponent<Rigidbody>().velocity.z);
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //player = other.gameObject;
        //other.transform.parent = transform;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().Move(rb.velocity * Time.deltaTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //player = null;
        //other.transform.parent = null;
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
        yield return new WaitForSeconds(timeToBreak);
        GetComponent<Animator>().SetTrigger("Break");
        Destroy(gameObject);
    }
}
