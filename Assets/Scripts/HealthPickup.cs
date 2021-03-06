﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : TriggerObject {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        is_pickup = true;
	}

    public override void GetTriggered() {
        base.GetTriggered();
        if(player_trigger) {
            DoPlayerTrigger();
        } else {
            DoEnemyTrigger();
        }
    }

    protected override void DoPlayerTrigger() {
        if(gm.AdjustHP(1)) {
            Destroy(gameObject);
        }
    }
}
