using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GunScript : MonoBehaviour
{
    public float _Damage = 10f;
    public float _Range = 100f;
    public GameObject _ImpactEffect;

    public Camera fpsCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, _Range))
        {
            Debug.Log(hit.transform.name);

            BossController target = hit.transform.GetComponent<BossController>();
            if (target != null)
            {
                target.TakeDamage(_Damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * 140f);
            }
            
            GameObject impactGO = Instantiate(_ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
        


    }
}
