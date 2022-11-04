using System;
using UnityEngine;
using System.Collections;
//------------------------------------------
public class LineSight : MonoBehaviour
{
	//------------------------------------------
	//Sensibilidad de la vision
	public enum SightSensitivity {STRICT, LOOSE};
	//Asignacion de la sensibilidad
	public SightSensitivity Sensitity = SightSensitivity.STRICT;
	//CVariable booleana que permite determinar si se puede ver el player
	public bool CanSeeTarget = false;
	//Reference al Traget/Player/Personaje principal
	public Transform Target = null;
	//Reference a los ojos 
	public Transform EyePoint = null;
	//Referencia a transform component
	private Transform ThisTransform = null;
	//Reference to last know object sighting, if any
	public Vector3 LastKnowSighting = Vector3.zero;
	
	public float distance; //Distancia de vision
	public float angle; //Cono de vision
	//******************************************************
	public LayerMask targetLayers; //Objetos a detectar
	public LayerMask obstacleLayer; //Obstaculos a detectar
	public Transform cannon;
	public float activateSensorRate = 0.0f;
	public Collider[] colliders;
	//------------------------------------------
	void Awake()
	{
		ThisTransform = GetComponent<Transform>();
		Target = GameObject.FindWithTag("Player").transform;
		LastKnowSighting = ThisTransform.position;
	}
	//------------------------------------------
	/// <summary>
	/// Funcion: Validar el campo de vision del personaje
	/// </summary>
	/// <returns></returns>
	bool InFOV()
	{
		Vector3 direccionATarget =Vector3.Normalize(Target.position - EyePoint.position);
		float miangulo = Vector3.Angle(transform.forward, direccionATarget);
		//Verificar el angulo de vision
		if (miangulo <= angle)
		{
			MirarA();
			return true;
		}
		//Not within view
		return false;
	}
	//------------------------------------------
	bool ClearLineofSight()
	{
		if (!Physics.Linecast(transform.position,Target.GetComponent<CharacterController>().bounds.center,out RaycastHit hit, obstacleLayer))  {
			
			Debug.DrawLine(EyePoint.position, Target.GetComponent<CharacterController>().bounds.center, Color.yellow);
			return true;
		}
		else
		{
			Debug.DrawLine(EyePoint.position, Target.GetComponent<CharacterController>().bounds.center, Color.white);
		}

		return false;
	}
	//------------------------------------------
	void UpdateSight()
	{
		switch(Sensitity)
		{
			case SightSensitivity.STRICT:
				CanSeeTarget = InFOV() && ClearLineofSight();
				break;
			case SightSensitivity.LOOSE:
				CanSeeTarget = InFOV() || ClearLineofSight();
				break;
		}
	}
	void MirarA()
	{
		var direccionMirar =Vector3.Normalize(Target.position - EyePoint.position);
		direccionMirar.y = 0;
		transform.parent.forward = direccionMirar;
	}
	
	private void Update()
	{
		colliders=null;
		colliders=Physics.OverlapSphere(transform.position,distance,targetLayers);
		//Target = null;
		
		if (colliders.Length <= 0)
		{
			CanSeeTarget = false;
			return;
		}
		foreach(Collider myCollider in colliders)
		{

				//Target = myCollider.gameObject.transform;
				Debug.DrawLine(EyePoint.position, myCollider.bounds.center, Color.green);
				UpdateSight();
				MirarA();
				if(CanSeeTarget)
					LastKnowSighting =  Target.position;

		}
		
		/*if(CanSeeTarget)
			RotateTowards();*/

	}
	void RotateTowards()
	{
		//Get look to rotation
		Quaternion DestRot = Quaternion.LookRotation(Target.GetComponent<CharacterController>().bounds.center-cannon.position,Vector3.up);
		//Update rotation
		cannon.rotation = Quaternion.RotateTowards(cannon.rotation, DestRot, 90 * Time.deltaTime);
	}
	//---------------------------------------------------
	void RotateTowardsWithDamp()
	{
		//Get look to rotation
		Quaternion DestRot = Quaternion.LookRotation(Target.GetComponent<CharacterController>().bounds.center-transform.position,Vector3.up);

		//Calc smooth rotate
		Quaternion smoothRot = Quaternion.Slerp(transform.rotation, DestRot, -1f*(Time.deltaTime*45));

		//Update Rotation
		transform.rotation = smoothRot;
	}
	//------------------------------------------
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, distance);
        
		Gizmos.color = Color.green;
		Vector3 DirDerecha = Quaternion.Euler(0,angle,0)*transform.forward;
		Vector3 DirIzquierda = Quaternion.Euler(0, -angle, 0) * transform.forward;

		Gizmos.DrawRay(transform.position, DirDerecha*distance);
		Gizmos.DrawRay(transform.position, DirIzquierda * distance);
	}
}
//------------------------------------------