using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public static bool faded = false;
    public void FadeComplete()
    {
        faded = true;
    }
}
