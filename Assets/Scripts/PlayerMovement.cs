using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform movePoint;
    public Animator anim;
    public InputSystem_Actions playerControls;
    public LayerMask whatStopsMovement;

    public float moveSpeed = 6f;
    private Vector2 direction;

    private InputAction move;

    void Awake()
    {
        movePoint.parent = null;
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move = playerControls.Player.Move;
        move.Disable();
    }


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed*Time.deltaTime);

        direction = move.ReadValue<Vector2>();
        if (direction.x != 0 && direction.y != 0)
        {
            if (direction.x > 0)
            {
                direction.x = 1;
            }
            else
            {
                direction.x = -1;
            }
            direction.y = 0;
        }

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(direction.x) > 0)
            {
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, 0, 0), 0.45f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(direction.x, 0, 0);
                }
            }

            if (Mathf.Abs(direction.y) > 0)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0, direction.y, 0), 0.45f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0, direction.y, 0);
                }
            }
        }
    }
}
