using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "BaseState", menuName = "State/Base")]
public class PlayerState : State
{
    protected PlayerMovement playerMovement = null;
    protected float timeFromStart = 0f;

    public override void OnEnter(GameObject _g)
    {
        timeFromStart = 0f;
        playerMovement = _g.GetComponent<PlayerMovement>();
    }
    public override void OnExit()
    {
        
    }
    public override void OnUpdate()
    {
        timeFromStart += Time.deltaTime;
    }

    protected float GetNormalizedAnimationTime(Animator _a)
    {
        Debug.LogError("problematic, causes states to skip since they check the privioues state's time insted of their own, opt to use manual time and events");
        if (_a.GetAnimatorTransitionInfo(0).anyState)
        {
            return _a.GetNextAnimatorStateInfo(0).normalizedTime;}
        else
        {
            return _a.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
     }
}
