using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCasso : MonoBehaviour
{
    private Rigidbody2D rigidbodyComponent;
    private Collider2D collider2DComponent;

    private bool isGrounded = true;

    public float jumpAmount = 10;


    // Start is called before the first frame update
    void Start()
    {
        this.rigidbodyComponent = GetComponent<Rigidbody2D>();
        this.collider2DComponent = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.isGrounded)
        {
            this.rigidbodyComponent.AddForce(Vector2.up * this.jumpAmount, ForceMode2D.Impulse);
        }

        // if (Input.GetKeyDown(KEyCode.Space))
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
