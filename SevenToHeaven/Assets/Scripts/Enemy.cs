using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Rigidbody2D rb2d { get; private set; }
    public float DistanceFromPlayer { get; private set; }

    private void Awake() { // Initialize values 
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Start() {
        PlayerAttack.EnemyList.Add(this);
    }

    public void CalculateDistanceFromPlayer() {
        Vector2 playerPosition = PlayerMovement.Instance.rb2d.position;
        DistanceFromPlayer = Mathf.Sqrt(Mathf.Pow(playerPosition.x - rb2d.position.x, 2) + Mathf.Pow(playerPosition.y - rb2d.position.y, 2)); //Distance formula
    }


}

