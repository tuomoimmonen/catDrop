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
    //[SerializeField] SkinDataSO skinData;

    //private ObjectPool<Fruit> fruitPool;

    [Header("Events")]
    public static Action<Fruit, Fruit> onFruitCollision;

    [Header("Data")]
    [SerializeField] FruitType fruitType;
    private bool hasCollided;
    private bool canFruitMerge;

    //[SerializeField] float minXpos, maxXpos;

    [Header("Effects")]
    [SerializeField] ParticleSystem mergeParticles;
    [SerializeField] ParticleSystem[] mergeTextEffects;
    //[SerializeField] ParticleSystem lowComboParticles;
    //[SerializeField] ParticleSystem medComboParticles;
    //[SerializeField] ParticleSystem highComboParticles;


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

    /*
    private void OnEnable()
    {
        Invoke("AllowFruitMerge", 0.5f);
        hasCollided = false;
    }

    private void OnDisable()
    {
        canFruitMerge = false;
        hasCollided = false;
    }
    */

    public void EnablePhysics()
    {
        fruitRb.bodyType = RigidbodyType2D.Dynamic;
        fruitCollider.enabled = true;
    }

    public void DisablePhysics()
    {
        fruitRb.bodyType = RigidbodyType2D.Kinematic;
        //fruitCollider.enabled = false;
    }

    public void MoveTo(Vector2 targetPosition)
    {
        transform.position = targetPosition;
        //if(transform.position.x <= minXpos) { transform.position = new Vector2(minXpos, transform.position.y); }
        //else if(transform.position.x >= maxXpos) { transform.position = new Vector2(maxXpos, transform.position.y); }
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
        //Debug.Log("collision");
        hasCollided = true;

        if (!canFruitMerge) { return; }

        if (collision.collider.TryGetComponent(out Fruit otherFruit))
        {

            if (!otherFruit.CanFruitMerge()) { return; }
            if (otherFruit.GetFruitType() == FruitType.Watermelon) { return; } //guard for not destroying largest fruit

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
        //int index = (int)fruitType;
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
        //DisablePhysics();
        //hasCollided = false;
        //FruitManager.instance.spawnableFruitsPool.Release(this);
        //if(index <= 3) { FruitManager.instance.spawnableFruitsPool.Release(this); }
        //else { FruitManager.instance.mergeFruitsPool.Release(this); }
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
