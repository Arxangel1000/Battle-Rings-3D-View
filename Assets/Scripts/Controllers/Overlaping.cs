using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlaping : MonoBehaviour
{
    public float timeAfterBorn;
    private bool iAmOldHere;
    public bool trigered;

    private void OnEnable()
    {
        timeAfterBorn = 0;
        iAmOldHere = false;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (timeAfterBorn < 0.02f && Creator.instance.canOverlaping)
        {
            gameObject.transform.position = Creator.instance.SpawnPoint();
        }
        trigered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        trigered = false;
        timeAfterBorn = 0;
    }

    private void Update()
    {
        if (Creator.instance.canOverlaping)
        {
            if(!trigered)
                timeAfterBorn += Time.deltaTime;
            
            if (timeAfterBorn >= 0.02f && !iAmOldHere)
            {
                iAmOldHere = true;
            }
        }
    }
}
