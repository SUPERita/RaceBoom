using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackState", menuName = "States/PlayerStates/Attack")]
public class AttackPlayerState : PlayerState
{
    [SerializeField] private float endTime = .5f;
    public override void OnEnter(GameObject _g)
    {
        base.OnEnter(_g);
    }
    public override void OnExit()
    {
        base.OnExit();
        playerMovement.ResetGravity();
        //playerMovement.ResetStateLock();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        //playerMovement.Gravity();
        //playerMovement.PlayerMove();
        //Debug.Log(GetNormalizedAnimationTime(playerMovement.anim));
        if (timeFromStart > endTime)
        {
            playerMovement.ResetPlayerState();
        }
    }
}
