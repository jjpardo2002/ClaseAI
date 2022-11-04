using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//------------------------------
public class AmmoManager : MonoBehaviour
{
	public float lifeTime = 3f;
	//------------------------------
	//Reference to ammo prefab
	public GameObject AmmoPrefab = null;

	//Ammo pool count
	public int PoolSize = 100;

	public Queue<Transform> AmmoQueue = new Queue<Transform>();
	//public Transform projectileSpawn;
	//Array of ammo objects to generate
	private GameObject[] AmmoArray;

	public static AmmoManager AmmoManagerSingleton = null;
	//------------------------------
	// Use this for initialization
	void Awake ()
	{
		if(AmmoManagerSingleton != null)
		{
			Destroy(GetComponent<AmmoManager>());
			return;
		}

		AmmoManagerSingleton = this;
		AmmoArray = new GameObject[PoolSize];

		for(int i=0; i<PoolSize; i++)
		{
			AmmoArray[i] = Instantiate(AmmoPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			Transform ObjTransform = AmmoArray[i].GetComponent<Transform>();
			ObjTransform.parent = GetComponent<Transform>();
			AmmoQueue.Enqueue(ObjTransform);
			AmmoArray[i].SetActive(false);
		}
	}
	//------------------------------
	public static Transform SpawnAmmo(GameObject projectileSpawn,float projectileSpeed)
	{
		
		//Get ammo
		Transform SpawnedAmmo = AmmoManagerSingleton.AmmoQueue.Dequeue();

		SpawnedAmmo.gameObject.SetActive(true);
		SpawnedAmmo.position = projectileSpawn.transform.position;
		SpawnedAmmo.rotation = projectileSpawn.transform.rotation;
		//SpawnedAmmo.GetComponent<AutoDestroy>().projectileSpeed = 50;
		//SpawnedAmmo.gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, projectileSpeed, 0));
		SpawnedAmmo.gameObject.GetComponent<Rigidbody>().velocity=SpawnedAmmo.transform.forward.normalized * projectileSpeed;
		//SpawnedAmmo.gameObject.transform.Translate(0,0,projectileSpeed*Time.deltaTime);
		//Add to queue end
		AmmoManagerSingleton.AmmoQueue.Enqueue(SpawnedAmmo);

		//Return ammo
		return SpawnedAmmo;
	}
	//------------------------------
}
//------------------------------