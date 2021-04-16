using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    int startRoundTime = 10;
    public float roundTime;

    [SerializeField]
    int numberRound = 0;
    [SerializeField]
    int progressNumber = 0;

    [SerializeField]
    GameObject[] players;

    [SerializeField]
    List<string> teams;

    [SerializeField]
    List<GameObject> roundTeam;
    [SerializeField]
    int numberOfRoundTeam = 0;
    [SerializeField]
    int numberOfPlayerFromTeam = 0;
    [SerializeField]
    GameObject playerRound;

    [SerializeField]
    Text roundTimeText;
    [SerializeField]
    Text colorTeamRoundText;
    [SerializeField]
    Text numberOfRoundText;

    [SerializeField]
    bool isCountdownWork = false;
    [SerializeField]
    bool isMenuOpen = false;
    [SerializeField]
    bool isGameFinish = false;

    private void Awake()
    {
        InitializationAllObjects();

        FindAllTeams();

        LoadTeamForRound(teams[numberOfRoundTeam]);

        StartCoroutine(Countdown(5));
    }

    void FindAllTeams()
    {
        foreach (GameObject playerObject in players)
        {
            Player player = playerObject.GetComponent<Player>();

            if (!teams.Contains(player.ReturnPlayerColor()))
            {
                teams.Add(player.ReturnPlayerColor());
            }
        }
    }

    void InitializationAllObjects()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void LoadTeamForRound(string roundOfColor)
    {
        roundTeam.Clear();

        foreach (GameObject playerObject in players)
        {
            Player player = playerObject.GetComponent<Player>();

            if (roundOfColor == player.ReturnPlayerColor())
            {
                roundTeam.Add(playerObject);
            }
        }
    }

    void NextRound()
    {
        if (progressNumber >= players.Length)
        {
            Debug.Log("New Round");
            numberRound++;
            progressNumber = 0;
            //numberOfRoundTeam = 0;
            //numberOfPlayerFromTeam = 0;
        }
    }

    void EndRoundOfTeam()
    {
        if (numberOfPlayerFromTeam + 1 >= roundTeam.Count())
        {
            numberOfPlayerFromTeam = 0;
            numberOfRoundTeam++;

            if (numberOfRoundTeam >= teams.Count)
            {
                numberOfRoundTeam = 0;
            }

            LoadTeamForRound(teams[numberOfRoundTeam]);
        }
        else
        {
            numberOfPlayerFromTeam++;
        }
    }

    void NextProgress()
    {
        progressNumber++;

        DisableControlOfPlayer(playerRound);

        playerRound = ReturnNextPlayer();
    }

    void EnableControlOfPlayer(GameObject player)
    {
        player.GetComponent<PlayerMovementScript>().SetPlayerControl(true);
    }

    void DisableControlOfPlayer(GameObject player)
    {
        player.GetComponent<PlayerMovementScript>().SetPlayerControl(false);
    }

    void DisableAllCintrolsOfPlayer()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerMovementScript>().SetPlayerControl(false);
        }
    }

    GameObject ReturnNextPlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (roundTeam[numberOfPlayerFromTeam].GetComponent<Player>().ReturnPlayerStatus() == true)
            {
                //Debug.Log(roundTeam[numberOfPlayerFromTeam]);
                return roundTeam[numberOfPlayerFromTeam];
            }
            else
            {
                //Debug.Log("NextPlayer");
                EndRoundOfTeam();
            }

        }

        return null;
    }

    private void Update()
    {
        if (isGameFinish == false)
        {
            if (isMenuOpen == false)
            {
                if (isCountdownWork == false)
                {
                    TimerOfProgress();

                    ReloadUI();
                }
            }
        }
    }

    void ReloadUI()
    {
        roundTimeText.text = PreparingTimeIndicator(roundTime);

        if (isGameFinish == false)
        {
            if (numberOfRoundTeam < teams.Count)
            {
                colorTeamRoundText.text = teams[numberOfRoundTeam].ToString() + " Team Round";
            }
        }

        numberOfRoundText.text = "Round: " + (numberRound + 1).ToString();
    }

    private string PreparingTimeIndicator(float time)
    {
        string OurTime;
        int minutes;
        int seconds;

        minutes = (int)time / 60;
        seconds = (int)time - minutes * 60;

        OurTime = minutes.ToString("00") + ":" + seconds.ToString("00");

        return OurTime;
    }

    public void PlayerMadeMove()
    {
        roundTime = 3;
    }

    void TimerOfProgress()
    {
        roundTime -= Time.deltaTime;

        DisableDeadPlayers();
        OurPlayerDead();
        CheckLastTeam();

        if (roundTime <= 0)
        {
            EndRoundOfTeam();
            NextProgress();
            NextRound();

            ChangePlayerForCamera();

            StartCoroutine(Countdown(5));
        }
    }

    void ChangePlayerForCamera()
    {
        GameObject.Find("CM vcam").GetComponent<CinemachineVirtualCamera>().Follow = ReturnNextPlayer().transform;
        GameObject.Find("CM vcam").GetComponent<CinemachineVirtualCamera>().LookAt = ReturnNextPlayer().transform;
    }

    IEnumerator Countdown(int seconds)
    {
        isCountdownWork = true;

        colorTeamRoundText.text = "Preparing";
        numberOfRoundText.text = "Round: " + numberRound.ToString();

        int counter = seconds;
        roundTimeText.text = PreparingTimeIndicator(counter);
        while (counter > 0)
        {
            yield return new WaitUntil(() => isMenuOpen == false);
            yield return new WaitForSeconds(1);
            counter--;
            colorTeamRoundText.text = "Preparing";
            roundTimeText.text = PreparingTimeIndicator(counter);
        }

        isCountdownWork = false;
        StartRound();
    }

    void StartRound()
    {
        //if (progressNumber == 0) 
        {
            roundTime = startRoundTime;

            playerRound = ReturnNextPlayer();
            EnableControlOfPlayer(playerRound);

            ChangePlayerForCamera();
        }
    }

    [SerializeField]
    List<string> checkTeam;

    void CheckLastTeam()
    {
        checkTeam = new List<string>();

        //Debug.Log("lastPlayer");

        foreach (GameObject playerObject in players)
        {
            Player player = playerObject.GetComponent<Player>();

            //Debug.Log(!checkTeam.Contains(player.ReturnPlayerColor()));

            if (!checkTeam.Contains(player.ReturnPlayerColor()))
            {
                if (player.ReturnPlayerStatus())
                {
                    //Debug.Log(player.ReturnPlayerColor());
                    checkTeam.Add(player.ReturnPlayerColor());
                }
            }
        }

        if (isGameFinish == false)
        {
            if (isMenuOpen == false)
            {
                if (checkTeam.Count == 1)
                {
                    isGameFinish = true;
                    //colorTeamRoundText.text = checkTeam[0].ToString() + " is winners";
                    FinishGameProcess(checkTeam[0].ToString() + " is winners");
                }
                else if (checkTeam.Count == 0)
                {
                    isGameFinish = true;
                    FinishGameProcess("Draw");
                }
                else
                {
                    isGameFinish = false;
                }
            }
        }
    }

    void FinishGameProcess(string winners)
    {
        StartRound(); 

        Debug.Log(winners);
        colorTeamRoundText.text = winners;

        if (isMenuOpen == false)
        {
            Invoke("OpenOrCloseMenu", 5f);
        }
    }

    [SerializeField]
    GameObject menuImage;

    public void OpenOrCloseMenu()
    {
        if (menuImage.activeSelf == false)
        {
            isMenuOpen = true;
            menuImage.SetActive(true);

            if (playerRound != null)
            {
                DisableControlOfPlayer(playerRound);
            }

        }
        else
        {
            if (isGameFinish == false)
            {
                isMenuOpen = false;
                menuImage.SetActive(false);

                if (playerRound != null)
                {
                    EnableControlOfPlayer(playerRound);
                }
            }
        }
    }

    void OurPlayerDead()
    {
        foreach (GameObject player in players)
        {
            Player ourPlayer = player.GetComponent<Player>();

            if (ourPlayer.ReturnOurPlayerTrue())
            {
                if (ourPlayer.ReturnPlayerStatus() == false)
                {
                    //DisableControlOfPlayer(playerRound);

                    if (!ourPlayer.ReturnPlayerAliveByAD())
                    {
                        menuImage.transform.GetChild(0).gameObject.SetActive(true);
                        OpenOrCloseMenu();
                    }
                    else
                    {
                        OpenOrCloseMenu();
                        isGameFinish = true;
                    }
                }
            }
        }
    }

    void DisableDeadPlayers()
    {
        foreach (GameObject player in players)
        {
            Player ourPlayer = player.GetComponent<Player>();

            if (ourPlayer.ReturnPlayerStatus() == false)
            { 
                if (ourPlayer.GetComponent<PlayerMovementScript>().ReturnPlayerControl())
                {
                    PlayerMadeMove();
                    DisableControlOfPlayer(playerRound);
                }

                player.SetActive(false);
            }
        }
    }

    public void ReturnToMenu()
    {
        foreach (GameObject player in players)
        {
            Player ourPlayer = player.GetComponent<Player>();

            Destroy(ourPlayer.gameObject);
        }

        SceneManager.LoadScene(0);
    }

    public void ReturnOurPersonToLife()
    {
        foreach (GameObject player in players)
        {
            Player ourPlayer = player.GetComponent<Player>();

            if (ourPlayer.ReturnOurPlayerTrue())
            {
                if (ourPlayer.ReturnPlayerStatus() == false)
                {
                    ourPlayer.ReturnToLifeAfterAD();

                    ourPlayer.gameObject.SetActive(true);

                    menuImage.transform.GetChild(0).gameObject.SetActive(false);
                    OpenOrCloseMenu();

                    DisableControlOfPlayer(playerRound);
                }
            }
        }
    }

    public void StartPlayingAd()
    {
        int timeToShake = 0;

        Invoke("ButtonShake", timeToShake);
        Invoke("DesplayAd", timeToShake + 1);
       
    }

    private void ButtonShake()
    {
        EventSystem.current.currentSelectedGameObject.transform.DOShakePosition(3, 3);
    }

    private void DesplayAd()
    {
        GetComponent<AdManagerScript>().DisplayInterstitialAD();
    }
}
