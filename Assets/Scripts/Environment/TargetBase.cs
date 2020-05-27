using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBase : SingletonMonoBehaviour<TargetBase>
{
    [Header("References")] public SphereCollider safeAreaCollider;
    public SphereCollider musicAreaTrigger;

    [Header("Zone Stats")] public float safeRadius;
    public float musicRadius;
    public float playerGuardingRadius;

    public bool isPlayerGuarding => Vector3.Distance(transform.position, PlayerModel.Instance.transform.position) <= playerGuardingRadius;

    new void Awake()
    {
        base.Awake();
        safeAreaCollider.radius = safeRadius;
        musicAreaTrigger.radius = musicRadius;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        DebugExtension.DebugWireSphere(transform.position, Color.green, safeRadius);
        DebugExtension.DebugWireSphere(transform.position, Color.blue, musicRadius);
        DebugExtension.DebugWireSphere(transform.position, Color.red, playerGuardingRadius);
    }

    public void OnEnterMusicArea(Collider other)
    {
        // TODO (Azee): Affect the player
    }

    public void OnExitMusicArea(Collider other)
    {
    }
}