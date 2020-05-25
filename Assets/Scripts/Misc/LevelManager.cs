using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    [Header("Dev Tools")] [SerializeField] private Transform playerMockLocation;

    public Transform playerLocation => playerMockLocation ?? PlayerModel.Instance.transform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
