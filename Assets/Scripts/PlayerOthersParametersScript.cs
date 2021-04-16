using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerOthersParametersScript : MonoBehaviour
{
    Player player;
    TextMeshPro playerNickNameTextMesh;
    TextMeshPro playerHPTextMesh;

    [SerializeField]
    Sprite[] playerClasses = new Sprite[3];

    [SerializeField]
    GameObject[] playerClassesBullets = new GameObject[3];

    [SerializeField]
    Color[] colors = new Color[6];

    void InitializationAllObjects()
    {
        player = GetComponent<Player>();

        playerNickNameTextMesh = transform.GetChild(0).GetComponent<TextMeshPro>();
        playerHPTextMesh = transform.GetChild(1).GetComponent<TextMeshPro>();
    }

    private void RotatePlayerText()
    {
        if (transform.localScale.x < 0 && playerNickNameTextMesh.gameObject.transform.localScale.x > 0)
        {
            playerNickNameTextMesh.gameObject.transform.localScale = new Vector3(-playerNickNameTextMesh.gameObject.transform.localScale.x, playerNickNameTextMesh.gameObject.transform.localScale.y, playerNickNameTextMesh.gameObject.transform.localScale.z);
            playerHPTextMesh.gameObject.transform.localScale = new Vector3(-playerHPTextMesh.gameObject.transform.localScale.x, playerHPTextMesh.gameObject.transform.localScale.y, playerHPTextMesh.gameObject.transform.localScale.z);
        }
        else if (transform.localScale.x > 0 && playerNickNameTextMesh.gameObject.transform.localScale.x < 0)
        {
            playerNickNameTextMesh.gameObject.transform.localScale = new Vector3(-playerNickNameTextMesh.gameObject.transform.localScale.x, playerNickNameTextMesh.gameObject.transform.localScale.y, playerNickNameTextMesh.gameObject.transform.localScale.z);
            playerHPTextMesh.gameObject.transform.localScale = new Vector3(-playerHPTextMesh.gameObject.transform.localScale.x, playerHPTextMesh.gameObject.transform.localScale.y, playerHPTextMesh.gameObject.transform.localScale.z);
        }
    }

    private void Awake()
    {
        InitializationAllObjects();
    }

    private void Update()
    {
        ReloadPlayerParameters();
    }

    void ReloadPlayerParameters()
    {
        SetNewPlayesNickName();
        SetNewPlayerClass();
        SetNewPlayerColor();
        SetNewPlayesHP();

        RotatePlayerText();
    }

    void SetNewPlayesNickName()
    {
        playerNickNameTextMesh.text = player.ReturnPlayerNickName();
    }

    void SetNewPlayesHP()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerHPTextMesh.text = player.ReturnPlayerHP().ToString() + "/" + player.ReturnPlayerHPNormal();
        }
        else
        {
            playerHPTextMesh.text = "";
        }
    }

    void SetNewPlayerClass()
    {
        switch (player.ReturnPlayerClass())
        {
            case "Hunter":
                {
                    GetComponent<SpriteRenderer>().sprite = playerClasses[0];
                    player.SetPlayerClassBullet(playerClassesBullets[0]);
                    break;
                }
            case "Captain":
                {
                    GetComponent<SpriteRenderer>().sprite = playerClasses[1];
                    player.SetPlayerClassBullet(playerClassesBullets[1]);
                    break;
                }
            case "BigGuns":
                {
                    GetComponent<SpriteRenderer>().sprite = playerClasses[2];
                    player.SetPlayerClassBullet(playerClassesBullets[2]);
                    break;
                }
        }
    }

    void SetNewPlayerColor()
    {
        switch (player.ReturnPlayerColor())
        {
            case "White":
                {
                    playerNickNameTextMesh.color = colors[0];
                    playerHPTextMesh.color = colors[0];
                    break;
                }
            case "Black":
                {
                    playerNickNameTextMesh.color = colors[1];
                    playerHPTextMesh.color = colors[1];
                    break;
                }
            case "Red":
                {
                    playerNickNameTextMesh.color = colors[2];
                    playerHPTextMesh.color = colors[2];
                    break;
                }
            case "Yellow":
                {
                    playerNickNameTextMesh.color = colors[3];
                    playerHPTextMesh.color = colors[3];
                    break;
                }
            case "Blue":
                {
                    playerNickNameTextMesh.color = colors[4];
                    playerHPTextMesh.color = colors[4];
                    break;
                }
            case "Green":
                {
                    playerNickNameTextMesh.color = colors[5];
                    playerHPTextMesh.color = colors[5];
                    break;
                }
        }
    }
}
