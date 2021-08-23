using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private GameObject spikesGO;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        StartCoroutine(SpikesGoUp());
    }
    IEnumerator SpikesGoUp()
    {
        yield return new WaitForSeconds(0.5f);
        float speed = 1;
        float time = 0;
        Vector3 FinalPosition = new Vector3(spikesGO.transform.position.x, spikesGO.transform.position.y+1, spikesGO.transform.position.z);
        Vector3 InitialPosition = spikesGO.transform.position;
        while (time < 1)
        {
            time += Time.deltaTime*speed;
            spikesGO.transform.position = Vector3.Lerp(spikesGO.transform.position, FinalPosition, time);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SpikesGoDown(InitialPosition));
    }
    IEnumerator SpikesGoDown(Vector3 initialPos)
    {
        yield return new WaitForSeconds(0.5f);
        float speed = 1;
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * speed;
            spikesGO.transform.position = Vector3.Lerp(spikesGO.transform.position, initialPos, time);
            yield return new WaitForEndOfFrame();
        }
    }
}
