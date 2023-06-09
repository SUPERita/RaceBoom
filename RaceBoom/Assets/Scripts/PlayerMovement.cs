using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
    [FoldoutGroup("Attacking")]
    [SerializeField] private Vector3 attackCorrectionSlide = Vector3.back;
    [FoldoutGroup("Attacking")]
    [SerializeField] private float attackCorrectionDur = .1f;

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

    [FoldoutGroup("Moving")]
    [SerializeField] private float speed = 5f;
    [FoldoutGroup("Moving")]
    [SerializeField] private float moveDistance = 1f;
    [FoldoutGroup("Moving")]
    [SerializeField] private float slideSpeed = .25f;

    [FoldoutGroup("Rolling")]
    [SerializeField] private float rollSpeed = 1f;
    [FoldoutGroup("Rolling")]
    [SerializeField] private float rolldur = 1f;
    [FoldoutGroup("Rolling")]
    [SerializeField] private float rollHeightChange = .5f;
    [FoldoutGroup("Rolling")]
    [SerializeField] private float rollCenterChange = .5f;
    private float startHeight = 1f;
    private float startCenter = 1f;


    [FoldoutGroup("Params")]
    [SerializeField] private Transform feetTransform = null;
    [FoldoutGroup("Params")]
    [field:SerializeField, FoldoutGroup("Params")] public Animator anim { get; private set; } = null;
    [field:SerializeField, FoldoutGroup("Params")] public CharacterController controller { get; private set; } = null;
    [FoldoutGroup("Params")]
    [SerializeField] private DestructibleChecker destructibleChecker = null;
    [FoldoutGroup("Params")]
    [SerializeField] private Joystick joystick = null;
    [FoldoutGroup("Params")]
    [SerializeField] private GameEventManager gameEventManager = null;
    [FoldoutGroup("Params")]
    [SerializeField] private Transform spawnPoint = null;
    [FoldoutGroup("Params")]
    [SerializeField] private HighscoreDisplay highscoreDisplay = null;

    //-1 1
    private int currentLane = 0;
    //[SerializeField] private Vector3 moveDirection = Vector3.forward;
    private bool inputLock = false;
    private bool isGrounded = true;
    private bool wasGroundedLastFrame = true;
    private bool isAlive = true;

    [Button]
    private void Editor_CacheParams()
    {
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        joystick = GameObject.FindObjectOfType<Joystick>();
        destructibleChecker = GetComponentInChildren<DestructibleChecker>();
        //no connection to the resources folder, i think
        gameEventManager = Resources.FindObjectsOfTypeAll<GameEventManager>()[0];
        highscoreDisplay = FindObjectsOfType<HighscoreDisplay>()[0];


        Debug.Log("params cached");
    }

    [FoldoutGroup("VFX")]
    [SerializeField] private MMF_Player KickFeedback = null;
    [FoldoutGroup("VFX")]
    [SerializeField] private GameObject dropFeedback = null;
    [FoldoutGroup("VFX")]
    [SerializeField] private MMF_Player deathFeedback = null;
    //[AssetList(Path = "Assets/Scripts/states/playerstates", AutoPopulate = true)]
    [SerializeField] private PlayerState[] states = null;
    private PlayerState currentState = null;

    private void OnDisable()
    {
        destructibleChecker.OnTriggerDestructible -= DestructibleChecker_OnTriggerDestructible;
        gameEventManager.OnGameStart -= GameEventManager_OnGameStart;
    }
    private void Start()
    {
        destructibleChecker.OnTriggerDestructible += DestructibleChecker_OnTriggerDestructible;
        gameEventManager.OnGameStart += GameEventManager_OnGameStart;
        //Debug.Log("need a better way to get isGrounded"); // DONE V
        SetState("Idle");

        startHeight = controller.height;
        startCenter = controller.center.y;
    }


    private void Update()
    {
        SetIsGrounded();
        if (Input.GetMouseButtonUp(0)) { inputLock = false; }
        //PlayAnimation(currentState);
        StateDecider();

        //Gravity(); now from inside states
        currentState.OnUpdate();

        //update score
        highscoreDisplay.UpdateDisplay(transform.position.z*2f);

        //just also do it
        wasGroundedLastFrame = isGrounded;
    }

    private bool rollRequest = false;
    private void StateDecider()
    {
        //slide
        if (!inputLock && Mathf.Abs(joystick.Horizontal) > .5f &&
            !currentState.blocksSliding)
        {
            if (joystick.Horizontal > 0) { currentLane++; }
            else { currentLane--; }
            currentLane = Mathf.Clamp(currentLane, -1, 1);

            SoundPool.instance.PlaySound("whoosh1", .1f);
            //Debug.Log(currentLane * moveDistance + " -- " + moveDur);
            //StartCoroutine(SlideTo(currentLane * moveDistance));
            transform.DOMoveX(/*transform.position.x + (Mathf.Sign(joystick.Horizontal))*/currentLane *5,slideSpeed).SetEase(Ease.OutCubic);
            inputLock = true;
        }
        if (!inputLock && Mathf.Abs(joystick.Vertical) > .5f)
        {
            //jump
            if (joystick.Vertical > 0 && isGrounded && !currentState.blocksJumping)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                inputLock = true;
                SoundPool.instance.PlaySound("whoosh2", .2f);
            }
            //drop
            else if (joystick.Vertical < 0 && !isGrounded && !currentState.blocksRolling)
            {
                _velocity.y = gravity;
                inputLock = true;
                rollRequest = true;
                SoundPool.instance.PlaySound("whoosh1", .2f);
            } else if (joystick.Vertical < 0 && isGrounded && !currentState.blocksRolling)
            {
                if(currentState.stateName != "Roll")
                {
                    SetState("Roll");
                    SoundPool.instance.PlaySound("whoosh2", .2f);
                }
            }
        }


        if(rollRequest && isGrounded) { SetState("Roll"); rollRequest = false; }
        if (Input.GetKeyDown(KeyCode.Space)) { SetState("Attack" + UnityEngine.Random.Range(1, 4)); }
        if (isGrounded) { SetState("Run"); }
        if (_velocity.y > 0 && !isGrounded) { SetState("Jump"); }
        if (_velocity.y < 0 && !isGrounded) { SetState("Fall"); }
        //
        if(currentState.stateName == "Jump" && _velocity.y < 0 && !isGrounded) { SetState("Fall", true); }

        if(isGrounded && !wasGroundedLastFrame)
        {
            SoundPool.instance.PlaySound("player_land", .2f);
            Instantiate(dropFeedback, transform.position, transform.rotation);
        }
        
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
        if(controller.enabled) { controller.Move(_velocity * Time.deltaTime); }
        
    }
    /*
    Vector3 slideVector = Vector3.zero;
    public IEnumerator SlideTo(float _toPos)
    {
        StopCoroutine(nameof(SlideTo));
        slideVector.x = _toPos;
        //slideVector.y = transform.position.y;
        //slideVector.z = transform.position.z;
        while (Mathf.Abs(transform.position.x-_toPos) > .1f)
        {
            controller.SimpleMove((slideVector).normalized*slideSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
    */
    private void PlayerDie()
    {
        deathFeedback?.PlayFeedbacks();
        SoundPool.instance.PlaySound("player_death");
        transform.DOKill();
        //controller.enabled = false;
        isAlive = false;
        SetState("Die");
        gameEventManager.Notify_OnGameOver();
    }
    private void RestartPlayer()
    {
        //transform.DOKill();
        Debug.Log("restarted");
        isAlive = true;
        SetState("Run", true);
            controller.enabled = false;
        //transform.localPosition = spawnPoint.transform.localPosition;//cant move player when controler is active
            controller.enabled = true;
        //gameEventManager.Notify_OnGameOver();
    }

    #region events

    private void DestructibleChecker_OnTriggerDestructible(Destructible obj)
    {
        SetState("Attack" + UnityEngine.Random.Range(1, 4));
        destructiblesInCheck.Add(obj);
        obj.Notify_ToDistruct();
        transform.DOMove(destructiblesInCheck[0].transform.position + attackCorrectionSlide, attackCorrectionDur);
        SoundPool.instance.ExantuateTheme();
    }
    private List<Destructible> destructiblesInCheck = new List<Destructible>();
    public void Anim_DestroyDestructiblesInTrigger()
    {
        if(destructiblesInCheck.Count > 0)
        {
            gameEventManager.Notify_OnDestructibleDestroyed();
            KickFeedback?.PlayFeedbacks();
            
            
        }
        foreach (Destructible _d in destructiblesInCheck)
        {
            _d.Destruct();
            ResourceManager.AddCoins(10);
            MessageManager.instance.SendMessage("+10$");
        }
        destructiblesInCheck = new List<Destructible>();
    }

    private void GameEventManager_OnGameStart()
    {
        RestartPlayer();
    }

    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isAlive) { return; }
        //Debug.Log("one");
        if (hit.collider.gameObject.CompareTag("Obstacle"))
        {
            //Debug.Log("one2" +   hit.collider.gameObject.name);
            PlayerDie();
        }
    }
    public void NotifyColliderWithPlayer()
    {
        if (!isAlive) { return; }
        PlayerDie();
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
    public void ResetGravity()
    {
        _velocity.y = 0f;
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
    public void PlayerHitBoxShorten(bool reset = false)
    {
        if (reset)
        {
            controller.height = startHeight;
            controller.center = startCenter * Vector3.up;
            return;
        } else
        {
            controller.height = rollHeightChange;
            controller.center = rollCenterChange * Vector3.up;
        }
        
    }

    #endregion
    
    private void Editor_PlayerAnim()
    {
        PlayAnimation("Attack1");
    }



}
