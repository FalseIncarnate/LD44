﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour {

    protected GameManager gm;
    protected Collider2D myCollider;
    protected SpriteRenderer sr;
    protected Animator anim;

    internal const int NORTH = 1;
    internal const int EAST = 2;
    internal const int SOUTH = 3;
    internal const int WEST = 4;

    internal const int PICKUP_MASK = ~(1 << 8);

    internal const int ENEMY_MASK = ~(1 << 9);

    public bool is_moving = false;
    public bool is_etheral = false;

    public Vector3 move_goal;
    public GameObject health_orb;

    // Use this for initialization
    public virtual void Start () {
        gm = FindObjectOfType<GameManager>();
        myCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        HandleMove();
    }

    protected void AttemptMove(int dir) {
        if(is_moving) {
            return;
        }

        if(!is_etheral && !CheckMove(dir)) {
            return;
        }

        is_moving = true;
        move_goal = GetEndPoint(transform.position, dir);
        HandleMove();
    }

    protected bool CheckMove(int dir) {
        myCollider.enabled = false;

        Vector3 origin_point = transform.position;
        Vector3 end_point = GetEndPoint(origin_point, dir);

        Transform tr = ColliderCheck(origin_point, end_point);

        myCollider.enabled = true;
        if(!tr || tr == null) {
            return true;
        }
        return CheckEnemyDamage(tr);
    }

    protected bool CheckEnemyDamage(Transform tr) {
        if(tr.GetComponent<MoveableObject>() != null) {
            if(tag == "Player") {
                if(tr.tag == "Enemy") {
                    Debug.Log("Player killed enemy in melee!");
                    int vamp_roll = (int)Random.Range(1, 99);
                    if(vamp_roll <= gm.vampire_chance) {
                        GameObject health_drop = Instantiate(health_orb);
                        health_drop.transform.position = tr.position;
                    }
                    tr.GetComponent<MoveableObject>().GetHurt();
                    return true;
                }
            }
            if(tag == "Enemy") {
                if(tr.tag == "Player") {
                    Debug.Log("Enemy attacked player in melee!");
                    int parry_roll = (int)Random.Range(1, 99);
                    if(parry_roll <= gm.parry_chance) {
                        Debug.Log("Player parried!");
                        int vamp_roll = (int)Random.Range(1, 99);
                        if(vamp_roll <= gm.vampire_chance) {
                            GameObject health_drop = Instantiate(health_orb);
                            health_drop.transform.position = tr.position;
                        }
                        GetHurt();
                    } else {
                        gm.AdjustHP(-1);
                    }
                    return false;
                }
                Debug.Log("Enemy duel!");
                if(is_etheral) {
                    if(!tr.GetComponent<MoveableObject>().is_etheral) {
                        //Ghosts kill physical enemies, will duel other ghosts
                        tr.GetComponent<MoveableObject>().GetHurt();
                        return true;
                    }
                }
                if(tr.GetComponent<MoveableObject>().is_etheral) {
                    //Physical enemies can't harm ghosts
                    return true;
                }
                int chance = (int)Random.Range(1, 99);
                if(chance < 50) {
                    GetHurt();
                } else {
                    tr.GetComponent<MoveableObject>().GetHurt();
                }
                return true;
            }
        }
        Collider2D hit_collider = tr.GetComponent<Collider2D>();
        return hit_collider.isTrigger;
    }

    Transform ColliderCheck(Vector3 origin_point, Vector3 end_point) {
        RaycastHit2D hit;
        hit = Physics2D.Linecast(origin_point, end_point, PICKUP_MASK);
        return hit.transform;
    }

    Vector3 GetEndPoint(Vector3 origin_point, int dir) {
        Vector3 end_point = origin_point;
        switch(dir) {
            case NORTH:
                end_point += Vector3.up;
                break;
            case EAST:
                end_point += Vector3.right;
                break;
            case SOUTH:
                end_point += Vector3.down;
                break;
            case WEST:
                end_point += Vector3.left;
                break;
        }
        return end_point;
    }

    protected virtual void HandleMove() {
        if(!is_moving) {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, move_goal, Time.deltaTime * 2.5f);
        if(transform.position == move_goal) {
            is_moving = false;
            return;
        }
    }

    protected void Trigger() {
        RaycastHit2D hit;
        Vector3 origin_point = transform.position;
        hit = Physics2D.Linecast(origin_point, origin_point, ~PICKUP_MASK);
        if(hit && hit.collider.isTrigger) {
            hit.transform.gameObject.GetComponent<TriggerObject>().GetTriggered();
        }

    }

    public virtual void GetHurt() {
        return;
    }

}
