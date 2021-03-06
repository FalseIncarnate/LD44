﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyControl : MoveableObject {

    protected int move_counter = 0;
    protected const int MOVE_RATE = 60;

    protected const int ENEMY_SIGHT_RANGE = 5;
    protected bool has_moveAnim = true;

    // Use this for initialization
    public override void Start() {
        base.Start();
        gm.enemies_left++;
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        if(is_moving && has_moveAnim) {
            DoMoveAnim();
        }
        move_counter++;
        if(move_counter == MOVE_RATE) {
            seekPlayer();
            move_counter = 0;
        }
        
    }

    protected override void HandleMove() {
        base.HandleMove();
        Trigger();
    }

    protected virtual void seekPlayer() {
        Transform playerTarget = FindObjectOfType<PlayerControl>().transform;
        if(!playerTarget) {
            RandomMove();
            return;
        }
        int difX = (int)Mathf.Abs(transform.position.x - playerTarget.position.x);
        int difY = (int)Mathf.Abs(transform.position.y - playerTarget.position.y);
        if(difX + difY <= Mathf.Max(ENEMY_SIGHT_RANGE + gm.sightMod, 1)) {
            if(difX > 0 && (difY > difX || difY == 0)) {
                if(transform.position.x < playerTarget.position.x) {
                    AttemptMove(EAST);
                } else {
                    AttemptMove(WEST);
                }
            } else {
                if(transform.position.y < playerTarget.position.y) {
                    AttemptMove(NORTH);
                } else {
                    AttemptMove(SOUTH);
                }
            }
        } else {
            RandomMove();
        }
    }

    protected void RandomMove() {
        int rand_dir = (int)Random.Range(1, 6);
        if(rand_dir < 5) {
            AttemptMove(rand_dir);
        }
    }

    public override void GetHurt() {
        gm.enemies_left--;
        Destroy(gameObject);
    }

    protected virtual void DoMoveAnim() {
        anim.SetTrigger("enemyMoving");
    }
}
