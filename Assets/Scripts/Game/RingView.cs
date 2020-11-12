using System;
using System.Collections;
using UnityEngine;

public class RingView : MonoBehaviour
{
    private RingModel model;
    private bool attention;

    private void Awake()
    {
        model = GetComponent<RingModel>();
    }

    public void LoadScaleUnit()
    {
        transform.localScale = new Vector3(model.unitSpawnRadius,model.unitSpawnRadius,model.unitSpawnRadius );
    }
    

}
