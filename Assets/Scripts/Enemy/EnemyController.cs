using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float detectionRadiusReaction = 7f;
    [SerializeField] private float detectionRadiusAttack = 2f;

    private bool isPlayerInRangeReaction;
    private bool isPlayerInRangeAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        CheckPlayerRanges();
        UpdateAnimation();
        FlipSpriteTowardsPlayer();
        MoveTowardsPlayer();
        AttackPlayer();
    }

    private void CheckPlayerRanges()
    {
        int playerLayerMask = LayerMask.GetMask("Player");
        isPlayerInRangeReaction = Physics2D.OverlapCircle(transform.position, detectionRadiusReaction, playerLayerMask) != null;
        isPlayerInRangeAttack = Physics2D.OverlapCircle(transform.position, detectionRadiusAttack, playerLayerMask) != null;
    }

    private void UpdateAnimation()
    {
        animator.SetBool("isPlayerInRange", isPlayerInRangeReaction);
    }

    private void FlipSpriteTowardsPlayer()
    {
        if (playerTransform != null)
        {
            float direction = Mathf.Sign(playerTransform.position.x - transform.position.x);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
        }
    }

    private void MoveTowardsPlayer()
    {
        if (animator.GetBool("isWalking") && !isPlayerInRangeAttack && playerTransform != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void AttackPlayer()
    {
        if (isPlayerInRangeAttack && !animator.GetBool("isAttacking"))
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);
        }
    }

    private void OnReactAnimationEnd()
    {
        animator.SetBool("isWalking", true);
    }
    
    private void OnAttackAnimationEnd()
    {
        if (!isPlayerInRangeAttack)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
    }
}
