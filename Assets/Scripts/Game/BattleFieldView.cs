using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldView : MonoBehaviour
{
    private BattleFieldModel model;
    private void Awake()
    {
        model = GetComponent<BattleFieldModel>();
    }

    private void Start()
    {
        transform.localScale = new Vector2(model.areaWidth, model.areaHeight);
    }
}
