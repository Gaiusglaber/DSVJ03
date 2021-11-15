using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILerpeable
{
    IEnumerator Lerp(float firstPos, float endPos, float speed);
}
