using UnityEngine;
using System.Collections;

public class AmmoShooter : MonoBehaviour 
{
	private bool CanFire = true;
	public GameObject shootingPoint;
	public float ReloadDelay = 0.1f;

	public float speedBullet = 10;
	// Update is called once per frame
	void Update () 
	{
		//Check fire control
		if(Input.GetButtonDown("Fire1") && CanFire)
		{

			AmmoManager.SpawnAmmo(shootingPoint,speedBullet);

			CanFire = false;
			Invoke ("EnableFire", ReloadDelay);
		}
	}

	void EnableFire()
	{
		CanFire = true;
	}
}
