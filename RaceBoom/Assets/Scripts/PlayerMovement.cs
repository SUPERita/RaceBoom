using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;


public class PlayerMovement : MonoBehaviour
{

    [FoldoutGroup("Jumping")]
    [SerializeField] private LayerMask groundLayer;
    [FoldoutGroup("Jumping")]
    [SerializeField] private float groundCheckRadius = .2f;
    [FoldoutGroup("Jumping")]
    [SerializeField] private Transform groundCheckPoint = null;
    [FoldoutGroup("Jumping")]
    [SerializeField] private float gravity = -9.81f;
    [FoldoutGroup("Jumping")]
    [SerializeField] private float jumpForce = 7f;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float moveDur = .25f;
    //[SerializeField] private Vector3 moveDirection = Vector3.forward;
    private bool inputLock = false;
    private bool isGrounded = true;
    
    [FoldoutGroup("Params")]
    [SerializeField] private Transform feetTransform = null;
    [FoldoutGroup("Params")]
    [field:SerializeField, FoldoutGroup("Params")] public Animator anim { get; private set; } = null;
    [field:SerializeField, FoldoutGroup("Params")] public CharacterController controller { get; private set; } = null;
    [FoldoutGroup("Params")]
    [SerializeField] private DestructibleChecker destructibleChecker = null;
    [FoldoutGroup("Params")]
    [SerializeField] private Joystick joystick = null;

    [Button]
    private void Editor_CacheParams()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        joystick = GameObject.FindObjectOfType<Joystick>();
        destructibleChecker = GetComponentInChildren<DestructibleChecker>();
        Debug.Log("params cached");
    }

    [FoldoutGroup("VFX")]
    [SerializeField] private MMF_Player KickFeedback = null;
    //[AssetList(Path = "Assets/Scripts/states/playerstates", AutoPopulate = true)]
    [SerializeField] private PlayerState[] states = null;
    private PlayerState currentState = null;

    private void Start()
    {
        destructibleChecker.OnTriggerDestructible += DestructibleChecker_OnTriggerDestructible;
        //Debug.Log("need a better way to get isGrounded"); // DONE V
        SetState(states[0]);

        
    }
    private void OnDisable()
    {
        destructibleChecker.OnTriggerDestructible -= DestructibleChecker_OnTriggerDestructible;
    }

    private void Update()
    {
        SetIsGrounded();
        if (Input.GetMouseButtonUp(0)) { inputLock = false; }
        //PlayAnimation(currentState);
        StateDecider();

        //Gravity(); now from inside states
        currentState.OnUpdate();
    }


    #region events

    private void DestructibleChecker_OnTriggerDestructible(Destructible obj)
    {
        SetState("Attack" + UnityEngine.Random.Range(1, 4));
        destructiblesInCheck.Add(obj);
    }
    private List<Destructible> destructiblesInCheck = new List<Destructible>();
    public void Anim_DestroyDestructiblesInTrigger()
    {
        if(destructiblesInCheck.Count > 0)
        {
            KickFeedback?.PlayFeedbacks();
        }
        foreach (Destructible _d in destructiblesInCheck)
        {
            _d.Destruct();
        }
        destructiblesInCheck = new List<Destructible>();
    }

    #endregion




    /*
    RaycastHit rInfo;
    private void SetIsGrounded()
    {
        isGrounded = Physics.Raycast(feetTransform.position, Vector3.down,out rInfo, groundCheckRadius, groundLayer);
        //Debug.Log(rInfo.collider.name);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(feetTransform.position, groundCheckRadius);
    }
    */

    private void StateDecider()
    {
        //slide
        if (!inputLock && Mathf.Abs(joystick.Horizontal) > .75f)
        {
            transform.DOMoveX(transform.position.x + (Mathf.Sign(joystick.Horizontal)) * moveDistance, moveDur).SetEase(Ease.OutCubic);
            inputLock = true;
        }
        if (!inputLock && Mathf.Abs(joystick.Vertical) > .75f)
        {
            //jump
            if (joystick.Vertical > 0 && isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                inputLock = true;
            }
            //roll
            else if (joystick.Vertical < 0 && !isGrounded)
            {
                _velocity.y = gravity;
                inputLock = true;
            }
        }
        


        if (Input.GetKeyDown(KeyCode.Space)) { SetState("Attack" + UnityEngine.Random.Range(1,4));  }
        if (isGrounded) { SetState("Run"); }
        if (_velocity.y > 0 && !isGrounded) { SetState("Jump"); }
        if (_velocity.y < 0 && !isGrounded) { SetState("Fall"); }
    }

    private Vector3 _velocity = Vector3.zero;
    public void Gravity()
    {
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Small offset to ensure the character is really on the ground
        }

        //Vector3 gravityOut = Vector3.up* Time.deltaTime*gravityMultiplayer*jumpTimeCounter ;

        //Vector3 gravityOut = Vector3.down * gravityMultiplayer * 9.81f;
        //gravityOut += Vector3.up * jumpForceCounter;
        //ravityOut *= Time.deltaTime;

        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }
    public void ResetGravity()
    {
        _velocity.y = 0f;
    }

    #region utils

    public void PlayerMove()
    {
        controller.Move(Vector3.forward * speed * Time.deltaTime);
    }

    private void SetState(PlayerState _state, bool forceState = false)
    {
        
        if (!forceState)
        {
            if(currentState != null)
            {
                //Debug.Log(currentState.priotiry + " - " + _state.priotiry);
                if(currentState == _state) { return; }
                if(currentState.priotiry > _state.priotiry) { return; }
                //if (stateLock) { return; }
            }
        }
        
        if(currentState != null){ currentState.OnExit(); }  
        
        currentState = _state;
        currentState.OnEnter(gameObject);
        PlayAnimation(currentState.stateName);
    }
    private void SetState(string _stateName, bool forceState = false)
    {
        SetState(GetStateFromName(_stateName), forceState);
    }
    private void PlayAnimation(string _stateName)
    {
        anim.CrossFadeInFixedTime(_stateName, .15f);
    }
    private PlayerState GetStateFromName(string _arg)
    {
        foreach(PlayerState _p in states)
        {
            if (_p.stateName == _arg) return _p;
        }
        Debug.Log("didnt find state named "+ _arg);
        return null;
    }

    public void ResetPlayerState()
    {
        SetState("Run", true);
    }
    /*
    public void ResetStateLock()
    {
        stateLock = false;
    }
    */
    private void SetIsGrounded()
    {
        isGrounded = Physics.Raycast(feetTransform.position, Vector3.down, groundCheckRadius, groundLayer);
        //isGrounded = controller.isGrounded;
    }

    #endregion

    
    private void Editor_PlayerAnim()
    {
        PlayAnimation("Attack1");
    }



}
