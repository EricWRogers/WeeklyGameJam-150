using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public int campersRemaining =>
        Mathf.Max(
            CamperManager.Instance.campersCount - PlayerModel.Instance.campersEaten -
            CamperManager.Instance.campersSafe, 0);

    public Transform playerVisiblePoint => PlayerModel.Instance.visibilityCheckPoint;

    public bool isGameEnded { get; private set; } = false;

    new void Awake()
    {
        base.Awake();

        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerModel.Instance.onCamperEaten += camper => { OnCampersRemainingChanged(); };
        CamperManager.Instance.onCamperSafe += camper => { OnCampersRemainingChanged(); };
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayGameOverSequence()
    {
        isGameEnded = true;

        HUDManager.Instance.ShowEndScreen();
    }

    void OnCampersRemainingChanged()
    {
        if (campersRemaining <= 0)
        {
            PlayGameOverSequence();
        }
    }

    public string GetRank()
    {
        return "C";
    }
}