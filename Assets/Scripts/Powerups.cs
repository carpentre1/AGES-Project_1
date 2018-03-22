using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour {

    public enum Type : int {Jump=1, Damage=2, Other=3};//not sure what all the boosts will be
    public Type powerupType = Type.Jump;

    public int jumpStrength = 50;
    public int powerupRespawnTime = 2;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(powerupType == Type.Jump)
        {
            if(other.GetComponent<TankMovement>())
            {
                other.GetComponent<TankMovement>().Jump(jumpStrength);
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
        Debug.Log("2");
        gameObject.transform.Translate(Vector3.up * 20);
        Debug.Log("3");
    }

    private void OnDisable()
    {
        LateCall();
    }
}
