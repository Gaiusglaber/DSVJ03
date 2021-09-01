using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    private int collectableACant = 0;
    private int collectableBCant = 0;
    [SerializeField] private TMP_Text collectableAText;
    [SerializeField] private TMP_Text collectableBText;
    private void Start()
    {
        CollectableA.OnPlayerCollect += AddScore;
    }
    private void OnDestroy()
    {
        CollectableA.OnPlayerCollect -= AddScore;
    }
    void AddScore(string tag)
    {
        if (tag == "CollectableA")
        {
            collectableACant++;
            collectableAText.text = collectableACant.ToString();
        }
        else
        {
            collectableBCant++;
            collectableBText.text = collectableBCant.ToString();
        }
    }
}
