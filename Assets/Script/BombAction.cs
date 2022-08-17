using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    public GameObject bombEffect;
    public int attackPower = 20;
    public float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider hit in cols)
        {
            EnemyFSM enemy = hit.GetComponent<EnemyFSM>(); 
            if(enemy != null)
            {
                enemy.HitEnemy(attackPower);
                continue;
            }
        }

        GameObject eff = Instantiate(bombEffect);

        eff.transform.position = transform.position;

        Destroy(gameObject);
    }
}