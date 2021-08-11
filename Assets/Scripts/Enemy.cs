using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject hitVFX;

    ScoreBoard scoreBoard;
    GameObject parentGameObject;

    [Header("Enemy Health Settings")][Tooltip("How much health the Enemy has")]
    [SerializeField] int enemyHealthPoints = 100;
    [Header("Enemy Hit Score Settings")][Tooltip("How much score is added to scoreboard per laser hit")]
    [SerializeField] int enemyHitScoreAmount = 50;
    [Header("Enemy Hit Damage Settings")][Tooltip("How much damage is applied to enemy per laser hit")]
    [SerializeField] int laserHitDamage = 25;

    void Start()
	{
		scoreBoard = FindObjectOfType<ScoreBoard>();
        parentGameObject = GameObject.FindWithTag("SpawnAtRuntime");
		AddRigidBody();
	}

	private void AddRigidBody()
	{
		Rigidbody rb = gameObject.AddComponent<Rigidbody>();
		rb.useGravity = false;
	}

	private void OnParticleCollision(GameObject other)
	{
		ProcessHit();

	}
    private void ProcessHit()
	{
		PlayHitVFX();
		scoreBoard.IncreaseScore(enemyHitScoreAmount);
		enemyHealthPoints -= laserHitDamage;
		if (enemyHealthPoints <= 0)
		{
			KillEnemy();
		}
	}

	private void PlayHitVFX()
	{
		GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
		vfx.transform.parent = parentGameObject.transform;
	}

	private void KillEnemy()
	{
		GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
		vfx.transform.parent = parentGameObject.transform;
		Destroy(gameObject);
	}

	
}
