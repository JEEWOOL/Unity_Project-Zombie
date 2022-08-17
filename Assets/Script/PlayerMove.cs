using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    CharacterController cc;
    Animator animator;

    public float moveSpeed = 7f;
    [SerializeField]
    private float gravity = -20f;
    public float ySpeed = 0;
    public float jumpPower = 10;
    public bool isJumping = false;
    public int HP = 100;
    int maxHP = 100;
    public Slider HPSlider;
    public GameObject hitEffect;

    public bool cur = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cc = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                isJumping = false;
                ySpeed = 0;
            }
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            ySpeed = jumpPower;
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.gm.gmState != GameManager.GameState.Go)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        animator.SetFloat("MoveMotion", dir.magnitude);

        transform.position = dir * moveSpeed * Time.deltaTime;

        dir = Camera.main.transform.TransformDirection(dir);

        ySpeed += gravity * Time.deltaTime;
        dir.y = ySpeed;

        cc.Move(dir * moveSpeed * Time.deltaTime);

        HPSlider.value = (float)HP / maxHP;
    }    

    public void DamageAction(int damage)
    {
        HP -= damage;

        if (HP > 0)
        {
            StartCoroutine(PlayHitEffect());
        }

        IEnumerator PlayHitEffect()
        {
            hitEffect.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            hitEffect.SetActive(false);
        }
    }
}
