using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private bool isOurPlayer = false;
    [SerializeField]
    private bool isPlayerAliveByAD = false;
    [SerializeField]
    private string playerNickName;
    [SerializeField]
    private string playerColor;
    [SerializeField]
    private string playerClass;
    [SerializeField]
    private GameObject playerClassBullet;
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerJumpPower;
    [SerializeField]
    private int playerHP;
    [SerializeField]
    private bool isPlayerAlive = true;

    [SerializeField]
    private float playerSpeedNormal;
    [SerializeField]
    private float playerJumpPowerNormal;
    [SerializeField]
    private int playerHPNormal;

    public void SetPlayerSpeedNormal(float playerSpeedNormal)
    {
        this.playerSpeedNormal = playerSpeedNormal;
    }

    public float ReturnPlayerSpeedNormal()
    {
        return this.playerSpeedNormal;
    }

    public void SetPlayerJumpPowerNormal(float playerJumpPowerNormal)
    {
        this.playerJumpPowerNormal = playerJumpPowerNormal;
    }

    public float ReturnPlayerJumpPowerNormal()
    {
        return this.playerJumpPowerNormal;
    }

    public void SetPlayerHPNormal(int playerHPNormal)
    {
        this.playerHPNormal = playerHPNormal;
    }

    public int ReturnPlayerHPNormal()
    {
        return this.playerHPNormal;
    }

    public void SetOurPlayerTrue(bool isOurPlayer)
    {
        this.isOurPlayer = isOurPlayer;
    }

    public bool ReturnOurPlayerTrue()
    {
        return this.isOurPlayer;
    }

    public void SetPlayerAliveByAD(bool isPlayerAliveByAD)
    {
        this.isPlayerAliveByAD = isPlayerAliveByAD;
    }

    public bool ReturnPlayerAliveByAD()
    {
        return this.isPlayerAliveByAD;
    }

    public void SetPlayerNickName(string playerNickName)
    {
        this.playerNickName = playerNickName;
    }

    public string ReturnPlayerNickName()
    {
        return this.playerNickName;
    }

    public void SetPlayerColor(string playerColor)
    {
        this.playerColor = playerColor;
    }

    public string ReturnPlayerColor()
    {
        return this.playerColor;
    }

    public void SetPlayerClass(string playerClass)
    {
        this.playerClass = playerClass;
    }

    public GameObject ReturnPlayerClassBullet()
    {
        return this.playerClassBullet;
    }

    public void SetPlayerClassBullet(GameObject playerClassBullet)
    {
        this.playerClassBullet = playerClassBullet;
    }

    public string ReturnPlayerClass()
    {
        return this.playerClass;
    }

    public void SetPlayerSpeed(float playerSpeed)
    {
        this.playerSpeed = playerSpeed;
    }

    public float ReturnPlayerSpeed()
    {
        return this.playerSpeed;
    }

    public void SetPlayerJumpPower(float playerJumpPower)
    {
        this.playerJumpPower = playerJumpPower;
    }

    public float ReturnPlayerJumpPower()
    {
        return this.playerJumpPower;
    }

    public void SetPlayerHP(int playerHP)
    {
        this.playerHP = playerHP;
    }

    public int ReturnPlayerHP()
    {
        return this.playerHP;
    }

    public bool ReturnPlayerStatus()
    {
        CheckPlayerStatus();

        return isPlayerAlive;
    }

    public void TakeDamageFormAnotherPlayer(int damage)
    {
        playerHP -= damage;

        CheckPlayerStatus();
    }

    public void CheckPlayerStatus()
    {
        if(playerHP <= 0)
        {
            isPlayerAlive = false;

            GetComponent<SpriteRenderer>().color = Color.red;

            GetComponent<Rigidbody2D>().simulated = false;

            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;

            GetComponent<Rigidbody2D>().simulated = true;

            GetComponent<Collider2D>().enabled = true;

            isPlayerAlive = true;
        }
    }

    public void SetStartParameters()
    {
        this.isPlayerAlive = true;
        this.playerSpeed = playerSpeedNormal;
        this.playerJumpPower = playerJumpPowerNormal;
        this.playerHP = playerHPNormal;
        this.isPlayerAliveByAD = false;
    }

    private void Start()
    {
        SetStartParameters();

        if (isOurPlayer)
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    public void ReturnToLifeAfterAD()
    {
        this.playerSpeed = playerSpeedNormal;
        this.playerJumpPower = playerJumpPowerNormal;
        this.playerHP = playerHPNormal / 2;

        SetPlayerAliveByAD(true);

        CheckPlayerStatus();
    }
}
