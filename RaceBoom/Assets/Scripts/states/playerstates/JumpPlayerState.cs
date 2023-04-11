using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpState", menuName = "States/PlayerStates/Jump")]
public class JumpPlayerState : PlayerState
{
    
    public override void OnEnter(GameObject _g)
    {
        base.OnEnter(_g);
        //SoundPool.instance.PlaySound("clip1");
       
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
