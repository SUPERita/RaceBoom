using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//[CreateAssetMenu(fileName = "BaseState", menuName = "State/Base")]
public class PlayerState : State
{
    protected PlayerMovement playerMovement = null;
    protected float timeFromStart = 0f;
    [field: SerializeField] public bool blocksSliding { get; private set; } = false;
    [field: SerializeField] public bool blocksJumping { get; private set; } = false;
    [field:SerializeField] public bool blocksRolling { get; private set; } = false;

    [SerializeField] private bool hasEffects = false;
    [LabelWidth(100)]
    [HorizontalGroup("a")]
    [ShowIf("hasEffects")]
    [SerializeField] private GameObject effectOnEnter = null;
    [LabelWidth(40)]
    [LabelText("height")]
    [HorizontalGroup("a")]
    [ShowIf("hasEffects")]
    [SerializeField] private float effectOnEnterHeight = 0f;
    [LabelWidth(100)]
    [HorizontalGroup("b")]
    [ShowIf("hasEffects")]
    [SerializeField] private GameObject effectOnExit = null;
    [LabelWidth(40)]
    [LabelText("height")]
    [HorizontalGroup("b")]
    [ShowIf("hasEffects")]
    [SerializeField] private float effectOnExitHeight = 0f;

    public override void OnEnter(GameObject _g)
    {
        timeFromStart = 0f;
        playerMovement = _g.GetComponent<PlayerMovement>();
        if (effectOnEnter) Instantiate(effectOnEnter, _g.transform.position + Vector3.up * effectOnEnterHeight, _g.transform.rotation);

    }
    public override void OnExit()
    {
        if (effectOnExit) Instantiate(effectOnExit, playerMovement.transform.position + Vector3.up * effectOnExitHeight, playerMovement.transform.rotation);
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
