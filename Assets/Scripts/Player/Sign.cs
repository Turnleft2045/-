using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Sign : MonoBehaviour
{
    public GameObject signSprite;
    public Transform playTrans;
    public PlayerInputControl playerInput;
    private Animator anim;
    private bool canPress;
    private IInteractable targetItem;

   private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
        playerInput.GamePlay.Confirm.started += OnConfirm;
    }

    private void OnDisable()
    {
        canPress = false;
    }
    private void Awake()
    {
        anim = signSprite.GetComponent<Animator>();
        playerInput = new PlayerInputControl();
        playerInput.Enable();
    }


    private void OnConfirm(InputAction.CallbackContext obj)
    {
        if (canPress)
        {

            targetItem.TriggerAction();
            GetComponent<AudioDefination>()?.PlayAudioClip();
        }
    }



    private void Update()
    {
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        signSprite.transform.localScale = playTrans.localScale;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Interactable"))
            {
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        
            canPress = false;
       
    }
   private void OnActionChange(object obj,InputActionChange actionChange)
    {
        if(actionChange==InputActionChange.ActionStarted)
        {
            //Debug.Log(((InputAction)obj).activeControl.device);

            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    anim.Play("keyboard");
                    break;
            }
        }
    }
}
