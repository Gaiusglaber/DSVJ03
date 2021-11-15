using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Numetry.Tools.Lerper;

public class PausePanel : MonoBehaviour,ILerpeable
{
    [SerializeField] private PlayerMovement player = null;
    [SerializeField] private float speedLerper = 0;
    [SerializeField] private float destXPos = 0;
    [SerializeField] private float lerperSpeed = 0;
    private FloatLerper floatLerper = null;
    private float initialXPos = 0;
    private void Awake()
    {
        initialXPos = GetComponent<RectTransform>().anchoredPosition.x;
        floatLerper = new FloatLerper(Time.deltaTime, AbstractLerper<float>.SMOOTH_TYPE.STEP_SMOOTHER);
        player.OnPause += ShowPanel;
        player.OnUnpause += HidePanel;
    }
    private void OnDestroy()
    {
        player.OnPause -= ShowPanel;
        player.OnUnpause -= HidePanel;
    }
    private void ShowPanel()
    {
        StartCoroutine(Lerp(initialXPos, destXPos, lerperSpeed));
    }
    private void HidePanel()
    {
        StartCoroutine(Lerp(destXPos, initialXPos, lerperSpeed));
    }
    public IEnumerator Lerp(float firstPos, float endPos, float speed)
    {
        float xPos = 0;
        RectTransform pos = GetComponent<RectTransform>();
        floatLerper.SetValues(firstPos, endPos, speed, true);
        while (floatLerper.On)
        {
            if (floatLerper.Reached)
            {
                floatLerper.SwitchState(false);
            }
            else
            {
                floatLerper.Update();
                xPos = floatLerper.CurrentValue;
                pos.anchoredPosition = new Vector2(xPos, pos.anchoredPosition.y);
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
