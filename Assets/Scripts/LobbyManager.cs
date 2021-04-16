using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    Player player;

    public InputField playerNickNameInputField;
    public Button startGameButton;
    public GameObject colorImage;
    public GameObject classImage;
    public Text errorText;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        player.SetOurPlayerTrue(true);
    }

    public void ReturnErrorTextLobby(string text)
    {
        StartCoroutine(ReturnErrorText(text));
    }

    IEnumerator ReturnErrorText(string text)
    {
        errorText.text = text;
        yield return new WaitForSeconds(2.5f);
        errorText.text = "";
    }

    void SetPlayerClass()
    {
        player.SetPlayerClass(ReturnChousenClass());
    }

    string ReturnChousenClass()
    {
        Toggle[] toggles = classImage.transform.GetComponentsInChildren<Toggle>();

        foreach (Toggle chousenToggle in toggles)
        {
            if (chousenToggle.isOn)
            {
                return chousenToggle.name.Replace("Toggle", "");
            }
        }

        return string.Empty;
    }

    void SetPlayerNickName()
    {
        player.SetPlayerNickName(ReturnInputNickName());
    }

    string ReturnInputNickName()
    {
        if (playerNickNameInputField.text.Length > 3)
        {
            return playerNickNameInputField.text;
        }

        return string.Empty;

    }

    void SetPlayerColor()
    {
        player.SetPlayerColor(ReturnChousenColor());
    }

    string ReturnChousenColor()
    {
        Toggle[] toggles = colorImage.transform.GetComponentsInChildren<Toggle>();

        foreach (Toggle chousenToggle in toggles)
        {
            if (chousenToggle.isOn)
            {
                return chousenToggle.name.Replace("Toggle", "").Replace("Color", "");
            }
        }

        return string.Empty;
    }

    private void Update()
    {
        SetPlayerNickName();
        SetPlayerColor();
        SetPlayerClass();
    }

    public void StartGame()
    {
        if (player.ReturnPlayerNickName() != "")
        {
            if (player.ReturnPlayerClass() != "")
            {
                if (player.ReturnPlayerColor() != "")
                {
                    SceneManager.LoadScene(1);
                }
                else
                {
                    ReturnErrorTextLobby("No chousen Color");
                }
            }
            else
            {
                ReturnErrorTextLobby("No chousen Class");
            }
        }
        else
        {
            ReturnErrorTextLobby("Short nickname");
        }
    }
}
