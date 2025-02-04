using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformJumpComponent : MonoBehaviour
{
    [SerializeField] private EdgeCollider2D[] edgeColliders;
    [SerializeField] private float jumpForceY = 5f;
    [SerializeField] private float jumpDuration = 1f;

    [SerializeField] private EdgeCollider2D centerCollider;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _capsule;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _capsule = GetComponent<CapsuleCollider2D>();
    }

    public void StartJump()
    {
        StartCoroutine(JumpCoroutine(GetRandomTargetCollider()));
    }

    [ContextMenu("CenterJump")]
    public void JumpToCentralPlatform()
    {
        var currentCollider = GetCurrentCollider();
        if (currentCollider == centerCollider)
            return;

        StartCoroutine(JumpCoroutine(centerCollider));
    }

    private IEnumerator JumpCoroutine(EdgeCollider2D targetCollider)
    {
        if (targetCollider == null)
            yield break;

        var colliderCenter = GetColliderCenter(targetCollider);
        var startPosition = transform.position;
        var elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            PerformJump(startPosition, colliderCenter, elapsedTime / jumpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        FinalizeJump(colliderCenter);
    }

    private EdgeCollider2D GetRandomTargetCollider()
    {
        var currentCollider = GetCurrentCollider();
        var availableColliders = new List<EdgeCollider2D>(edgeColliders);

        if (currentCollider != null)
            availableColliders.Remove(currentCollider);

        return availableColliders.Count > 0
            ? availableColliders[Random.Range(0, availableColliders.Count)]
            : null;
    }

    private EdgeCollider2D GetCurrentCollider()
    {
        foreach (var edgeCollider in edgeColliders)
        {
            if (_capsule.IsTouching(edgeCollider))
                return edgeCollider;
        }

        return null;
    }

    private Vector2 GetColliderCenter(EdgeCollider2D edgeCollider)
    {
        return edgeCollider.transform.TransformPoint(edgeCollider.points[1]);
    }

    private void PerformJump(Vector2 startPosition, Vector2 targetPosition, float progress)
    {
        var newX = Mathf.Lerp(startPosition.x, targetPosition.x, progress);
        var newY = Mathf.Lerp(startPosition.y, targetPosition.y + jumpForceY, progress);

        _rigidbody.MovePosition(new Vector2(newX, newY));
    }

    private void FinalizeJump(Vector2 targetPosition)
    {
        var finalPosition = new Vector2(targetPosition.x, targetPosition.y + jumpForceY);
        _rigidbody.MovePosition(finalPosition);
    }
}