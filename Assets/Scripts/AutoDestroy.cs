using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Tiempo de espera para destruir la Bala")]
    public float delayDestruction;
    public float projectileSpeed;
    // Start is called before the first frame update
    void OnEnable()
    {
        //Destroy(gameObject,delayDestruction);
        Invoke("HideObjectPool",delayDestruction);
    }

    private void Start()
    {
        //Invoke("HideObjectPool",delayDestruction);
    }
    private void Update()
    {
        //gameObject.transform.Translate(0, 0, projectileSpeed * Time.deltaTime);
        //gameObject.GetComponent<Rigidbody>().velocity = transform.forward.normalized * projectileSpeed;
    }
    public void HideObjectPool()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
