using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour
{
    public enum PLATAFORMTYPES {MOVE,BREAK,MOVEANDDAMAGE }
    [SerializeField] public PLATAFORMTYPES plataformTypes;
    [SerializeField] public float timeToMove=0;
    [SerializeField] public float timeToBreak=0;
    [SerializeField] public float toGo=0;
    private Vector3 initialPos=Vector3.zero;
    private Vector3 FinalPosition = Vector3.zero;
    private void Start()
    {
        FinalPosition = new Vector3(transform.position.x + toGo, transform.position.y, transform.position.z);
        initialPos = transform.position;
        switch (plataformTypes) {
            case PLATAFORMTYPES.MOVE:
                StartCoroutine(PlataformGo());
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
    IEnumerator PlataformGo()
    {
        //yield return new WaitForSeconds(timeToMove);
        float time = 0;
        float speed = 1;
        Vector3 FinalPosition = new Vector3(transform.position.x+toGo, transform.position.y, transform.position.z);
        while (time < timeToMove)
        {
            time += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(initialPos, FinalPosition, time);
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitForSeconds(timeToMove);
        StartCoroutine(PlataformReturn());
    }
    IEnumerator PlataformReturn()
    {
        float time = 0;
        float speed = 1;
        while (time < timeToMove)
        {
            time += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(FinalPosition, initialPos, time);
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitForSeconds(timeToMove);
        StartCoroutine(PlataformGo());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (plataformTypes == PLATAFORMTYPES.BREAK)
        {
            if (collision.transform.CompareTag("Player"))
            {
                StartCoroutine(PlataformStartBreak(transform.position));
            }
        }
    }
    IEnumerator PlataformStartBreak(Vector3 initialPos)
    {
        GetComponent<Animator>().SetTrigger("Break");
        yield return new WaitForSeconds(timeToBreak);
        Destroy(gameObject);
    }
}
