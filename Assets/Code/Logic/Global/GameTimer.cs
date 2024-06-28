using System;
using System.Collections;
using UnityEngine;

public class GameTimer 
{
    //public void CorutyinStatr()
    //{
        
    //}

    private IEnumerator Waits(float time, Action callback = null)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }
}
