using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScene : MonoBehaviour
{
    public Animator animatorScene;
    private int SceneIndex;
    void Update()
    {
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
    public void BackButton()
    {
        StartCoroutine("GoBackPressed");
    }
    public void TryAgainButton()
    {
        StartCoroutine("TryAgainPressed");
    }
    public void FadeLevel(int SceneToTransition)
    {
        SceneIndex = SceneToTransition;
        animatorScene.SetTrigger("FadeOut");
    }
    IEnumerator GoBackPressed()
    {
        yield return new WaitForSeconds(1);
        FadeLevel(SceneManager.GetActiveScene().buildIndex - 3);
        yield return null;
    }
    IEnumerator TryAgainPressed()
    {
        yield return new WaitForSeconds(1);
        FadeLevel(SceneManager.GetActiveScene().buildIndex - 1);
        yield return null;
    }
}
