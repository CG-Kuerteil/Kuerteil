using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    public float _HP = 100f;
    public float _HP_Max = 100f;
    public Slider _Slider;
    private float _HP_Pickup_Amount;
    private float _Damage;


    // Start is called before the first frame update
    void Start()
    {
        //_Slider = GameObject.FindGameObjectWithTag("HP_Slider").GetComponent<Slider>();
        _Slider.minValue = 0f;
        _Slider.maxValue = _HP;
        _Slider.value = _HP;
        _HP_Pickup_Amount = GameControl.Instance._HP_Pickup;
        _Damage = GameControl.Instance._FireDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HP_UP")
        {
            GetHealth(_HP_Pickup_Amount);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Fire")
        {
            TakeDamage(_Damage);
        }
    }

    public void GetHealth(float amount)
    {
        _HP += amount;
        if (_HP > _HP_Max)
        {
            _HP = _HP_Max;
        }
        _Slider.value = _HP;
    }
    public void TakeDamage(float amount)
    {
        _HP -= amount;
        if (_HP <= 0)
        {
            Debug.Log("Player dead...");
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                KampfMinigameController.Instance.Died();
                Revive();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                GameControl.Instance.Exit();
            }
            _Slider.value = 0f;
        }
        else
        {
            _Slider.value = _HP;
        }
    }

    public void Revive()
    {
        _HP = _HP_Max;
        _Slider.value = _HP-1;
    }
}
