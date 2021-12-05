using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public int collectableACant = 0;
    public int collectableBCant = 0;
    [SerializeField] private TMP_Text collectableAText;
    [SerializeField] private TMP_Text collectableBText;
    [SerializeField] private int maxWhiteCollectables = 0;
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
            AkSoundEngine.PostEvent("collect_item",gameObject);
            collectableACant++;
            collectableAText.text = collectableACant.ToString();
        }
        else
        {
            AkSoundEngine.PostEvent("collect_special", gameObject);
            collectableBCant++;
            collectableBText.text = collectableBCant.ToString();
        }
    }
}
