
using UnityEngine;

public class Unit : MonoBehaviour
{

    public virtual void ReceiveDamage()
    {
        Die();
    }

    protected virtual void Die()
    {
        Destroy(transform.root.gameObject);
    }


    protected void Move(Rigidbody2D inBody, float inSpeed, ref bool inRigthPosition, float inH)
    {
        inBody.velocity = new Vector2(inH * inSpeed, inBody.velocity.y);
        if (inH > 0 && !inRigthPosition)
            Flip(ref inRigthPosition);
        else if (inH < 0 && inRigthPosition)
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
