using UnityEngine;
using Numetry.Tools.Lerper;
using System.Collections;
public class Hover : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Animator hoverAnimator;
    public Transform followTransform;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;
    private ColorLerper colorLerper= new ColorLerper(0f, AbstractLerper<Color>.SMOOTH_TYPE.STEP_SMOOTHER);
    private Vector3Lerper posLerper = null;
    private Color initialColor;
    [SerializeField] private Material[] materialToDmg;
    [SerializeField] private MeshRenderer meshHover;
    [SerializeField]private float colorChangingSpeed = 0;
    [SerializeField] private float secondsToHeal = 0;
    [SerializeField] private int life = 2;
    private bool light = false;
    public bool paused = false;
    void Start()
    {
        posLerper = new Vector3Lerper(0, AbstractLerper<Vector3>.SMOOTH_TYPE.STEP_SMOOTHER);
        initialColor = GetComponentInChildren<MeshRenderer>().material.color;
        Enemy.OnPlayerHit += MaterialChange;
        Spikes.OnPlayerHit += MaterialChange;
        player.OnTurnOnLantern += TurnLantern;
        Plataform.OnPlayerHit += MaterialChange;
    }
    private void OnDestroy()
    {
        Enemy.OnPlayerHit -= MaterialChange;
        Spikes.OnPlayerHit -= MaterialChange;
        player.OnTurnOnLantern -= TurnLantern;
        Plataform.OnPlayerHit -= MaterialChange;
    }
    private void TurnLantern()
    {
        if (!light)
        {
            hoverAnimator.Play("TurnLightOn");
            AkSoundEngine.PostEvent("light_switch",gameObject);
            light = true;
        }
        else
        {
            AkSoundEngine.PostEvent("light_switch", gameObject);
            hoverAnimator.Play("TurnLightOff");
            light = false;
        }
    }
    private void MaterialChange()
    {
        StartCoroutine(ColorLerping());
    }
    IEnumerator ColorLerping()
    {
        if (life >= 0)
        {
            colorLerper.SetValues(initialColor, materialToDmg[life].color, colorChangingSpeed, true);
            life--;
            while (colorLerper.On)
            {
                colorLerper.Update();
                meshHover.material.color = colorLerper.CurrentValue;
                if (colorLerper.Reached)
                {
                    colorLerper.SwitchState(false);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            yield return null;
        }
        else
        {
            GameManager.GetInstance().GameOver();
            yield return null;
        }
    }
    void FollowPlayer()
    {
        Vector3 playerPos = new Vector3(followTransform.position.x - 2, followTransform.position.y + 2,
            followTransform.position.z - 2);
        transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothTime);
        transform.forward = Vector3.SmoothDamp(transform.forward, followTransform.forward, ref velocity, smoothTime);

        float amplitude = 0.003f;
        float frequency = 0.3f;
        Vector3 tempPos = transform.position;

        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }
    void Update()
    {
        FollowPlayer();
    }
}