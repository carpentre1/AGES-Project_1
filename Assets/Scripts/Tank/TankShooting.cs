using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 12f; 
    public float m_MaxLaunchForce = 25f; 
    public float m_MaxChargeTime = 0.5f;

    
    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;

    public int weaponType = 2;//1 is triple shot, 2 is cannon, 3 is artillery

    Transform leftTransform;
    Transform rightTransform;

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

        if(m_PlayerNumber == 1)
        {
            weaponType = PlayerPrefs.GetInt("PlayerOneWeapon");
        }
        if (m_PlayerNumber == 2)
        {
            weaponType = PlayerPrefs.GetInt("PlayerTwoWeapon");
        }
        if (m_PlayerNumber == 3)
        {
            weaponType = PlayerPrefs.GetInt("PlayerThreeWeapon");
        }
        if (m_PlayerNumber == 4)
        {
            weaponType = PlayerPrefs.GetInt("PlayerFourWeapon");
        }
        if(weaponType == 1)
        {
            m_MinLaunchForce = 10f;
            m_MaxLaunchForce = 16f;
            m_MaxChargeTime = .4f;
        }
        if (weaponType == 2)
        {
            m_MinLaunchForce = 12f;
            m_MaxLaunchForce = 25f;
            m_MaxChargeTime = .5f;
        }
        if (weaponType == 3)
        {
            m_MinLaunchForce = 15f;
            m_MaxLaunchForce = 30f;
            m_MaxChargeTime = .6f;
        }
    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;

        if(m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            //at max charge, haven't fired
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if(Input.GetButtonDown(m_FireButton))
        {
            //have we pressed fire for the first time?
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.volume = .5f;
            m_ShootingAudio.Play();
        }
        else if(Input.GetButton(m_FireButton) && !m_Fired)
        {
            //holding the fire button, not yet fired
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            //released the fire button, haven't fired
            Fire();
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation, this.gameObject.transform) as Rigidbody;
        if (weaponType == 3)
        {
            Transform artilleryTransform = m_FireTransform;
            Debug.Log("before" + artilleryTransform.transform.rotation);
            artilleryTransform.transform.Rotate(-40, 0, 0);
            Debug.Log("after rotation" + artilleryTransform.transform.rotation);
            shellInstance.velocity = m_CurrentLaunchForce * artilleryTransform.forward * 1.5f;
            artilleryTransform.transform.Rotate(40, 0, 0);//it kept preserving the rotation no matter what i tried, so this is my fix for preventing each shot from being more heavily rotated than the last
            Debug.Log("end" + artilleryTransform.transform.rotation);
        }
        if(weaponType == 2)
        {
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward * 1.5f;
        }
        if (weaponType == 1)
        {
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward * 1.5f;

            Vector3 sidePosition = m_FireTransform.position + new Vector3(2f, 0, 0);
            Transform sideTransform = m_FireTransform;
            sideTransform.transform.Translate(-2, 0, 0);
            Rigidbody shellInstanceLeft = Instantiate(m_Shell, sideTransform.position, m_FireTransform.rotation, this.gameObject.transform) as Rigidbody;
            sideTransform.transform.Translate(2, 0, 0);

            sideTransform.transform.Rotate(0, -20, 0);//left
            shellInstanceLeft.velocity = m_CurrentLaunchForce * sideTransform.forward * 1.5f;
            sideTransform.transform.Rotate(0, 20, 0);

            sidePosition = m_FireTransform.position + new Vector3(-2f, 0, 0);
            sideTransform = m_FireTransform;
            sideTransform.transform.Translate(2, 0, 0);
            Rigidbody shellInstanceRight = Instantiate(m_Shell, sideTransform.position, m_FireTransform.rotation, this.gameObject.transform) as Rigidbody;
            sideTransform.transform.Translate(-2, 0, 0);

            sideTransform.transform.Rotate(0, 20, 0);//right
            shellInstanceRight.velocity = m_CurrentLaunchForce * sideTransform.forward * 1.5f;
            sideTransform.transform.Rotate(0, -20, 0);
        }
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}