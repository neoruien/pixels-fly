using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class PlayerController : Agent
{
    public int numLives = 3;
    public int numCoins;

    protected PlayerManager playerManager;
    protected CharacterController controller;
    protected Vector3 direction;
    public float defaultForwardSpeed;
    public float forwardSpeed = 20f;
    public float maxSpeed = 100f;
    public float acceleration = 0.01f;

    public TileManager tm;
    protected int desiredLane = 1; // three lanes: 0, 1, 2

    public float maxHeight = 15f;
    public GameObject maxHeightPrompt;
    public GameObject respawnCountdown;
    public float jumpForce;
    public float gravity = -20f;

    protected ControllerColliderHit prevHit;
    protected Quaternion defaultRotation;
    protected bool wasOnGround;
    protected GameObject currSkin;
    protected GameObject leftWing;
    protected GameObject rightWing;

    private AudioManager audioManager;
    protected Animation playerDies;

    public float maxTime;
    public float minSwipeDist;
    protected float startTime;
    protected float endTime;
    protected Vector3 startPos;
    protected Vector3 endPos;
    protected float swipeDistance;
    protected float swipeTime;

    // Start is called before the first frame update
    protected void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        controller = GetComponent<CharacterController>();
        defaultForwardSpeed = forwardSpeed;
        defaultRotation = transform.rotation;
        wasOnGround = false;
        numCoins = 0;

        // set player skin and abilities
        if (playerManager.userData.user.skins[0] == -1)
        {
            currSkin = GameObject.Find("Player").transform.GetChild(0).gameObject;
            GameObject.Find("Abilities").transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            currSkin = GameObject.Find("Player").transform.GetChild(playerManager.userData.user.skins[0]).gameObject;
            GameObject.Find("Abilities").transform.GetChild(playerManager.userData.user.skins[0]).gameObject.SetActive(true);
        }
        currSkin.SetActive(true);
        leftWing = currSkin.transform.GetChild(1).gameObject;
        rightWing = currSkin.transform.GetChild(2).gameObject;

        playerDies = GameObject.Find("Canvas").GetComponent<Animation>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (transform.position.y >= maxHeight) maxHeightPrompt.SetActive(true);
        else maxHeightPrompt.SetActive(false);

        direction.z = forwardSpeed;
        if (forwardSpeed < maxSpeed) forwardSpeed += acceleration;
        
        // Mobile phone touch controls
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;
                endPos = touch.position;

                swipeDistance = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;

                if (swipeTime < maxTime && swipeDistance > minSwipeDist)
                {
                    if (endPos.x > startPos.x && desiredLane < 2) moveRight();
                    if (endPos.x < startPos.x && desiredLane > 0) moveLeft();
                } else if (transform.position.y < maxHeight)
                {
                    Jump();
                }
            }
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) &&
            transform.position.y < maxHeight) Jump(); // can only jump with y < maxHeight

        if (!controller.isGrounded)
        {
            direction.y += gravity * Time.deltaTime;
            if (wasOnGround) walkToFly();
        }
        else if (controller.isGrounded)
        {
            wasOnGround = true;
            currSkin.GetComponent<Animation>().Play(currSkin.name + " Walk");
        }

        // gather the inputs on which lane we should be
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && desiredLane < 2) moveRight();
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && desiredLane > 0) moveLeft();

        // calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + 
                                 transform.position.y * transform.up;
        if (desiredLane == 0) targetPosition += Vector3.left * tm.laneDistance;
        else if (desiredLane == 2) targetPosition += Vector3.right * tm.laneDistance;
        
        // transform.position = Vector3.Lerp(transform.position, targetPosition, 500 * Time.deltaTime);
        if (transform.position == targetPosition) return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude) controller.Move(moveDir);
        else controller.Move(diff);
    }

    protected void FixedUpdate()
    {
        controller.Move(direction * Time.fixedDeltaTime);
    }

    protected void walkToFly()
    {
        wasOnGround = false;
        transform.rotation = defaultRotation;
        leftWing.SetActive(true);
        rightWing.SetActive(true);
        currSkin.GetComponent<Animation>().Play(currSkin.name + " Fly");
    }

    public void Jump()
    {
        currSkin.GetComponent<Animation>().Play(currSkin.name + " Fly");
        direction.y = jumpForce;
    }

    public void moveRight()
    {
        desiredLane++;
    }

    public void moveLeft()
    {
        desiredLane--;
    }

    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            playerDies.Play();
            audioManager.Hit();

            if (numLives <= 1)
            {
                playerManager.gameOver = true;
                audioManager.Gameover();
            }
            else
            {
                Debug.Log(numLives + "");
                enabled = false;
                Time.timeScale = 0;
                Invoke("Revive", 0.5f);
                prevHit = hit;
                hit.transform.tag = "Untagged";
            }
        }
    }

    protected void Revive()
    {
        float respawnHeight = 5f;
        numLives--;
        transform.position = new Vector3(transform.position.x, respawnHeight, transform.position.z - (tm.tileLength / 2));
        Invoke("Countdown3", 0.5f);
        audioManager.Rip();
    }

    protected void Countdown3()
    {
        respawnCountdown.SetActive(true);
        respawnCountdown.transform.GetChild(1).GetComponent<Text>().text = "3";
        Invoke("Countdown2", 0.3f);
    }

    protected void Countdown2()
    {
        respawnCountdown.transform.GetChild(1).GetComponent<Text>().text = "2";
        Invoke("Countdown1", 0.3f);
    }

    protected void Countdown1()
    {
        respawnCountdown.transform.GetChild(1).GetComponent<Text>().text = "1";
        Invoke("enableMovement", 0.3f);
    }

    protected void enableMovement()
    {
        respawnCountdown.SetActive(false);
        forwardSpeed = defaultForwardSpeed;
        enabled = true;
        Time.timeScale = 1;
        prevHit.transform.tag = "Obstacle";
    }
}
