﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision_Down : TriggerObject
{

    // Use this for initialization
    protected override void Start() {
        base.Start();
        is_pickup = true;
        purchase_cost = 1;
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
        gm.ui.UpdateShopText("Vision_Down", purchase_cost);
    }

    public override void OnPurchase() {
        gm.UpdateVision(-1);
        gm.AdjustHP(purchase_cost * -1);
    }
}
