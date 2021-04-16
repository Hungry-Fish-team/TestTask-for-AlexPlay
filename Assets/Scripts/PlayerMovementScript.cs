using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    bool isThisPlayerMoving = false;

    [SerializeField]
    string[] playerMovement = { "idle", "walk", "jump", "aim", "shoot", "aiming" };

    Player player;
    [SerializeField]
    string playerMove;

    private void Start()
    {
        player = GetComponent<Player>();

        playerMove = playerMovement[0];
    }

    public void SetPlayerControl(bool isThisPlayerMoving)
    {
        this.isThisPlayerMoving = isThisPlayerMoving;
        if (this.isThisPlayerMoving == false)
        {
            playerMove = playerMovement[0];
            DeletePlayerDirection();
        }
    }

    public bool ReturnPlayerControl()
    {
        return isThisPlayerMoving;
    }

    Touch touch;

    public Vector2 startPos;
    //public Vector2 direction;

    public int clickCount = 0;

    public GameObject playerPointer;
    GameObject playerInstantiatePointer;

    void Update()
    {
        PlayerMovement();
    }

    private void CreatePlayerDirection()
    {
        if (playerInstantiatePointer == null)
        {
            GameObject newPlayerPointer = Instantiate(playerPointer, transform);
            playerInstantiatePointer = newPlayerPointer;
        }
    }

    private void DeletePlayerDirection()
    {
        if (playerInstantiatePointer != null)
        {
            Destroy(playerInstantiatePointer.gameObject);
        }
    }

    private double FindLengthBetweenToV2(Vector2 start, Vector2 end)
    {
        //Debug.Log(Math.Sqrt(Math.Pow(end.x - start.x, 2) + Math.Pow(end.y - start.y, 2)));
        return Math.Sqrt(Math.Pow(end.x - start.x, 2) + Math.Pow(end.y - start.y, 2));
    }

    private double FindLengthBetweenToV3(Vector3 start, Vector3 end)
    {
        //Debug.Log(Math.Sqrt(Math.Pow(end.x - start.x, 2) + Math.Pow(end.z - start.z, 2)));
        return Math.Sqrt(Math.Pow(end.x - start.x, 2) + Math.Pow(end.z - start.z, 2));
    }

    private void PlayerMovement()
    {
        if (ReturnPlayerControl())
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        clickCount++;
                        startPos = touch.position;
                        StartCoroutine(DoubleClickTime());
                        //(startPos, startPos);
                        break;

                    case TouchPhase.Moved:
                        //direction = touch.position;
                        PlayerMove(startPos, touch.position);
                        break;

                    case TouchPhase.Stationary:
                        PlayerMove(startPos, touch.position);
                        break;

                    case TouchPhase.Ended:
                        PlayerShoots();
                        break;
                }
            }
        }
        //BulletsMoving(bullets, endPosBullets);
    }

    private void PlayerMove(Vector2 startPos, Vector2 endPos)
    {
        float buffNearPlayer = 1.5f;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(endPos);

        if (playerMove != playerMovement[3])
        {
            if (LeftSideInput(buffNearPlayer, worldPosition))
            {
                //Debug.Log("Right" + (transform.position.x - GetComponent<SpriteRenderer>().bounds.size.x * buffNearPlater) + " " + transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x * buffNearPlater + " " + worldPosition.x);
                if (playerMove != playerMovement[2])
                {
                    playerMove = playerMovement[1];
                }
                transform.position = new Vector2(transform.position.x + 0.05f, transform.position.y);
                RotationOurPlayer("Right");
            }
            else if (RightSideInput(buffNearPlayer, worldPosition))
            {
                //Debug.Log("Left" + (transform.position.x - GetComponent<SpriteRenderer>().bounds.size.x * buffNearPlater) + " " + transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x * buffNearPlater + " " + worldPosition.x);
                if (playerMove != playerMovement[2])
                {
                    playerMove = playerMovement[1];
                }
                transform.position = new Vector2(transform.position.x - 0.05f, transform.position.y);
                RotationOurPlayer("Left");
            }
            else if (AimSideInput(buffNearPlayer, worldPosition))
            {
                //Debug.Log("aim" + (transform.position.x - GetComponent<SpriteRenderer>().bounds.size.x * buffNearPlater) + " " + transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x * buffNearPlater + " " + worldPosition.x);
                if (playerMove == playerMovement[0])
                {
                    playerMove = playerMovement[3];
                    //CreatePlayerDirection();
                    //RotationOurPointer(startPos, endPos);
                    //StrengthOfMovement(startPos, endPos);
                }
                //StartCoroutine(WaitAimFunc());
            }
        }
        else if (playerMove == playerMovement[3])
        {
            CreatePlayerDirection();
            RotationOurPointer(startPos, endPos);
            StrengthOfMovement(startPos, endPos);
        }
    }

    bool LeftSideInput(float buffNearPlayer, Vector3 worldPosition)
    {
        return transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x * buffNearPlayer < worldPosition.x;
    }

    bool RightSideInput(float buffNearPlayer, Vector3 worldPosition)
    {
        return transform.position.x - GetComponent<SpriteRenderer>().bounds.size.x * buffNearPlayer > worldPosition.x;
    }

    bool AimSideInput(float buffNearPlayer, Vector3 worldPosition)
    {
        return transform.position.x + GetComponent<SpriteRenderer>().bounds.size.x * buffNearPlayer > worldPosition.x && transform.position.x - GetComponent<SpriteRenderer>().bounds.size.x < worldPosition.x && transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y * buffNearPlayer > worldPosition.y && transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y < worldPosition.y;
    }

    IEnumerator WaitAimFunc()
    {
        playerMove = playerMovement[3];

        yield return new WaitForSeconds(1.5f);

        if (playerMove == playerMovement[3])
        {
            playerMove = playerMovement[5];
        }
    }

    void PlayerShoots()
    {
        if (playerMove == playerMovement[3])
        {
            if (playerInstantiatePointer)
            {
                if (-playerInstantiatePointer.transform.localScale.x >= playerMoveDistance / 1.2)
                {
                    playerMove = playerMovement[4];

                    StartCoroutine("CreateBullet");
                }
                else
                {
                    playerMove = playerMovement[0];
                    DeletePlayerDirection();
                }
            }
        }
        else
        {
            playerMove = playerMovement[0];
        }
    }

    [SerializeField]
    Vector3 endPosBullets;
    [SerializeField]
    Vector3 startPosBullets;
    GameObject bullets;

    IEnumerator CreateBullet()
    {
        startPosBullets = transform.position;
        endPosBullets = TakeEndOfPointer();

        bullets = Instantiate(player.ReturnPlayerClassBullet(), startPosBullets, transform.GetChild(1).rotation, transform);
        SetPlayerControl(false);

        bullets.GetComponent<BulletsScript>().BulletMove(endPosBullets, gameObject);

        DeletePlayerDirection();

        GameObject.Find("GameManager").GetComponent<GameManager>().PlayerMadeMove();

        yield return new WaitForSeconds(5f);

        Destroy(bullets.gameObject);
    }

    //void BulletsMoving(GameObject bullets, Vector3 endPosBullets)
    //{
    //    if (bullets != null)
    //    {
    //        bullets.transform.position = Vector2.Lerp(bullets.transform.position, endPosBullets, Time.deltaTime * 5f);
    //    }
    //}

    void RotationOurPlayer(string direction)
    {
        if (direction == "Left")
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else if (direction == "Right")
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public float lookMoveSpeed;
    public int playerMoveSpeed;
    public float playerMoveDistance;

    private Vector3 TakeEndOfPointer()
    {
        if (playerInstantiatePointer != null)
        {
            return playerInstantiatePointer.transform.GetChild(1).position;
        }
        return transform.position;
    }

    private void RotationOurPointer(Vector2 start, Vector2 end)
    {
        //Debug.Log("Direction");
        double x = end.x - start.x;
        double y = end.y - start.y;
        double angle = Math.Atan(y / x) * 180 / Math.PI;
        if (!Double.IsNaN(angle))
        {
            if (x > 0)
            {
                RotationOurPlayer("Left");
                playerInstantiatePointer.transform.rotation = Quaternion.Euler(0, 0, 180 + (float)angle);
            }
            else
            {
                RotationOurPlayer("Right");
                playerInstantiatePointer.transform.rotation = Quaternion.Euler(0, 0, 180 + (float)angle);
            }
        }
    }

    private void StrengthOfMovement(Vector2 start, Vector2 end)
    {
        double length = FindLengthBetweenToV2(start, end);

        if (-playerInstantiatePointer.transform.localScale.x <= playerMoveDistance)
        {
            playerInstantiatePointer.transform.localScale = new Vector3(-(float)(length / (100 * lookMoveSpeed)), 1, 1);
        }
    }

    private IEnumerator DoubleClickTime()
    {
        yield return new WaitForSeconds(0.15f);

        if (clickCount == 1)
        {
        }
        if (clickCount > 1 || clickCount == 0)
        {
            playerMove = playerMovement[2];

            if (isGrounded)
            {
                PlayerJumpMove();
                playerMove = playerMovement[1];
            }
            else
            {
                playerMove = playerMovement[2];
            }

            StopCoroutine("DoubleClickTime");

        }
        clickCount = 0;
    }

    void PlayerJumpMove()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 250 * GetComponent<Rigidbody2D>().mass));
    }

    bool isGrounded;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            //Debug.Log("isGrounded");
            isGrounded = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            //Debug.Log("is not Grounded");
            isGrounded = false;
        }
    }


}

