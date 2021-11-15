using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Numetry.Tools.Lerper;

public class PostProcessing : MonoBehaviour, ILerpeable
{
    [SerializeField] private Volume postProcessProfile;
    [SerializeField]private PlayerMovement player = null;
    [SerializeField] private float amountOfBlur = 0;
    [SerializeField] private float speedLerper = 0;
    private DepthOfField depthOfField = null;
    private FloatLerper DepthLerper = null;
    private float InitialBlurValue = 0;
    private void Awake()
    {
        player.OnPause += PauseOn;
        player.OnUnpause += PauseOff;
    }
    private void Start()
    {
        DepthLerper = new FloatLerper(Time.deltaTime, AbstractLerper<float>.SMOOTH_TYPE.STEP_SMOOTHER);
        postProcessProfile.profile.TryGet(out depthOfField);
        InitialBlurValue = depthOfField.focusDistance.value;
    }
    private void OnDestroy()
    {
        player.OnPause -= PauseOn;
        player.OnUnpause -= PauseOff;
    }
    public void PauseOn()
    {
        StartCoroutine(Lerp(InitialBlurValue, amountOfBlur, speedLerper));
    }
    public void PauseOff()
    {
        StartCoroutine(Lerp(amountOfBlur, InitialBlurValue, speedLerper));
    }
    public IEnumerator Lerp(float firstPos, float endPos, float speed)
    {
        DepthLerper.SetValues(firstPos, endPos, speed, true);
        while (DepthLerper.On)
        {
            if (DepthLerper.Reached)
            {
                DepthLerper.SwitchState(false);
            }
            else
            {
                DepthLerper.Update();
                depthOfField.focusDistance.value = DepthLerper.CurrentValue;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
