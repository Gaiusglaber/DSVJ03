using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubUIManager : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text collectableAText = null;
    [SerializeField] private TMPro.TMP_Text collectableBText = null;
    private void Start()
    {
        collectableAText.text = PlayerPrefs.GetInt("A", 0).ToString();
        collectableBText.text = PlayerPrefs.GetInt("B", 0).ToString();
    }
}
