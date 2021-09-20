using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public PlayerMovement player;
    private bool completed;
    #region SINGLETON
    static private GameManager instance;
    static public GameManager GetInstance() { return instance; }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    public bool win;
    public bool gameOver = false;
    public bool pause = false;
    public int HighScore;
    public int level = 1;
    public void GameOver()
    {
        if (gameOver)
        {
            GameObject animatorScene = GameObject.FindGameObjectWithTag("SceneTransition");
            if (animatorScene != null)
            {
                animatorScene.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine("Transition");
            }
        }
    }
    public void TransitionToNewScene()
    {
        if (Fade.faded)
        {
            Fade.faded = false;
            SceneManager.LoadScene(4);
        }
    }
    IEnumerator Transition()
    {
        yield return new WaitForSeconds(2);
        TransitionToNewScene();
        yield return null;
    }
    void Start()
    {
        player.OnPlayerCompletedLevel += Suceed;
        player.OnPlayerDie += GameOverPlayer;
    }
    private void OnDestroy()
    {
        player.OnPlayerCompletedLevel -= Suceed;
        player.OnPlayerDie -= GameOverPlayer;
    }
    private void GameOverPlayer()
    {
        GoBackToHub();
    }
    private void Suceed()
    {
        completed = true;
        GoBackToHub();
    }
    void GoBackToHub()
    {
        GameObject animatorScene = GameObject.FindGameObjectWithTag("SceneTransition");
        if (animatorScene != null)
        {
            animatorScene.GetComponent<Animator>().SetTrigger("FadeOut");
        }
    }
}
