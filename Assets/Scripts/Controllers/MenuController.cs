using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject winBanner;
    [SerializeField] private Text whoWin;
    [SerializeField] private Text howTime;
    [SerializeField] private Camera thisCamera;
    [SerializeField] private GameObject canvasPortrait;
    [SerializeField] private GameObject canvasLandscape;
    

    private void Start()
    {
        thisCamera = thisCamera.GetComponent<Camera>();
        winBanner.SetActive(false);
        SubscribeIsOneTeamIsGone();
    }

    private void Update()
    {
        ScreenOrientationManager();
    }
    
    private void SubscribeIsOneTeamIsGone()
    {
        EventUnitsTeamIsGone.teamIsGone += WinLogic;
    }

    private void UnSubscribeIsOneTeamIsLos()
    {
        EventUnitsTeamIsGone.teamIsGone -= WinLogic;
    }

    private void OnDisable()
    {
        UnSubscribeIsOneTeamIsLos();
    }

    public void RestartScene()
    {
        GameData.currentCountUnit = 0;
        GameData.blueUnitAtScene = 0;
        GameData.redUnitAtScene = 0;
        GameData.sumRadiusBlueUnit = 0;
        GameData.sumRadiusRedUnit = 0;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ScreenOrientationManager()
    {
        if (Screen.orientation == ScreenOrientation.Landscape)
        {
            thisCamera.orthographicSize = GameData.gameAreaWidth * 3;
            canvasPortrait.SetActive(false);
            canvasLandscape.SetActive(true);
        }
        else if (Screen.orientation == ScreenOrientation.Portrait)
        {
            thisCamera.orthographicSize = GameData.gameAreaWidth * 6;
            canvasPortrait.SetActive(true);
            canvasLandscape.SetActive(false);
        }
    }

    private void WinLogic()
    {
        winBanner.SetActive(true);
        if (GameData.blueUnitAtScene <= 0)
            whoWin.text = "Победила: красная команда";
        if (GameData.redUnitAtScene <= 0)
            whoWin.text = "Победила: синяя команда";
        var timeSession = System.DateTime.Now - TimeSession.instance.timeInSession;
        howTime.text = "Время сессии " + timeSession.Minutes + "m : " + timeSession.Seconds + "s";
    }
}