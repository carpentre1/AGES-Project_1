using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

    //button art: HeyHumans
    //https://opengameart.org/content/xbox-360-pixel-art-controller-icons
    //CC-BY-SA 3.0

    int numPlayers = 0;
    public float inputDelay = .3f;//the amount of time to prevent input when navigating between weapons

    public bool playerOneJoined = false;
    public bool playerTwoJoined = false;
    public bool playerThreeJoined = false;
    public bool playerFourJoined = false;

    bool playerOneInputDisabled = false;//momentarily disables left/right movement when selecting weapons so you don't scroll too fast
    bool playerTwoInputDisabled = false;
    bool playerThreeInputDisabled = false;
    bool playerFourInputDisabled = false;

    public enum WeaponType { Triple=1, Cannon=2, Artillery=3}//trying to figure out how to use enums effectively. i think this helps?

    public WeaponType playerOneWeapon = WeaponType.Cannon;
    public WeaponType playerTwoWeapon = WeaponType.Cannon;
    public WeaponType playerThreeWeapon = WeaponType.Cannon;
    public WeaponType playerFourWeapon = WeaponType.Cannon;

    string currentScreen = "Main Menu";


    public AudioClip navigation;
    public AudioClip navigationSelect;

    public AudioClip playerJoined;
    public AudioClip readyToStart;
    public AudioClip starting;
    public AudioClip WeaponSelect;

    public AudioSource a_playerJoined;
    public AudioSource a_readyToStart;
    public AudioSource a_starting;
    public AudioSource a_WeaponSelect;

    #region game objects
    public GameObject mainMenu;
    public GameObject playerPanels;
    public GameObject creditsPanel;
    public GameObject audioPanel;
    public GameObject pressStartPanel; bool panelShown = false;

    public GameObject playerPanel1;
    public GameObject playerPanel2;
    public GameObject playerPanel3;
    public GameObject playerPanel4;

    public GameObject weaponText1;
    public GameObject weaponText2;
    public GameObject weaponText3;
    public GameObject weaponText4;

    public GameObject weaponInfoText1;
    public GameObject weaponInfoText2;
    public GameObject weaponInfoText3;
    public GameObject weaponInfoText4;
    #endregion

    void Start() {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            Debug.Log(names[x].Length);//prints connected controllers. 33 is xbox, 19 is PS
        }
        playerPanels.SetActive(false);

    }
    private void OnDestroy()// when the scene changes to the main game, save these variables so they can be used in the game
    {
        PlayerPrefs.SetInt("PlayerOneJoined", System.Convert.ToInt32(playerOneJoined));
        PlayerPrefs.SetInt("PlayerTwoJoined", System.Convert.ToInt32(playerTwoJoined));
        PlayerPrefs.SetInt("PlayerThreeJoined", System.Convert.ToInt32(playerThreeJoined));
        PlayerPrefs.SetInt("PlayerFourJoined", System.Convert.ToInt32(playerFourJoined));

        PlayerPrefs.SetInt("PlayerOneWeapon", System.Convert.ToInt32(playerOneWeapon));
        PlayerPrefs.SetInt("PlayerTwoWeapon", System.Convert.ToInt32(playerTwoWeapon));
        PlayerPrefs.SetInt("PlayerThreeWeapon", System.Convert.ToInt32(playerThreeWeapon));
        PlayerPrefs.SetInt("PlayerFourWeapon", System.Convert.ToInt32(playerFourWeapon));
    }

    public void StartJoinScreen()//when clicking "start" on main menu, show "press A to join" screen
    {
        currentScreen = "Join";
        mainMenu.SetActive(false);
        playerPanels.SetActive(true);
        foreach (Transform child in playerPanels.transform)
        {
            if (child.name == "JoinText" || child.name == "JoinImage")
            {
                child.gameObject.SetActive(true);
            }
            if (child.name == "PlayerWeaponPanel")
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    void ShowWeaponPanel(GameObject playerPanel)//hides "A to join" and shows weapon selection UI
    {
        foreach (Transform child in playerPanel.transform)
        {
            if (child.name == "JoinText" || child.name == "JoinImage")
            {
                child.gameObject.SetActive(false);
            }
            if (child.name == "PlayerWeaponPanel")
            {
                child.gameObject.SetActive(true);
            }
        }
    }


    void Update() {//listen for inputs from each controller, let players join if they press A, let them choose their weapon with left/right, start game if they push start button

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
            Debug.Log("this button quits when in a build rather than editor");
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("player 1");
            if (!playerOneJoined && currentScreen == "Join")
            {
                playerOneJoined = true;
                a_playerJoined.Play();
                numPlayers += 1;
                ShowWeaponPanel(playerPanel1);
            }
        }
        if(Input.GetButtonDown("Cancel") && currentScreen == "Main Menu")//back button is 1 on xbox, 2 on ps4. fire is 0 on xbox, 1 on ps4
        {
            creditsPanel.SetActive(false);
            audioPanel.SetActive(false);
            mainMenu.SetActive(true);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("player 2");
            if (!playerTwoJoined && currentScreen == "Join")
            {
                playerTwoJoined = true;
                a_playerJoined.Play();
                numPlayers += 1;
                ShowWeaponPanel(playerPanel2);
            }
        }
        if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("player 3");
            if (!playerThreeJoined && currentScreen == "Join")
            {
                playerThreeJoined = true;
                a_playerJoined.Play();
                numPlayers += 1;
                ShowWeaponPanel(playerPanel3);
            }
        }
        if (Input.GetButtonDown("Fire4"))
        {
            Debug.Log("player 4");
            if (!playerFourJoined && currentScreen == "Join")
            {
                playerFourJoined = true;
                a_playerJoined.Play();
                numPlayers += 1;
                ShowWeaponPanel(playerPanel4);
            }
        }
        if (numPlayers >= 2 && !panelShown)
        {
            pressStartPanel.SetActive(true);
            a_readyToStart.Play();
            panelShown = true;
        }
        if (Input.GetButtonDown("Start") && numPlayers >= 2)
        {
            Debug.Log("start " + numPlayers + playerOneJoined + playerTwoJoined + playerThreeJoined + playerFourJoined);
            a_starting.Play();
            pressStartPanel.SetActive(true);
            SceneManager.LoadScene(1);
        }


        #region move left or right to choose weapon
        //there's clearly some better way to do this but making this mess was quicker than figuring it all out
        //player 1
        if (Input.GetAxis("Horizontal1") >= .8 && currentScreen == "Join" && !playerOneInputDisabled)
        {
            playerOneWeapon = (WeaponType)Mathf.Min((float)playerOneWeapon + 1, 3);
            a_WeaponSelect.Play();
            weaponText1.GetComponent<Text>().text = WeaponText(false, playerOneWeapon);
            weaponInfoText1.GetComponent<Text>().text = WeaponText(true, playerOneWeapon);
            playerOneInputDisabled = true;
            StartCoroutine(AxisInputDelay(1));
        }
        if (Input.GetAxis("Horizontal1") <= -.8 && currentScreen == "Join" && !playerOneInputDisabled)
        {
            playerOneWeapon = (WeaponType)Mathf.Max((float)playerOneWeapon - 1, 1);
            a_WeaponSelect.Play();
            weaponText1.GetComponent<Text>().text = WeaponText(false, playerOneWeapon);
            weaponInfoText1.GetComponent<Text>().text = WeaponText(true, playerOneWeapon);
            playerOneInputDisabled = true;
            StartCoroutine(AxisInputDelay(1));
        }
        //player 2
        if (Input.GetAxis("Horizontal2") >= .8 && currentScreen == "Join" && !playerTwoInputDisabled)
        {
            playerTwoWeapon = (WeaponType)Mathf.Min((float)playerTwoWeapon + 1, 3);
            a_WeaponSelect.Play();
            weaponText2.GetComponent<Text>().text = WeaponText(false, playerTwoWeapon);
            weaponInfoText2.GetComponent<Text>().text = WeaponText(true, playerTwoWeapon);
        }
        if (Input.GetAxis("Horizontal2") <= -.8 && currentScreen == "Join" && !playerTwoInputDisabled)
        {
            playerTwoWeapon = (WeaponType)Mathf.Max((float)playerTwoWeapon - 1, 1);
            a_WeaponSelect.Play();
            weaponText2.GetComponent<Text>().text = WeaponText(false, playerTwoWeapon);
            weaponInfoText2.GetComponent<Text>().text = WeaponText(true, playerTwoWeapon);
        }
        //player 3
        if (Input.GetAxis("Horizontal3") >= .8 && currentScreen == "Join" && !playerThreeInputDisabled)
        {
            playerThreeWeapon = (WeaponType)Mathf.Min((float)playerThreeWeapon + 1, 3);
            a_WeaponSelect.Play();
            weaponText3.GetComponent<Text>().text = WeaponText(false, playerThreeWeapon);
            weaponInfoText3.GetComponent<Text>().text = WeaponText(true, playerThreeWeapon);
        }
        if (Input.GetAxis("Horizontal3") <= -.8 && currentScreen == "Join" && !playerThreeInputDisabled)
        {
            playerThreeWeapon = (WeaponType)Mathf.Max((float)playerThreeWeapon - 1, 1);
            a_WeaponSelect.Play();
            weaponText3.GetComponent<Text>().text = WeaponText(false, playerThreeWeapon);
            weaponInfoText3.GetComponent<Text>().text = WeaponText(true, playerThreeWeapon);
        }
        //player 4
        if (Input.GetAxis("Horizontal4") >= .8 && currentScreen == "Join" && !playerFourInputDisabled)
        {
            playerFourWeapon = (WeaponType)Mathf.Min((float)playerFourWeapon + 1, 3);
            a_WeaponSelect.Play();
            weaponText4.GetComponent<Text>().text = WeaponText(false, playerFourWeapon);
        }
        if (Input.GetAxis("Horizontal4") <= -.8 && currentScreen == "Join" && !playerFourInputDisabled)
        {
            playerFourWeapon = (WeaponType)Mathf.Max((float)playerFourWeapon - 1, 1);
            a_WeaponSelect.Play();
            weaponText4.GetComponent<Text>().text = WeaponText(false, playerFourWeapon);
            weaponInfoText4.GetComponent<Text>().text = WeaponText(true, playerFourWeapon);
        }
    }

    IEnumerator AxisInputDelay(int player)//restores their ability to navigate weapon selection after a short delay
    {
        yield return new WaitForSeconds(inputDelay);
        switch (player)
        {
            case 1: playerOneInputDisabled = false; break;
            case 2: playerTwoInputDisabled = false; break;
            case 3: playerThreeInputDisabled = false; break;
            case 4: playerFourInputDisabled = false; break;
        }
    }
    #endregion

    string WeaponText(bool isDescription, WeaponType weapon)//returns the name or description of the weapon
    {
        if (isDescription)//if it's the description under the weapon
            if (weapon == WeaponType.Triple)
            {
                return "Fires a spread of 3 shots.\nShort range, small explosions.";
            }
            else if (weapon == WeaponType.Cannon)
            {
                return "Medium range and explosion size.";
            }
            else return "Long range shots with enormous explosions.";
        else//if it's the name of the weapon itself
        {
            if (weapon == WeaponType.Triple)
            {
                return "Triple Shot";
            }
            else if (weapon == WeaponType.Cannon)
            {
                return "Cannon";
            }
            else return "Artillery";
        }
    }
}
