using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float m_WalkSpeed = 4.0f;
    public float m_AngularSpeed = 180.0f;

    private Vector3 m_Movement = Vector3.zero;
    private Vector3 m_MousePosition = Vector3.zero;
    private Rigidbody m_Body;

    private void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        KeyboardControl();
        MouseControl();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void KeyboardControl()
    {
        m_Movement.x = Input.GetAxis("Horizontal");
        m_Movement.z = Input.GetAxis("Vertical");
    }

    private void MouseControl()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Camera.main.farClipPlane))
        {
            m_MousePosition = hit.point;
            m_MousePosition.y = transform.position.y;
        }
    }

    private void Move()
    {
        Vector3 displacement = m_Movement.normalized * m_WalkSpeed * Time.fixedDeltaTime;
        m_Body.MovePosition(m_Body.position + displacement);
    }

    private void Rotate()
    {
        Vector3 direction = m_MousePosition - transform.position;
        m_Body.rotation = Quaternion.LookRotation(direction);
    }

}
