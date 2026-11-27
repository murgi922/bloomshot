using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private GameObject[] equipments;
    [SerializeField] private Animator animator;
    private SpriteRenderer playerSpriteRend;
    private string currentAnimation = "";
    private Vector2 moveValue;
    private bool isBack = false;
    public bool IsBack
    {
        get => isBack;
        set
        {
            if (value != isBack)
            {
                isBack = value;
                ExchangeWeaponInd();
            }
        }
    }
    private void Start()
    {
        playerSpriteRend = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        CheckAnimation();
    }
    private void CheckAnimation()
    {
        if (moveValue.y < -0.2f)
        {
            IsBack = false;
            ChangeAnimation("Walk", 0f);
        }
        else if (moveValue.y > 0.2f)
        {
            IsBack = true;
            ChangeAnimation("BackWalk", 0f);
        }
        else if(IsBack)
        {
            ChangeAnimation("BackIdle", 0f);
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
    public void SendMoveValue(Vector2 value)
    {
        moveValue = value;
    }
    private void ExchangeWeaponInd()
    {
        int ind = playerSpriteRend.sortingOrder;
        int ind1 = equipments[0].GetComponent<SpriteRenderer>().sortingOrder;
        foreach (GameObject obj in equipments)
        {
            obj.GetComponent<SpriteRenderer>().sortingOrder = ind;
        }
        playerSpriteRend.sortingOrder = ind1;
    }
}
