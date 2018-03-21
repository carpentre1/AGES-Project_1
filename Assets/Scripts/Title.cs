using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour {

    //button art: HeyHumans
    //https://opengameart.org/content/xbox-360-pixel-art-controller-icons
    //CC-BY-SA 3.0

    int numPlayers = 2;

    bool playerOneJoined = false;
    bool playerTwoJoined = false;
    bool playerThreeJoined = false;
    bool playerFourJoined = false;

    bool playerOneInputDisabled = false;//momentarily disables left/right movement when selecting weapons so you don't scroll too fast
    bool playerTwoInputDisabled = false;
    bool playerThreeInputDisabled = false;
    bool playerFourInputDisabled = false;

    enum WeaponType { Triple=1, Cannon=2, Artillery=3}//trying to figure out how to use enums effectively. i think this helps?

    WeaponType playerOneWeapon = WeaponType.Cannon;
    WeaponType playerTwoWeapon = WeaponType.Cannon;
    WeaponType playerThreeWeapon = WeaponType.Cannon;
    WeaponType playerFourWeapon = WeaponType.Cannon;

    string currentScreen = "Main Menu";


    #region game objects
    public GameObject mainMenu;
    public GameObject playerPanels;
    public GameObject creditsPanel;
    public GameObject audioPanel;

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
        }
    }

    public void StartGame()//when on join screen and someone presses start on their controller, switch scenes to the game
    {
        //import which players are active and what weapons they're using to tankmanager?
        //load scene
    }

    void Update() {//listen for inputs from each controller, let players join if they press A, let them choose their weapon with left/right, start game if they push start button
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("player 1");
            if (!playerOneJoined && currentScreen == "Join")
            {
                playerOneJoined = true;
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
        }
        if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("player 3");
        }
        if (Input.GetButtonDown("Fire4"))
        {
            Debug.Log("player 4");
        }
        if (Input.GetButtonDown("Submit") && numPlayers >= 2)
        {
            Debug.Log("start");
        }

        #region move left or right to choose weapon
        //there's clearly some better way to do this but i can't figure it out
        //player 1
        if (Input.GetAxis("Horizontal1") >= .8 && currentScreen == "Join" && !playerOneInputDisabled)
        {
            playerOneWeapon = (WeaponType)Mathf.Min((float)playerOneWeapon + 1, 3);
            //UI move noise
            weaponText1.GetComponent<Text>().text = WeaponText(false, playerOneWeapon);
            weaponInfoText1.GetComponent<Text>().text = WeaponText(true, playerOneWeapon);
            playerOneInputDisabled = true;
            StartCoroutine(AxisInputDelay(1));
        }
        if (Input.GetAxis("Horizontal1") <= -.8 && currentScreen == "Join" && !playerOneInputDisabled)
        {
            playerOneWeapon = (WeaponType)Mathf.Max((float)playerOneWeapon - 1, 1);
            //UI move noise
            weaponText1.GetComponent<Text>().text = WeaponText(false, playerOneWeapon);
            weaponInfoText1.GetComponent<Text>().text = WeaponText(true, playerOneWeapon);
            playerOneInputDisabled = true;
            StartCoroutine(AxisInputDelay(1));
        }
        //player 2
        if (Input.GetAxis("Horizontal2") >= .8 && currentScreen == "Join" && !playerTwoInputDisabled)
        {
            playerTwoWeapon = (WeaponType)Mathf.Min((float)playerTwoWeapon + 1, 3);
            //UI move noise
            weaponText2.GetComponent<Text>().text = WeaponText(false, playerTwoWeapon);
            weaponInfoText2.GetComponent<Text>().text = WeaponText(true, playerTwoWeapon);
        }
        if (Input.GetAxis("Horizontal2") <= -.8 && currentScreen == "Join" && !playerTwoInputDisabled)
        {
            playerTwoWeapon = (WeaponType)Mathf.Max((float)playerTwoWeapon - 1, 1);
            //UI move noise
            weaponText2.GetComponent<Text>().text = WeaponText(false, playerTwoWeapon);
            weaponInfoText2.GetComponent<Text>().text = WeaponText(true, playerTwoWeapon);
        }
        //player 3
        if (Input.GetAxis("Horizontal3") >= .8 && currentScreen == "Join" && !playerThreeInputDisabled)
        {
            playerThreeWeapon = (WeaponType)Mathf.Min((float)playerThreeWeapon + 1, 3);
            //UI move noise
            weaponText3.GetComponent<Text>().text = WeaponText(false, playerThreeWeapon);
            weaponInfoText3.GetComponent<Text>().text = WeaponText(true, playerThreeWeapon);
        }
        if (Input.GetAxis("Horizontal3") <= -.8 && currentScreen == "Join" && !playerThreeInputDisabled)
        {
            playerThreeWeapon = (WeaponType)Mathf.Max((float)playerThreeWeapon - 1, 1);
            //UI move noise
            weaponText3.GetComponent<Text>().text = WeaponText(false, playerThreeWeapon);
            weaponInfoText3.GetComponent<Text>().text = WeaponText(true, playerThreeWeapon);
        }
        //player 4
        if (Input.GetAxis("Horizontal4") >= .8 && currentScreen == "Join" && !playerFourInputDisabled)
        {
            playerFourWeapon = (WeaponType)Mathf.Min((float)playerFourWeapon + 1, 3);
            //UI move noise
            weaponText4.GetComponent<Text>().text = WeaponText(false, playerFourWeapon);
        }
        if (Input.GetAxis("Horizontal4") <= -.8 && currentScreen == "Join" && !playerFourInputDisabled)
        {
            playerFourWeapon = (WeaponType)Mathf.Max((float)playerFourWeapon - 1, 1);
            //UI move noise
            weaponText4.GetComponent<Text>().text = WeaponText(false, playerFourWeapon);
            weaponInfoText4.GetComponent<Text>().text = WeaponText(true, playerFourWeapon);
        }
    }

    IEnumerator AxisInputDelay(int player)
    {
        yield return new WaitForSeconds(.5f);
        switch (player)
        {
            case 1: playerOneInputDisabled = false; break;
            case 2: playerTwoInputDisabled = false; break;
            case 3: playerThreeInputDisabled = false; break;
            case 4: playerFourInputDisabled = false; break;
        }
    }
    #endregion

    string WeaponText(bool isDescription, WeaponType weapon)//returns the name or description of the weapon.
    {
        if (isDescription)//if it's the description under the weapon
            if (weapon == WeaponType.Triple)
            {
                return "Fires a spread of 3 shots.\nShort range, low damage.";
            }
            else if (weapon == WeaponType.Cannon)
            {
                return "Rapidly fires single shots.\nMedium range and damage.";
            }
            else return "Slowly fires shots with enormous explosions.\nLong range, high damage.";
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
