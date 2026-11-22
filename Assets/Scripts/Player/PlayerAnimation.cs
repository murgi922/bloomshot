using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    private string currentAnimation = "";
    private void Update()
    {
        CheckAnimation();
    }
    private void CheckAnimation()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            ChangeAnimation("Walk", 0f);
        }
        else ChangeAnimation("Idle", 0f);
    }
    private void ChangeAnimation(string animation, float crossFade = 0.2f)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            animator.CrossFade(animation, crossFade);
        }
    }
}
