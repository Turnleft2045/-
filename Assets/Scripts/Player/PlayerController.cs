using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("监听事件")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO afterSceneLoadedEvent;
    public VoidEventSO loadDataEvent;
    public PlayerInputControl inputControl;
    public VoidEventSO backToMenuEvent;

    private Rigidbody2D rb;

    public Vector2 inputDirection;

    private PhysicsCheck physicsCheck;

    private PlayerAnimation playerAnimation;
    public CapsuleCollider2D coll;

    [Header("基本参数")]

    public float speed;

    public float jumpforce;

    public float hurtforce;
    public bool ishurt;
    public bool isDead;

    public bool isAttack;

    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();
        coll= GetComponent<CapsuleCollider2D>();
        inputControl = new PlayerInputControl();
        //跳跃
        inputControl.GamePlay.Jump.started += Jump;
        //攻击
        inputControl.GamePlay.Attack.started += PlayerAttack;

        inputControl.Enable();

    }

   

    private void OnEnable()
    {
        
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        backToMenuEvent.OnEventRaised += OnLoadDataEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        sceneLoadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        backToMenuEvent.OnEventRaised -= OnLoadDataEvent;
    }
    
    private void OnLoadDataEvent()
    {
        isDead = false;
    }

    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();

        CheckState();
    }

    private void FixedUpdate()
    {
        if(!ishurt && ! isAttack)
            move();
    }

    private void OnLoadEvent(GameSceneSO arg0,Vector3 arg1,bool arg2)
    {
        inputControl.GamePlay.Disable();
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputControl.GamePlay.Enable();
    }
    public void move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        //人物翻转
        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    private void Jump(InputAction.CallbackContext context)
    {
        if (physicsCheck.isGround)
            rb.AddForce(transform.up * jumpforce, ForceMode2D.Impulse);
    }



    public void PlayerAttack(InputAction.CallbackContext obj)
    {
        playerAnimation.PlayAttack();
        isAttack = true;
      

    }




    public void GetHurt(Transform attacker)
    {
        ishurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;

        rb.AddForce(dir * hurtforce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.GamePlay.Disable();

    }

    public void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }
}
