
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected float _speed;
		
    public virtual void Move(Rigidbody2D inBody, float inSpeed, float inMoveDirection)
    {
        inBody.velocity = new Vector2(inMoveDirection * inSpeed, inBody.velocity.y);
				/*
        if (inMoveDirection > 0 && !inRigthPosition)
            Flip(ref inRigthPosition);
        else if (inMoveDirection < 0 && inRigthPosition)
            Flip(ref inRigthPosition);*/
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

		public void SetSpeed(float inSpeed)
		{
				_speed = inSpeed;
		}
}
