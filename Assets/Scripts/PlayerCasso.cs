using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCasso : MonoBehaviour
{
    private const float STAGE_MINIMUM_X = -10.0f;

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

    // Outta 100.
    public float bloodLevelWifeHealth = 100.0f;

    // Animation
    private SpriteRenderer spriteRendererComponent;
    private Dictionary<string, Sprite[]> walkSprites = new Dictionary<string, Sprite[]>();
    private Dictionary<string, Sprite> idleSprites = new Dictionary<string, Sprite>();
    private Sprite[] jumpSprites;
    private Sprite[] biteSprites;

    private int currentAnimationFrame = 0;
    private int animationFrameInterval = 15;

    private string GetBloodLevel()
    {
        // fill in based on health
        return Globals.SpriteKeys.MID_BLOOD;
    }

    private bool ShouldJump()
    {
        // check isSucking() later, isCutScene(), etc.
        // && this.rigidbodyComponent.velocity.y == 0; -- sticky
        return this.isGrounded &&
            (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W));
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

        Debug.Log("Position is: " + this.rigidbodyComponent.position.x);
        if (this.rigidbodyComponent.position.x > 100.0) {        
            SceneManager.LoadScene("Outro");
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

    private bool IsIdleMovement()
    {
        return this.rigidbodyComponent.velocity == Vector2.zero;
    }

    private void ClampToStageBounds()
    {
        if (transform.position.x < STAGE_MINIMUM_X)
        {
            transform.position = new Vector2(STAGE_MINIMUM_X + 0.1f, transform.position.y);
            this.rigidbodyComponent.velocity = new Vector2(0.0f, this.rigidbodyComponent.velocity.y);
        }
        else // else if somehow past right bounds
        {

        }
    }

    private void LoadPlayerSprites()
    {
        // Sprite lowIdle = Resources.Load("Sprites/Low/idle") as Sprite;
        // Sprite lowWalk1 = Resources.Load("Sprites/Low/walk_1") as Sprite;
        // Sprite lowWalk2 = Resources.Load("Sprites/Low/walk_2") as Sprite;

        // Sprite midIdle = Resources.Load("Sprites/Mid/idle") as Sprite;
        // Sprite midWalk1 = Resources.Load("Sprites/Mid/walk_1") as Sprite;
        // Sprite midWalk2 = Resources.Load("Sprites/Mid/walk_2") as Sprite;

        // Sprite highIdle = Resources.Load("Sprites/High/idle") as Sprite;
        // Sprite highWalk1 = Resources.Load("Sprites/High/walk_1") as Sprite;
        // Sprite highWalk2 = Resources.Load("Sprites/High/walk_2") as Sprite;

        // // inverse order
        // Sprite jump1 = Resources.Load("Sprites/Jump/jump_3") as Sprite;
        // Sprite jump2 = Resources.Load("Sprites/Jump/jump_2") as Sprite;
        // Sprite jump3 = Resources.Load("Sprites/Jump/jump_1") as Sprite;

        // Sprite bite1 = Resources.Load("Sprites/Bite/bite_1") as Sprite;
        // Sprite bite2 = Resources.Load("Sprites/Bite/bite_2") as Sprite;
        // Sprite bite3 = Resources.Load("Sprites/Bite/bite_3") as Sprite;

        // Sprite[] lowWalkSprites = new Sprite[] { lowWalk1, lowWalk2 };
        // Sprite[] midWalkSprites = new Sprite[] { midWalk1, midWalk2 };
        // Sprite[] highWalkSprites = new Sprite[] { highWalk1, highWalk2 };

        // this.jumpSprites = new Sprite[] { jump1, jump2, jump3 };
        // this.biteSprites = new Sprite[] { bite1, bite2, bite3 };

        // Debug.Log("Low sprites " + lowWalkSprites);
        // Debug.Log(" Dict " + this.idleSprites.Count);

        // this.walkSprites.Add(Globals.SpriteKeys.LOW_BLOOD, lowWalkSprites);
        // this.walkSprites.Add(Globals.SpriteKeys.MID_BLOOD, midWalkSprites);
        // this.walkSprites.Add(Globals.SpriteKeys.HIGH_BLOOD, highWalkSprites);

        // this.idleSprites.Add(Globals.SpriteKeys.LOW_BLOOD, lowIdle);
        // this.idleSprites.Add(Globals.SpriteKeys.MID_BLOOD, midIdle);
        // this.idleSprites.Add(Globals.SpriteKeys.HIGH_BLOOD, highIdle);
        // Debug.Log(" Dict2 " + this.idleSprites.Count);
        // this.idleSprites.ToList().ForEach(x => Debug.Log("key " + x.key + " value " + x.value));
    }

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbodyComponent = GetComponent<Rigidbody2D>();
        this.collider2DComponent = GetComponent<Collider2D>();
        this.spriteRendererComponent = GetComponent<SpriteRenderer>();

        LoadPlayerSprites();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldJump())
        {
            MoveJump();
        }

        Globals.PlayerDirection playerDirection = Globals.PlayerDirection.RIGHT;

        if (ShouldMoveRight())
        {
            MoveRightClamped();
            playerDirection = Globals.PlayerDirection.RIGHT;
        }

        if (ShouldMoveLeft())
        {
            MoveLeftClamped();
            playerDirection = Globals.PlayerDirection.LEFT;
        }

        // Must come last
        ClampToStageBounds();

        // TODO
        bool isBiting = false;

        Globals.PlayerAction playerAction;

        if (isGrounded)
        {
            if (IsIdleMovement())
            {
                playerAction = Globals.PlayerAction.IDLE;
            }
            else
            {
                playerAction = isBiting ? Globals.PlayerAction.BITING : Globals.PlayerAction.WALKING;
            }
        }
        else
        {
            playerAction = Globals.PlayerAction.JUMPING;
        }

        switch(playerDirection)
        {
            case Globals.PlayerDirection.RIGHT:
                this.spriteRendererComponent.flipX = true;
                break;
            case Globals.PlayerDirection.LEFT:
                this.spriteRendererComponent.flipX = false;
                break;
        }

        // AnimatePlayer(playerDirection, playerAction);
    }

    void AnimatePlayer(Globals.PlayerDirection playerDirection, Globals.PlayerAction playerAction)
    {
        // if (Time.frameCount % this.animationFrameInterval == 0)
        // {
        //     ++this.currentAnimationFrame;
        // }

        // string bloodLevel = GetBloodLevel();
        // Sprite spriteToRender = this.idleSprites[bloodLevel];

        // switch(playerAction)
        // {
        //     case Globals.PlayerAction.BITING:
        //         spriteToRender = this.biteSprites[this.currentAnimationFrame % (this.biteSprites.Length - 1)];
        //         break;
        //     case Globals.PlayerAction.IDLE:
        //         spriteToRender = this.idleSprites[bloodLevel];
        //         break;
        //     case Globals.PlayerAction.JUMPING:
        //         spriteToRender = this.jumpSprites[this.currentAnimationFrame % (this.jumpSprites.Length - 1)];
        //         break;
        //     case Globals.PlayerAction.WALKING:
        //         Sprite[] currentWalkSprites = this.walkSprites[bloodLevel];
        //         spriteToRender = currentWalkSprites[this.currentAnimationFrame % (this.jumpSprites.Length - 1)];
        //         break;
        // }

        // switch(playerDirection)
        // {
        //     case Globals.PlayerDirection.RIGHT:
        //         this.spriteRendererComponent.flipX = true;
        //         break;
        //     case Globals.PlayerDirection.LEFT:
        //         this.spriteRendererComponent.flipX = false;
        //         break;
        // }

        // this.spriteRendererComponent.sprite = spriteToRender;
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
