using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fruit : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D fruitRb;
    private CircleCollider2D fruitCollider;

    [Header("Elements")]
    public static Action<Fruit, Fruit> onFruitCollision;

    [Header("Data")]
    [SerializeField] FruitType fruitType;


    private void Awake()
    {
        fruitCollider = GetComponent<CircleCollider2D>();
        fruitRb = GetComponent<Rigidbody2D>();
    }

    public void EnablePhysics()
    {
        fruitRb.bodyType = RigidbodyType2D.Dynamic;
        fruitCollider.enabled = true;
        
    }

    public void DisablePhysics()
    {
        fruitRb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void MoveTo(Vector2 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent(out Fruit otherFruit))
        {
            if(otherFruit.GetFruitType() != fruitType)
            {
                return;
            }
            //Debug.Log("fruit collision" + otherFruit.name);

            onFruitCollision?.Invoke(this, otherFruit);
        }
    }

    public FruitType GetFruitType()
    {
        return fruitType;
    }
}
