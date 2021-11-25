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
        GameObject aux = GameObject.FindGameObjectWithTag("Player");
        if (aux)
        {
            player = aux.GetComponent<PlayerMovement>();
        }
        if (player)
        {
            player.OnPlayerCompletedLevel += Suceed;
            player.OnPlayerDie += GameOverPlayer;
            player.OnTalkingToNpc += SpawnHiddenCollectables;
        }
    }
    private void SpawnHiddenCollectables(float TimeToDespawn)
    {
        GameObject hiddenCollectables = GameObject.FindGameObjectWithTag("HiddenCollectables");
        for (int i = 0; i < hiddenCollectables.transform.childCount; i++)
        {
            hiddenCollectables.transform.GetChild(i).gameObject.SetActive(true);
        }
        StartCoroutine(DespawnObjects(hiddenCollectables, TimeToDespawn));
    }
    private IEnumerator DespawnObjects(GameObject collectables,float TimeToDespawn)
    {
        yield return new WaitForSeconds(TimeToDespawn);
        for(int i = 0; i < collectables.transform.childCount; i++)
        {
            collectables.transform.GetChild(i).gameObject.SetActive(false);
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
            StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex-2));
        }
    }
}
