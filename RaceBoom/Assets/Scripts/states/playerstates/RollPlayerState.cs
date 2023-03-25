using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RollState", menuName = "States/PlayerStates/Roll")]
public class RollPlayerState : PlayerState
{
    [SerializeField] private float duration = .5f;
    public override void OnEnter(GameObject _g)
    {
        base.OnEnter(_g);
        playerMovement.PlayerHitBoxShorten();
    }
    public override void OnExit()
    {
        base.OnExit();
        playerMovement.PlayerHitBoxShorten(true);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        playerMovement.PlayerMove();
        playerMovement.Gravity();

        if (timeFromStart > duration)
        {
            playerMovement.ResetPlayerState();
        }
    }
}
