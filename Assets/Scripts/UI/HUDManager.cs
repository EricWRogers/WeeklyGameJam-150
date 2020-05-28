using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HUDManager : SingletonMonoBehaviour<HUDManager>
{
    // Default 10min
    public float GameLengthSec = 600.0f;
    public TMP_Text TimeTillDawn = null;
    public GameObject PauseMenu = null;
    public RectTransform TimeBar = null;
    public string EatenString = "Eaten";
    public TMP_Text EatenText = null;
    public string LeftString = "Left";
    public TMP_Text LeftText = null;
    public string ExcapeString = "Escaped";
    public TMP_Text ExcapeText = null;

    private bool CountingDown = false;
    private float OriginalTime = 600.0f;
    private float OriginalTimeBarWidth = 400.0f;

    public MenuPage winPanel;

    new void Awake()
    {
        base.Awake();
        OriginalTime = GameLengthSec;
        OriginalTimeBarWidth = TimeBar.rect.width;
    }

    void Start()
    {
        StartCountDown();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeTillDawn != null)
            _UpdateTimeLeft(Time.deltaTime);
        else
            Debug.Log("TimeTillDawn = null");
    }

    public void StartCountDown()
    {
        CountingDown = true;
    }

    void OnPauseToggle(InputValue inputValue)
    {
        if (Time.timeScale == 0.0f)
            ResumeGame();
        else
            PauseGame();
    }
    public void PauseGame()
    {
        if(PauseMenu != null)
        {
            HelperUtilities.UpdateCursorLock(false);
            PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
    public void ResumeGame()
    {
        if (PauseMenu != null)
        {
            HelperUtilities.UpdateCursorLock(true);
            PauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
    public void GoToMainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }
    private float _UpdateCountDown(float sec, float dt)
    {
        sec -= dt;
        return sec;
    }
    private void _UpdateTimeLeft(float dt)
    {
        if (CountingDown)
        {
            GameLengthSec = _UpdateCountDown(GameLengthSec, dt);
            _UpdateClockUI(GameLengthSec, (6 * 3600));
            _UpdateTimeBar(GameLengthSec, (6 * 3600));

            if (GameLengthSec <= 0.0f)
            {
                CountingDown = false;
                _GameOver();
            }
        }
    }
    private void _UpdateClockUI(float sec,float maxTime)
    {
        if (sec < 0.0f)
            sec = 0.0f;

        float invertedTimeLeft = OriginalTime - sec;
        sec = (invertedTimeLeft * maxTime) / OriginalTime;

        


        int hour = (int)((sec / 60) / 60);

        if (hour <= 0)
            hour = 12;

        int min = (int)((sec / 60) % 60);

        if (min < 10)
            TimeTillDawn.text = hour + " : 0" + min + " AM";
        else
            TimeTillDawn.text = hour + " : " + min + " AM";
        
        EatenText.text = EatenString + " : " + PlayerModel.Instance.campersEaten;
        LeftText.text = LeftString + " : " + LevelManager.Instance.campersRemaining;
        ExcapeText.text = ExcapeString + " : " + (CamperManager.Instance.campersSafe);
    }
    private void _UpdateTimeBar(float sec,float maxTime)
    {
        if (sec < 0.0f)
            sec = 0.0f;

        float width = (sec * OriginalTimeBarWidth) / OriginalTime;

        // Debug.Log(width);

        TimeBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
    private void _GameOver()
    {
        _UpdateClockUI(0.0f,(6 * 3600));
        Debug.Log("GameOver");
        LevelManager.Instance.PlayGameOverSequence();
    }

    public void ShowEndScreen()
    {
        Time.timeScale = 0f;
        winPanel.Show();
    }
}
