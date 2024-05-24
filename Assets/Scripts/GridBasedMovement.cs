using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using Mirror.Examples.AdditiveLevels;
using Unity.Cinemachine;

public class GridBasedMovement : NetworkBehaviour
{

    public Transform movePoint;
    public LayerMask whatStopsMovement;

    public float moveSpeed;

    private Vector2 direction;

    public InputSystem_Actions playerControls;
    private InputAction move;

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movePoint.parent = null;

        if (isLocalPlayer)
        {
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.GetComponent<CinemachineCamera>().Follow = movePoint;
        }
    }

    private void OnEnable()
    {
        movePoint.position = this.transform.position;
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move = playerControls.Player.Move;
        move.Disable();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

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
                    
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, 0, 0), 0.45f, whatStopsMovement))
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
}
