using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public PlayerMovement player;
    public bool completed;
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
            GameObject animatorScene = GameObject.FindGameObjectWithTag("SceneTransition");
            if (animatorScene != null)
            {
                animatorScene.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex));
            }
    }
    public void TransitionToNewScene(int index)
    {

        if (Fade.faded)
        {
            Fade.faded = false;
            SceneManager.LoadScene(index + 1);
        }
    }
    IEnumerator Transition(int index)
    {
        yield return new WaitForSeconds(2);
        TransitionToNewScene(index);
        yield return null;
    }
    void Start()
    {
        if (player)
        {
            player.OnPlayerCompletedLevel += Suceed;
            player.OnPlayerDie += GameOverPlayer;
        }
    }
    private void OnDestroy()
    {
        if (player)
        {
            player.OnPlayerCompletedLevel -= Suceed;
            player.OnPlayerDie -= GameOverPlayer;
        }
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
    public void GoBackToHub()
    {
        GameObject animatorScene = GameObject.FindGameObjectWithTag("SceneTransition");
        if (animatorScene != null)
        {
            animatorScene.GetComponent<Animator>().SetTrigger("FadeOut");
            StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex-1));
        }
    }
}
