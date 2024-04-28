using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fruit : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D fruitRb;
    private Collider2D fruitCollider;
    private Animator fruitAnimator;
    [SerializeField] SpriteRenderer fruitSprite;

    [Header("Events")]
    public static Action<Fruit, Fruit> onFruitCollision;

    [Header("Data")]
    [SerializeField] FruitType fruitType;
    private bool hasCollided;
    private bool canFruitMerge;

    [Header("Effects")]
    [SerializeField] ParticleSystem mergeParticles;
    [SerializeField] ParticleSystem[] mergeTextEffects;


    private void Awake()
    {
        fruitAnimator = GetComponent<Animator>();
        fruitCollider = GetComponent<Collider2D>();
        fruitRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Invoke("AllowFruitMerge", 0.5f);
    }

    private void AllowFruitMerge()
    {
        canFruitMerge = true;
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
        PlayCollisionAnimation();
        ManageCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ManageCollision(collision);
    }

    private void ManageCollision(Collision2D collision)
    {
        hasCollided = true;

        if (!canFruitMerge) { return; }

        if (collision.collider.TryGetComponent(out Fruit otherFruit))
        {

            if (!otherFruit.CanFruitMerge()) { return; }

            if (otherFruit.GetFruitType() != fruitType)
            {
                return;
            }
            //Debug.Log("fruit collision" + otherFruit.name);

            onFruitCollision?.Invoke(this, otherFruit);
        }
    }

    public void HandleMergeParticles()
    {
        if(mergeParticles != null)
        {
            mergeParticles.transform.SetParent(null);
            mergeParticles.Play();
        }
        StartCoroutine(StartObjectDestroy());
        //Destroy(gameObject);
    }

    private IEnumerator StartObjectDestroy()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    public void HandleMergeText()
    {
        if(mergeTextEffects != null)
        {
            int randomEffect = UnityEngine.Random.Range(0, mergeTextEffects.Length);
            mergeTextEffects[randomEffect].transform.SetParent(null);
            mergeTextEffects[randomEffect].transform.localScale = Vector3.one; //resets the fruits scale before playing the text bubble
            mergeTextEffects[randomEffect].Play();
        }
    }

    private void PlayCollisionAnimation()
    {
        fruitAnimator.Play("FruitCollision");
    }

    public FruitType GetFruitType()
    {
        return fruitType;
    }

    public Sprite GetFruitSprite()
    {
        return fruitSprite.sprite;
    }

    public bool FruitHasCollided()
    {
        return hasCollided;
    }

    public bool CanFruitMerge()
    {
        return canFruitMerge;
    }

    public Color GetFruitColor()
    {
        return fruitSprite.color;
    }
}
