using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour {

    enum Type : int {Jump=1, Damage=2, Other=3};//not sure what all the boosts will be
    int powerupType;

    public int jumpStrength = 50;

	// Use this for initialization
	void Start () {
		if(this.name.Contains("Jump"))
        {
            powerupType = (int)Type.Jump;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(powerupType == (int)Type.Jump)
        {
            other.GetComponent<TankMovement>().Jump(jumpStrength);
            Debug.Log("jumped");
        }
    }
}
