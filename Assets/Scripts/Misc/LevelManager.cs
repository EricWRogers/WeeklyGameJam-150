using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public readonly string[] ranks = new[]
    {
        "A",
        "B",
        "C",
        "D",
        "E",
    };

    public readonly string perfectRank = "S";

    public AudioClip startClip;
    public Volume daylightVolume;

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
        SoundEffectsManager.Instance.Play(startClip);

        PlayerModel.Instance.onCamperEaten += camper => { OnCampersRemainingChanged(); };
        CamperManager.Instance.onCamperSafe += camper => { OnCampersRemainingChanged(); };
    }

    // Update is called once per frame
    void Update()
    {
        daylightVolume.weight = 1.0f - HUDManager.Instance.timeLeftNormalized;
    }

    public void PlayGameOverSequence(string reason)
    {
        isGameEnded = true;

        HUDManager.Instance.ShowEndScreen(reason);
    }

    void OnCampersRemainingChanged()
    {
        if (campersRemaining <= 0)
        {
            PlayGameOverSequence("No more campers available to hunt!");
        }
    }

    public string GetRank()
    {
        if (PlayerModel.Instance.campersEaten == CamperManager.Instance.campersCount)
        {
            return perfectRank;
        }

        var rankIndex = (PlayerModel.Instance.campersEaten * ranks.Length) / CamperManager.Instance.campersCount;
        rankIndex = ranks.Length - rankIndex;
        return ranks[Mathf.Clamp(rankIndex, 0, ranks.Length - 1)];
    }
}