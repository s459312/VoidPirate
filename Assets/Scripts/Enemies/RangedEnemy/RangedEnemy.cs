using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy, IRangedEnemy
{
  public GameObject projectile;
  public AudioSource spit;
  public ParticleSystem particles;
  public GameObject cashParticles;
  private bool moneySpawned;

  void Start()
  {
    health = enemyData.maxHealth;
    currState = EnemyState.Idle;
    activeBehaviour = enemyData.activeBehaviour;
    useRoomLogic = enemyData.useRoomLogic;
    isUndestructible = false;
    enemyMovement = GetComponent<RangedEnemyMovement>();

    animator = GetComponent<RangedEnemyAnimator>();

    healthBar = GetComponent<HealthBar>();

    enemyCalculations = GetComponent<RangedEnemyCalculations>();
    enemyMovement.SetPlayerTransform(GameObject.FindGameObjectWithTag("Player").transform);  
    }


  void Update()
  {
    if (currState != EnemyState.Die)
    {
      ScrollStates();
      SelectBehaviour();
    }

  }

  public void ScrollStates()
  {
    switch (currState)
    {

      case (EnemyState.Idle):
        Idle();
        break;
      case (EnemyState.Wander):
        //Wander();
        break;
      case (EnemyState.Follow):
        Follow();
        break;
      case (EnemyState.Die):
        break;
      case (EnemyState.Attack):
        RangedAttack();
        break;
    }
  }
  public void SelectBehaviour()
  {
    if (activeBehaviour)
      ActiveBehaviour();
    else
      PassiveBehaviour();


  }

  public void PassiveBehaviour()
  {
    currState = EnemyState.Idle;
    enemyCalculations.SetNextAttackTime();
    }

  public void ActiveBehaviour()
  {
    if (enemyCalculations.IsInLineOfSight() && !enemyCalculations.IsInAttackRange() && currState != EnemyState.Die)
    {
      currState = EnemyState.Follow;
    }
    else if (!enemyCalculations.IsInLineOfSight() && currState != EnemyState.Die)
    {
      currState = EnemyState.Idle;
    }
    if (enemyCalculations.IsInAttackRange() && enemyCalculations.CanAttack() && currState != EnemyState.Die)
    {
      //Attack();
      currState = EnemyState.Attack;
    }

  }


  public void RangedAttack()
  {
    animator.SetIsAttackingTrue();
    if (enemyCalculations.CanAttack())
    {
      spit.Play();
      Instantiate(projectile, transform.position, Quaternion.identity);
      enemyCalculations.SetNextAttackTime();
    }

  }

  public override void Follow()
  {
    animator.SetIsAttackingFalse();
    if (!audioSource.isPlaying)
      audioSource.Play();
    transform.position = enemyMovement.MoveEnemy(transform.position, enemyData.speed);
  }

  public override void Idle()
  {
    animator?.SetIsAttackingFalse();
  }

  public override void TakeDamage(float damage)
  {
    if (!isUndestructible)
    {
      if (!particles.isPlaying)
        particles.Play();
      healthBar.SetHealthBarActive();
      health -= damage;
      if (health > enemyData.maxHealth)
        health = enemyData.maxHealth;
      healthBar.SetHealthBarValue(enemyCalculations.CalculateHealthPercentage(health));
      CheckDeath(healthBar, animator, gameObject);
    }
  }

  protected override void CheckDeath(HealthBar healthBar, IEnemyAnimator animator, GameObject gameObject)
  {
    if (health <= 0)
    {
      DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
      destroyedEvent.CallDestroyedEvent(false, 1);
      healthBar.SetHealthBarInActive();
      animator.SetIsDeadTrue();

      currState = EnemyState.Die;

      if (useRoomLogic)
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCorutine());

      if (!moneySpawned)
      {
        Instantiate(cashParticles, transform.position, Quaternion.identity);
        moneySpawned = true;
      }

      Destroy(gameObject, enemyData.despawnTimer);
    }
  }

}