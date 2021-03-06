﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 1f;         
    public float m_EndDelay = 3f;           
    public CameraControl m_CameraControl;   
    public Text m_MessageText;              
    public GameObject m_TankPrefab;         
    public TankManager[] m_Tanks;           


    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;

    public AudioSource a_winRound;
    public AudioSource a_winGame;


    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            Application.Quit();
            Debug.Log("this button quits when in a build rather than editor");
        }
    }


    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
            if (m_Tanks[i].m_PlayerNumber == 1)
            {
                if (PlayerPrefs.GetInt("PlayerOneJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    m_Tanks[i].DisableControl();
                    m_Tanks[i].m_Instance.SetActive(false);

                }
            }
            if (m_Tanks[i].m_PlayerNumber == 2)
            {
                if (PlayerPrefs.GetInt("PlayerTwoJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    m_Tanks[i].DisableControl();
                    m_Tanks[i].m_Instance.SetActive(false);
                }
            }
            if (m_Tanks[i].m_PlayerNumber == 3)
            {
                if (PlayerPrefs.GetInt("PlayerThreeJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    m_Tanks[i].DisableControl();
                    m_Tanks[i].m_Instance.SetActive(false);
                }
            }
            if (m_Tanks[i].m_PlayerNumber == 4)
            {
                if (PlayerPrefs.GetInt("PlayerFourJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    m_Tanks[i].DisableControl();
                    m_Tanks[i].m_Instance.SetActive(false);
                }
            }
        }
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_MessageText.text = string.Empty;

        while(!OneTankLeft())
        {
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();
        m_RoundWinner = null;
        m_RoundWinner = GetRoundWinner();

        if(m_RoundWinner != null)
        {
            m_RoundWinner.m_Wins++;
        }

        m_GameWinner = GetGameWinner();
        string message = EndMessage();
        m_MessageText.text = message;
        if(m_GameWinner != null)
        {
            a_winGame.Play();
        }
        else
        {
            a_winRound.Play();
        }

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        a_winRound.Play();
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_PlayerNumber == 1)
            {
                if (PlayerPrefs.GetInt("PlayerOneJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    break;

                }
            }
            if (m_Tanks[i].m_PlayerNumber == 2)
            {
                if (PlayerPrefs.GetInt("PlayerTwoJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    break;
                }
            }
            if (m_Tanks[i].m_PlayerNumber == 3)
            {
                if (PlayerPrefs.GetInt("PlayerThreeJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    break;
                }
            }
            if (m_Tanks[i].m_PlayerNumber == 4)
            {
                if (PlayerPrefs.GetInt("PlayerFourJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    break;
                }
            }
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_PlayerNumber == 1)
            {
                if (PlayerPrefs.GetInt("PlayerOneJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    m_Tanks[i].DisableControl();
                    break;
                }
            }
            if (m_Tanks[i].m_PlayerNumber == 2)
            {
                if (PlayerPrefs.GetInt("PlayerTwoJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    m_Tanks[i].DisableControl();
                    break;
                }
            }
            if (m_Tanks[i].m_PlayerNumber == 3)
            {
                if (PlayerPrefs.GetInt("PlayerThreeJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    m_Tanks[i].DisableControl();
                    break;
                }
            }
            if (m_Tanks[i].m_PlayerNumber == 4)
            {
                if (PlayerPrefs.GetInt("PlayerFourJoined") == 0)
                {
                    Debug.Log("excluding " + m_Tanks[i].m_PlayerNumber);
                    m_Tanks[i].DisableControl();
                    break;
                }
            }
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }
}