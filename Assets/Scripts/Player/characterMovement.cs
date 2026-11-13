using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterMovement : MonoBehaviour
{
    #region Movement
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    private InputAction moveAction;
    private Vector2 moveValue;

    private void Movement()
    {
        moveValue = new Vector2(moveAction.ReadValue<Vector2>().x, moveAction.ReadValue<Vector2>().y);
        rb.AddForce(moveValue * moveSpeed * 10f * Time.deltaTime, ForceMode2D.Force);
    }
    private void LimitSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = moveValue.normalized * maxSpeed;
        }
    }
    #endregion
    #region Built in functions
    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        if (moveAction != null) moveAction.Enable();
    }
    private void Update()
    {
        LimitSpeed();
        Movement();
        Debug.Log(rb.linearVelocity.magnitude);
    }
    #endregion
}
