using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FallState", menuName = "States/PlayerStates/Fall")]
public class FallPlayerState : PlayerState
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
        playerMovement.PlayerMove();
    }
}
