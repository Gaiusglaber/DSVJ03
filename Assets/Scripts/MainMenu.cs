using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text versionTxt = null;
    public GameObject scoreObjetc;
    public Text score;
    public Animator animatorPlayButton;
    public Animator animatorScene;
    private int SceneIndex;
    private bool pressed;
    private void Start()
    {
        versionTxt.text = Application.version;
    }
    void Update()
    {
        InputController();
        SceneFade();
    }
    public void SceneFade()
    {
        if (Fade.faded)
        {
            Fade.faded = false;
            SceneManager.LoadScene(SceneIndex);
        }
    }
    public void InputController()
    {
        if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey("enter") || pressed)
        {
            animatorPlayButton.SetTrigger("Selected");
            StartCoroutine("PlayPressed");
        }
    }
    public void Play()
    {
        pressed = true;
    }
    public void Credits()
    {
        StartCoroutine("CreditsPressed");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void FadeLevel(int SceneToTransition)
    {
        SceneIndex = SceneToTransition;
        animatorScene.SetTrigger("FadeOut");
    }
    IEnumerator PlayPressed()
    {
        pressed = false;
        yield return new WaitForSeconds(1);
        FadeLevel(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }
    IEnumerator CreditsPressed()
    {
        yield return new WaitForSeconds(1);
        FadeLevel(4);
        yield return null;
    }
}
