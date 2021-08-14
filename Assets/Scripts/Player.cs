using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float m_WalkSpeed = 4.0f;
    public float m_AngularSpeed = 180.0f;

    private Vector3 m_Movement = Vector3.zero;
    private Rigidbody m_Body;

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Control();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Control()
    {
        m_Movement.x = Input.GetAxis("Horizontal");
        m_Movement.y = 0.0f;  
        m_Movement.z = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        Vector3 displacement = m_Movement.normalized * m_WalkSpeed * Time.fixedDeltaTime;
        m_Body.MovePosition(m_Body.position + displacement);
    }

    private void Rotate()
    {
        Vector3? position = GetMouseInWorldPosition();
        if (position == null) return;
        Vector3 direction = (Vector3)(position - transform.position);
        direction.y = transform.position.y;
        m_Body.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private Vector3? GetMouseInWorldPosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Camera.main.farClipPlane))
        {
            return hit.point;
        }

        return null;
    }
}
