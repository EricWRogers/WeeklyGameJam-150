using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : SingletonMonoBehaviour<BackgroundMusic>
{
    new void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
