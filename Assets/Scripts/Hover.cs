﻿using UnityEngine;
using Numetry.Tools.Lerper;
using System.Collections;
public class Hover : MonoBehaviour
{
    public Transform followTransform;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;
    private ColorLerper colorLerper= new ColorLerper(0f, AbstractLerper<Color>.SMOOTH_TYPE.STEP_SMOOTHER);
    private Color initialColor;
    [SerializeField] private Material materialToDmg;
    [SerializeField]private float colorChangingSpeed = 0;
    [SerializeField] private float secondsToHeal = 0;
    void Start()
    {
        initialColor = GetComponentInChildren<MeshRenderer>().material.color;
        Enemy.OnPlayerHit += MaterialChange;
        Spikes.OnPlayerHit += MaterialChange;
    }
    private void OnDestroy()
    {
        Enemy.OnPlayerHit -= MaterialChange;
        Spikes.OnPlayerHit -= MaterialChange;
    }
    private void MaterialChange()
    {
        StartCoroutine(ColorLerping());
    }
    IEnumerator ColorLerping()
    {
        colorLerper.SetValues(initialColor, materialToDmg.color, colorChangingSpeed, true);
        while (colorLerper.On)
        {
            colorLerper.Update();
            foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material.color = colorLerper.CurrentValue;
            }
            if (colorLerper.Reached)
            {
                colorLerper.SwitchState(false);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForSeconds(secondsToHeal);
        StartCoroutine(HealLerping());

    }
    IEnumerator HealLerping()
    {
        colorLerper.SetValues(materialToDmg.color, initialColor, colorChangingSpeed, true);
        while (colorLerper.On)
        {
            colorLerper.Update();
            foreach(var renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material.color = colorLerper.CurrentValue;
            }
            if (colorLerper.Reached)
            {
                colorLerper.SwitchState(false);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
    void Update()
    {
        Vector3 playerPos = new Vector3(followTransform.position.x-2, followTransform.position.y + 3, followTransform.position.z-2);
        transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothTime);
        transform.forward = Vector3.SmoothDamp(transform.forward, followTransform.forward, ref velocity, smoothTime);

        float amplitude = 0.003f;
        float frequency = 0.3f;
        Vector3 tempPos = transform.position;

        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }
}