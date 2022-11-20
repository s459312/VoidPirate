using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkPickup : PickableItem, IPickable
{
    [SerializeField] ScriptableEnergyDrink energyDrink;

    // Update is called once per frame
    void Update()
    {
        InteractOnPickup();
    }


    public override void InteractOnPickup()
    {
        if (Input.GetKeyDown(itemPickupCode) && isInRange)
        {
            EnergyBoost();

            if (destroyOnPickUp)
            {
                Destroy(gameObject);
            }
        }
    }

    private void EnergyBoost()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().Heal(energyDrink.hp_content);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().SpeedBuffSecondsPercentAdd(StatModApplicationType.ConsumableAppliedBuff,energyDrink.percentBuff, energyDrink.duration);
    }
}