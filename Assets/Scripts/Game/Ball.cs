using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    #region Components

    // rigidBody2D Unity Physic System
    public Rigidbody2D rigid;

    // animator & sprite => image render
    public Animator animator;
    public SpriteRenderer characterRenderer;

    // Particle System -> explode effect when win
    public ParticleSystem winExplosion;
    public bool isExplode = false;

    #endregion

    #region Variables

    // Coordinate in Oxy
    private Vector2 previousPosition;
    private Vector2 currentPosition;

    // Get rotation data
    private Quaternion originalRotation;

    // Transform manage ball's action
    [SerializeField] private Transform character;

    public bool isMoveLeft;
    public bool isRun;

    #endregion

    #region MonoBehaviours

    // Awake -> Enable -> Start -> Update -> FixedUpdate -> LateUpdate -> OnDisable -> OnDestroy
    private void Awake()
    {
        // assign the initial rotation
        originalRotation = transform.rotation;
    }

    void Start()
    {
        // Start a routine to manage the ball's movement
        StartCoroutine(CheckDirection());
    }

    // OnCollisionEnter2D: Handle the collision between balls
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            StartCoroutine(Win());
        }
    }

    IEnumerator Win()
    {
        if (isExplode) yield break;
        // keep the position 
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        winExplosion.Play(true); // play the particle effect
        AudioController.instance.PlayWinSound(); // play the sound effect
        isExplode = true;
        
        yield return new WaitForSeconds(1.75f);
        
        winExplosion.Stop();
        GameManager.instance.WinGame();
    }

    private void Update()
    {
        // keep the player stand straight up
        character.transform.rotation = originalRotation; 
    }

    #endregion

    #region Methods

    IEnumerator CheckDirection()
    {
        isRun = false;
        while (true)
        {   
            // get data to compare between 0.05s
            previousPosition = transform.position;
            yield return new WaitForSeconds(0.05f);
            currentPosition = transform.position;

            //Check player facing left or right
            if (currentPosition.x > previousPosition.x && isMoveLeft)
            {
                isMoveLeft = false;
                TurnPlayerDirection("Right");
            }
            else if (currentPosition.x < previousPosition.x && !isMoveLeft)
            {
                isMoveLeft = true;
                TurnPlayerDirection("Left");
            }
            
            if (Math.Abs(currentPosition.x - previousPosition.x) < 0.01f &&
                Math.Abs(currentPosition.y - previousPosition.y) < 0.01f) // fix floating point comparison 
            {
                if (isRun)
                {
                    isRun = false;
                    SetTriggerAnimator("Idle");
                }
            }
            else
            {
                if (!isRun)
                {
                    isRun = true;
                    SetTriggerAnimator("Run");
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void TurnPlayerDirection(string dir)
    {
        // flip the direction of the player's image
        if (dir == "Left")
        {   
            characterRenderer.flipX = true;
            return;
        }

        if (dir == "Right")
        {
            characterRenderer.flipX = false;
            return;
        }
    }

    private void SetTriggerAnimator(string triggerName)
    {   
        // Set the player's animation
        animator.ResetAllAnimatorParameters(AnimatorControllerParameterType.Trigger);
        animator.SetTrigger(triggerName);
    }

    // If interactive = true -> enable the physic system (gravity,....). When start drawing
    public void IsInteractive(bool isUsing)
    {
        if (isUsing)
        {
            rigid.simulated = true;
            rigid.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rigid.simulated = false;
            rigid.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    #endregion
}