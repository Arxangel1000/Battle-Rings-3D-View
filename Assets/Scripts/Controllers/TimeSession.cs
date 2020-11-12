using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSession : MonoBehaviour
{
    public static TimeSession instance;
    public DateTime timeInSession;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
       timeInSession = System.DateTime.Now;
    }
    
}
