using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCasso : MonoBehaviour
{
    private Rigidbody2D rigidbodyComponent;
    private Collider2D collider2DComponent;

    private bool isGrounded = true;
    private float framesPerAccelerationIncrease = 45.0f;

    public float jumpAmount = 10.0f;
    public float maxHorizontalMoveSpeed = 6.0f;
    
    // If you're in the air, how much can you influence your movement? 
    // 1.0 = 100%
    // A hack, you can just mash the keys.
    public float maxAirMovementAdjustPercent = 0.25f;

    private bool ShouldJump()
    {
        // check isSucking() later, isCutScene(), etc.
        // && this.rigidbodyComponent.velocity.y == 0; -- sticky
        return this.isGrounded &&
            (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.W));
    }

    private bool ShouldMoveRight()
    {
        return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
    }

    private bool ShouldMoveLeft()
    {
        return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
    }

    private void MoveJump()
    {
        this.rigidbodyComponent.AddForce(Vector2.up * this.jumpAmount, ForceMode2D.Impulse);
    }

    private void MoveRightClamped()
    {
        if (this.rigidbodyComponent.velocity.x < this.maxHorizontalMoveSpeed)
        {
            Debug.Log("Velocity is " + this.rigidbodyComponent.velocity);
            float horizontalSpeedModifier = (this.maxHorizontalMoveSpeed / this.framesPerAccelerationIncrease);
            horizontalSpeedModifier *= this.isGrounded ? 1.0f : this.maxAirMovementAdjustPercent;

            this.rigidbodyComponent.AddForce(Vector2.right * horizontalSpeedModifier, ForceMode2D.Impulse);
        }
        else
        {
            this.rigidbodyComponent.velocity = new Vector2(this.maxHorizontalMoveSpeed, this.rigidbodyComponent.velocity.y);
        }
    }

    private void MoveLeftClamped()
    {
        // Apparently `Vector2.SmoothDamp()` is better for this.
        if (this.rigidbodyComponent.velocity.x > -this.maxHorizontalMoveSpeed)
        {
            Debug.Log("Velocity is " + this.rigidbodyComponent.velocity);
            float horizontalSpeedModifier = (this.maxHorizontalMoveSpeed / this.framesPerAccelerationIncrease);
            horizontalSpeedModifier *= this.isGrounded ? 1.0f : this.maxAirMovementAdjustPercent;

            this.rigidbodyComponent.AddForce(Vector2.left * horizontalSpeedModifier, ForceMode2D.Impulse);
        }
        else
        {
            this.rigidbodyComponent.velocity = new Vector2(-this.maxHorizontalMoveSpeed, this.rigidbodyComponent.velocity.y);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbodyComponent = GetComponent<Rigidbody2D>();
        this.collider2DComponent = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldJump())
        {
            MoveJump();
        }

        if (ShouldMoveRight())
        {
            MoveRightClamped();
        }

        if (ShouldMoveLeft())
        {
            MoveLeftClamped();
        }
    }

    void OnCollisionEnter2D(Collision2D theCollision)
    {
        if (theCollision.gameObject.CompareTag(Globals.Tags.GROUND))
        {
            this.isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D theCollision)
    {
        if (theCollision.gameObject.CompareTag(Globals.Tags.GROUND))
        {
            this.isGrounded = false;
        }
    }
}
