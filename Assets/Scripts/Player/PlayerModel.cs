using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : SingletonMonoBehaviour<PlayerModel>
{
    public PlayerMovement playerMovement { get; private set; }
    new void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
    }
}
