
using System;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public static BarController instance;
    [SerializeField] private Image blueLine;
    [SerializeField] private Image redLine;
    private float bluePersent;
    private float redPersent;
    private float sumAll;

    private void Awake()
    {
        instance = this;
    }
    
    public void UpdateBart() 
    {
        sumAll = GameData.sumRadiusBlueUnit + GameData.sumRadiusRedUnit;
        bluePersent = (GameData.sumRadiusBlueUnit * 100) / sumAll;
        redPersent = (GameData.sumRadiusRedUnit * 100) / sumAll;
        blueLine.fillAmount = bluePersent/100;
        redLine.fillAmount = redPersent/100;
    }
}
