using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "States/PlayerStates/Idle")]
public class IdlePlayerState : PlayerState
{
    
    public override void OnEnter(GameObject _g)
    {
        base.OnEnter(_g);
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        playerMovement.Gravity();
        //playerMovement.PlayerMove();
    }
}
