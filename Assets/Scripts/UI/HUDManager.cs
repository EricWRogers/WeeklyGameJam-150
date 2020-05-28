using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : SingletonMonoBehaviour<HUDManager>
{
    // Default 10min
    public float GameLengthSec = 600.0f;
    public TMP_Text TimeTillDawn = null;
    public GameObject PauseMenu = null;

    private bool CountingDown = false;
    private float OriginalTime = 600.0f;

    new void Awake()
    {
        base.Awake();
        OriginalTime = GameLengthSec;
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
    public void PauseGame()
    {
        if(PauseMenu != null)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
    public void ResumeGame()
    {
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
            _UpdateClockUI(GameLengthSec);

            if (GameLengthSec <= 0.0f)
            {
                CountingDown = false;
                _GameOver();
            }
        }
    }
    private void _UpdateClockUI(float sec)
    {
        if (sec < 0.0f)
            sec = 0.0f;

        float invertedTimeLeft = OriginalTime - sec;
        sec = (invertedTimeLeft * (6 * 3600)) / OriginalTime;


        int hour = (int)((sec / 60) / 60);

        if (hour <= 0)
            hour = 12;

        int min = (int)((sec / 60) % 60);

        if (min < 10)
            TimeTillDawn.text = hour + " : 0" + min + " AM";
        else
            TimeTillDawn.text = hour + " : " + min + " AM";
    }
    private void _GameOver()
    {
        _UpdateClockUI(0.0f);
        Debug.Log("GameOver");
    }
}
