using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour {

    public enum Type : int {Jump=1, Health=2, Invuln=3};//sudden big jump, full health restore, temporary invulnerability
    public Type powerupType = Type.Jump;

    public int jumpStrength = 50;
    public int powerupRespawnTime = 10;
    public float invulnDuration = 6f;

    public AudioSource a_powerupJump;
    public AudioSource a_powerupHealth;
    public AudioSource a_powerupInvuln;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<TankMovement>())//if it's a tank
        {
            if (powerupType == Type.Jump)
            {
                other.GetComponent<TankMovement>().Jump(jumpStrength);
                a_powerupJump.Play();
                ResetPowerup();
            }
            if (powerupType == Type.Health)
            {
                other.GetComponent<TankHealth>().TakeDamage(-100);
                a_powerupHealth.Play();
                ResetPowerup();
            }
            if (powerupType == Type.Invuln)
            {
                other.GetComponent<TankHealth>().invulnerable = true;
                StartCoroutine(PowerupDurationCoroutine(other, invulnDuration));
                StartCoroutine(Blink(other, invulnDuration));
                a_powerupInvuln.Play();
                ResetPowerup();
            }
        }

    }

    private void ResetPowerup()
    {
        StartCoroutine(LateCall());
        Debug.Log("0");
        gameObject.transform.Translate(Vector3.down * 20);
    }
    IEnumerator LateCall()
    {
        Debug.Log(powerupRespawnTime);

        yield return new WaitForSeconds(powerupRespawnTime);
        gameObject.transform.Translate(Vector3.up * 20);
    }
    IEnumerator PowerupDurationCoroutine(Collider other, float duration)
    {
        yield return new WaitForSeconds(duration);
        other.GetComponent<TankHealth>().invulnerable = false;
    }
    IEnumerator Blink(Collider other, float duration)
    {
        float timeWaited = 0;
        float blinkInterval = .15f;
        float endTime = Time.time + duration;
        var lerpedColor = Color.white;
        GameObject tankRenderer = other.transform.GetChild(0).gameObject;
        Component[] tankPieces = tankRenderer.GetComponentsInChildren<Renderer>();
        while(timeWaited < duration)
        {
            var originalColor = Color.red;
            lerpedColor = Color.Lerp(Color.white, Color.black, Time.deltaTime);
            foreach(Renderer renderer in tankPieces)
            {
                originalColor = renderer.material.color;
                renderer.material.color = lerpedColor;
            }
            yield return new WaitForSeconds(blinkInterval);
            //lerpedColor = Color.Lerp(Color.black, Color.white, Time.time);
            foreach (Renderer renderer in tankPieces)
            {
                renderer.material.color = originalColor;
            }
            yield return new WaitForSeconds(blinkInterval);
            timeWaited += blinkInterval * 2;

        }
    }

    private void OnDisable()
    {
        LateCall();
    }
}
