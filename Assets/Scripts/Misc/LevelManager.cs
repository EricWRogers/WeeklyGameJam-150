using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public int campersRemaining =>
        Mathf.Max(CamperManager.Instance.campersCount - PlayerModel.Instance.campersEaten, 0);

    public Transform playerVisiblePoint => PlayerModel.Instance.visibilityCheckPoint;

    public bool isGameEnded { get; private set; } = false;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerModel.Instance.onCamperEaten += camper =>
        {
            if (campersRemaining <= 0)
            {
                PlayGameWonSequence();
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameOverSequence()
    {
        isGameEnded = true;
    }

    void PlayGameWonSequence()
    {
        isGameEnded = true;
    }
}
