using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    private Transform headObject;
    private Rigidbody2D playerRigidbody;
    private RaycastHit2D hit;
    private float rayDistance = 0.5f;

    // Inputs 
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Button _jumpTrigger;

    // Launcher 
    [SerializeField] private bool isInsideLanuncher = false;
    [SerializeField] private GameObject _arrowPointer;
    [SerializeField] private Transform _arrowHolder;

    // Speed data 
    private float movementSpeed = 5f;
    private float movementSpeedWalk = 10f;
    private float movementSpeedRun = 20f;
    private float jumpValue = 30f;
    private float launchForce = 2000f;

    // Input Data 
    private float movementHorizontal;
    //private bool movementRun, movementJump, isWalkAble, isSealing;
    private bool movementRun, isWalkAble, isSealing;
    private Vector2 movement = Vector2.zero;

    private SpriteRenderer m_SpriteRenderer;

    private void Start() { 
        playerRigidbody = GetComponent<Rigidbody2D>();
        headObject = transform.GetChild(0);
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        _jumpTrigger.onClick.AddListener(JumpFunction);
    }

    private void FixedUpdate()
    {
        movementHorizontal = _joystick.Horizontal;
        //movementRun = (Input.GetKey(KeyCode.LeftShift)) ? true : false;
        //movementJump = (Input.GetKey(KeyCode.Space)) ? true : false;

        hit = Physics2D.Raycast(headObject.position, Vector3.up, rayDistance);
        isSealing = (hit.collider != null) ? true : false;

        if(movementHorizontal != 0) m_SpriteRenderer.flipX = movementHorizontal < 0 ? true : false;

        movementSpeed = (movementRun) ? movementSpeedRun : movementSpeedWalk;
        movement.x = movementHorizontal * movementSpeed;

        if(isWalkAble && !stopMovement) playerRigidbody.velocity = movement;
        //if (movementJump && isWalkAble && !isSealing) playerRigidbody.AddForce(Vector2.up * ((movementRun) ? jumpRun : jumpWalk));

        // // Launcher 

        // if(Input.GetKeyDown(KeyCode.K) && isInsideLanuncher){
        //     Debug.Log("Paused");
        //     Time.timeScale = 0;
        // }

        // if(Input.GetKeyUp(KeyCode.K) && isInsideLanuncher){
        //     Debug.Log("Un-Paused");
        //     Time.timeScale = 1;
        // }

    }

    public string Horizontal;

    public void LauncherButton(int value){
        if(isInsideLanuncher) {
            Time.timeScale = value;
            _arrowPointer.SetActive((value == 0) ? true : false);


            if(value == 1){
                Vector2 launchDirection = new Vector2(_joystick.Horizontal, _joystick.Vertical);
                playerRigidbody.velocity = Vector3.zero;
                playerRigidbody.AddForce(launchDirection * launchForce);
            }
        }

    }

    private bool stopMovement = false;

    private void JumpFunction(){
        if (isWalkAble && !isSealing) {
            // playerRigidbody.AddForce(Vector2.up * ((movementRun) ? jumpRun : jumpWalk));
            //movement.y = (movementRun) ? jumpRun : jumpWalk;
            stopMovement = true;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpValue);
            Invoke(nameof(DisableStopMovement), 0.1f);
        }
    }

    private void DisableStopMovement(){
        stopMovement = false;
    }

    public void EnableRunning(){
        movementRun = true;
    }

    public void DisableRunning(){
        Invoke(nameof(DisableRun), 1.0f);
    }

    private void DisableRun(){
        movementRun = false;
    }



    private void OnCollisionStay2D(Collision2D collision) { isWalkAble = (collision.gameObject.tag.Equals("GROUND")) ? true : false; }

    private void OnCollisionExit2D(Collision2D collision) { if (collision.gameObject.tag.Equals("GROUND")) isWalkAble = false; }

    // Launcher

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Launcher")) {
            _arrowHolder = other.transform;
            _arrowPointer = _arrowHolder.GetChild(0).gameObject;
            isInsideLanuncher = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Launcher")) isInsideLanuncher = false;
    }

}
