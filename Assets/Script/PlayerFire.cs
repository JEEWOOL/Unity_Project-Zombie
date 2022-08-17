using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    ParticleSystem ps;
    Animator animator;

    public GameObject[] effectFlash;
    public Text wModeText;
    public GameObject firePosition;
    public GameObject bombFactory;
    public GameObject bulletEffect;
    public float throwPower = 15f;
    public int weaponPower = 5;
    bool zoomMode = false;

    public GameObject weapon01;
    public GameObject weapon02;
    public GameObject crosshair01;
    public GameObject crosshair02;
    public GameObject wRbt01;
    public GameObject wRbt02;
    public GameObject crosshair02_zoom;

    bool fireMode = true;

    AudioSource audioSource;
    public AudioClip audioAttack;
    public AudioClip sniper;
    public AudioClip boom;

    void PlayerSound(string action)
    {
        switch (action)
        {
            case "sochong":
                audioSource.clip = audioAttack;
                break;
            case "sniper":
                audioSource.clip = sniper;
                break;
            case "boom":
                audioSource.clip = boom;
                break;
        }
        audioSource.Play();
    }

    enum WeaponMode
    {
        Normal,
        Sniper
    }
    WeaponMode wMode;


    void Start()
    {  
        ps = bulletEffect.GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        wMode = WeaponMode.Normal;
        fireMode = true;
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {

        if (GameManager.gm.gmState != GameManager.GameState.Go)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                case WeaponMode.Normal:                    
                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    Rigidbody rb = bomb.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    PlayerSound("boom");
                    break;

                case WeaponMode.Sniper:                    
                    if (!zoomMode)
                    {
                        crosshair02.SetActive(true);
                        Camera.main.fieldOfView = 15f;
                        zoomMode = true;
                    }
                    else
                    {
                        crosshair02.SetActive(false);
                        Camera.main.fieldOfView = 60f;
                        zoomMode = false;                        
                    }
                    break;
            }
        }

        if(fireMode == true)
        {
            if (Input.GetMouseButton(0))
            {
                PlayerSound("sochong");
                if (animator.GetFloat("MoveMotion") == 0)
                {
                    animator.SetTrigger("Attack");                    
                }

                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hitInfo = new RaycastHit();

                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                        eFSM.HitEnemy(weaponPower);
                    }

                    else
                    {
                        bulletEffect.transform.position = hitInfo.point;
                        bulletEffect.transform.forward = hitInfo.normal;
                        ps.Play();
                    }
                }

                StartCoroutine(ShootEffectOn(0.05f));
            }
        }

        if(fireMode == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (animator.GetFloat("MoveMotion") == 0)
                {
                    PlayerSound("sniper");
                    animator.SetTrigger("Attack");
                }

                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hitInfo = new RaycastHit();

                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                        eFSM.HitEnemy(weaponPower);
                    }

                    else
                    {
                        bulletEffect.transform.position = hitInfo.point;
                        bulletEffect.transform.forward = hitInfo.normal;
                        ps.Play();
                    }
                }

                StartCoroutine(ShootEffectOn(0.05f));
            }
        }        

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;
            Camera.main.fieldOfView = 60f;
            
            wModeText.text = "Normal Mode";

            fireMode = true;
            weapon01.SetActive(true);
            weapon02.SetActive(false);
            crosshair01.SetActive(true);
            crosshair02.SetActive(false);
            wRbt01.SetActive(true);
            wRbt02.SetActive(false);
        }        
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;
            
            wModeText.text = "Sniper Mode";

            fireMode = false;
            weapon01.SetActive(false);
            weapon02.SetActive(true);
            crosshair01.SetActive(false);
            crosshair02.SetActive(false);
            wRbt01.SetActive(false);
            wRbt02.SetActive(true);
        }
    }

    IEnumerator ShootEffectOn(float duration)
    {
        int num = Random.Range(0, effectFlash.Length -1);
        effectFlash[num].SetActive(true);
        yield return new WaitForSeconds(duration);
        effectFlash[num].SetActive(false);
    }
}