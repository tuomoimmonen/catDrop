using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergePushEffect : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float pushRadius;
    [SerializeField] Vector2 pushForceMinMax;
    [SerializeField] float pushForce = 1.05f;
    Vector2 pushPosition;
    [SerializeField] bool enableGizmos;
    private void Awake()
    {
        MergeManager.onMergeHandled += MergeHandledCallback;
        SettingsManager.onPushForceChanged += PushForceChangedCallback;
    }
    private void OnDisable()
    {
        MergeManager.onMergeHandled -= MergeHandledCallback;
        SettingsManager.onPushForceChanged -= PushForceChangedCallback;
    }

    private void PushForceChangedCallback(float pushForceValue)
    {
        pushForce = Mathf.Lerp(pushForceMinMax.x, pushForceMinMax.y, pushForceValue);
    }
    private void MergeHandledCallback(FruitType fruitType, Vector2 mergePosition)
    {
        pushPosition = mergePosition;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pushPosition, pushRadius);

        foreach (Collider2D collider in colliders)
        {
            if(collider.TryGetComponent(out Fruit fruit))
            {
                Vector2 force = ((Vector2)fruit.transform.position - mergePosition).normalized;
                force *= pushForce;
                fruit.GetComponent<Rigidbody2D>().AddForce(force);

            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!enableGizmos) { return; }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pushPosition, pushRadius);
    }
#endif

}
