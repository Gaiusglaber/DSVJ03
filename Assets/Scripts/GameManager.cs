using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

[Serializable]
public class Level
{
    public int CollectableA = 0;
    public int CollectableB = 0;
}
public class GameManager : MonoBehaviour
{
    public int cantlvls = 0;
    public Level[] levels;
    public PlayerMovement player;
    public ScoreManager scoreManager;
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
    public int indexlevel = 0;
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
        scoreManager = FindObjectOfType<ScoreManager>();
        levels = new Level[cantlvls];
        GameObject level = GameObject.FindGameObjectWithTag("Level");
        if (level)
        {
            for (short i = 0; i < level.transform.childCount; i++)
            {
                if (level.transform.GetChild(i).CompareTag("CollectableAText") && completed)
                {
                    if (level.transform.GetChild(i).GetComponent<TMPro.TMP_Text>().text == "0")
                    {
                        continue;
                    }
                    PlayerPrefs.SetInt("A", levels[i].CollectableA);
                    level.transform.GetChild(i).GetComponent<TMPro.TMP_Text>().text = levels[i].CollectableA.ToString();
                }
                if (level.transform.GetChild(i).CompareTag("CollectableBText") && completed)
                {
                    level.transform.GetChild(i).GetComponent<TMPro.TMP_Text>().text = levels[i].CollectableB.ToString();
                    PlayerPrefs.SetInt("B", levels[i].CollectableB);
                }
            }
        }
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
        levels[indexlevel].CollectableA = scoreManager.collectableACant;
        levels[indexlevel].CollectableB = scoreManager.collectableBCant;
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
