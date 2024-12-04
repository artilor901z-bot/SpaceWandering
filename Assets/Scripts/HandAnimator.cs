using UnityEngine;

public class HandAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // 动画参数名称
    private const string CLICK_TRIGGER = "Click";
    
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 确保组件存在
        if (animator == null)
        {
            Debug.LogError("Hand GameObject missing Animator component!");
        }
        if (spriteRenderer == null)
        {
            Debug.LogError("Hand GameObject missing SpriteRenderer component!");
        }
    }

    public void PlayClickAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(CLICK_TRIGGER);
        }
    }
}