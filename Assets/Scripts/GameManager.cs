using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public PlayerMovement player;
    private bool completed;
    void Start()
    {
        player.OnPlayerCompletedLevel += Suceed;
        player.OnPlayerDie += GameOver;
    }
    private void OnDestroy()
    {
        player.OnPlayerCompletedLevel -= Suceed;
        player.OnPlayerDie -= GameOver;
    }
    private void GameOver()
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
