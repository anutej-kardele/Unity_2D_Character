using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Transform headObject;
    private Rigidbody2D playerRigidbody;
    private RaycastHit2D hit;
    private float rayDistance = 0.5f;

    // Speed data 
    private float movementSpeed = 5f;
    private float movementSpeedWalk = 5f;
    private float movementSpeedRun = 10f;
    private float jumpWalk = 1200f;
    private float jumpRun = 1500f;

    // Input Data 
    private float movementHorizontal;
    private bool movementRun, movementJump, isWalkAble, isSealing;
    private Vector2 movement = Vector2.zero;

    private SpriteRenderer m_SpriteRenderer;

    private void Start() { 
        playerRigidbody = GetComponent<Rigidbody2D>();
        headObject = transform.GetChild(0);
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        hit = Physics2D.Raycast(headObject.position, Vector3.up, rayDistance);
        isSealing = (hit.collider != null) ? true : false;

        movementHorizontal = Input.GetAxis("Horizontal");
        if(movementHorizontal != 0) m_SpriteRenderer.flipX = movementHorizontal < 0 ? true : false;
        movementRun = (Input.GetKey(KeyCode.LeftShift)) ? true : false;
        movementJump = (Input.GetKey(KeyCode.Space)) ? true : false;

        movementSpeed = (movementRun) ? movementSpeedRun : movementSpeedWalk;
        movement.x = movementHorizontal * movementSpeed;

        if(isWalkAble) playerRigidbody.velocity = movement;
        if (movementJump && isWalkAble && !isSealing) playerRigidbody.AddForce(Vector2.up * ((movementRun) ? jumpRun : jumpWalk));
    }

    private void OnCollisionStay2D(Collision2D collision) { isWalkAble = (collision.gameObject.tag.Equals("GROUND")) ? true : false; }

    private void OnCollisionExit2D(Collision2D collision) { if (collision.gameObject.tag.Equals("GROUND")) isWalkAble = false; }
}
