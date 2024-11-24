using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public CapsuleCollider2D coll;
    [Header("¼ì²â²ÎÊý")]
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRaduis;
    public LayerMask groundLayer;
    public bool maual;

    [Header("×´Ì¬")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        if(!maual)
        {
            rightOffset = new Vector2(coll.bounds.size.x/2+coll.offset.x, coll.bounds.size.y/2);
            leftOffset = new Vector2(-rightOffset.x, coll.bounds.size.y/2);    
        }

    }

    private void Update()
    {
        check();
    }

    public void check()
    {
       isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset* transform.localScale, checkRaduis,groundLayer);
       touchLeftWall= Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);
       touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2) transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis); 
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }

}
