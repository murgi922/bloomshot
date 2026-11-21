using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string currentAnimation = "";
    private void Update()
    {
        CheckAnimation();
    }
    private void CheckAnimation()
    {
        ChangeAnimation("Idle", 0f);
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
