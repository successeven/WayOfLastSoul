﻿
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected float _speed;



    public virtual void Die()
    {
				GetComponent<Rigidbody2D>().gravityScale = 1;
        Invoke("DestroyObject", 4f);
    }

    protected virtual void DestroyObject()
    {
        Destroy(transform.root.gameObject);
    }

    public virtual void Move(Rigidbody2D inBody, float inSpeed, ref bool inRigthPosition, float inMoveDirection)
    {
        inBody.velocity = new Vector2(inMoveDirection * inSpeed, inBody.velocity.y);
								
				if (inMoveDirection > 0 && !inRigthPosition)
            Flip(ref inRigthPosition);
        else if (inMoveDirection < 0 && inRigthPosition)
            Flip(ref inRigthPosition);
    }

    protected void Flip(ref bool inRigthPosition)
    {
        // Switch the way the player is labelled as facing.
        inRigthPosition = !inRigthPosition;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.root.localScale;
        theScale.x *= -1;
        transform.root.localScale = theScale;
    }
}
