﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private static float valueSinhronize;
    private void OnEnable()
    {
        Time.timeScale = valueSinhronize;
        slider.value = valueSinhronize;
    }

    public void TimeSpeedValue()
    {
        valueSinhronize = slider.value;
        Time.timeScale = valueSinhronize;
    }
}
