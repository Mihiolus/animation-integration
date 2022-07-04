using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _rotationSpeed = 180f;
    [SerializeField]
    private Transform _enemy;
    [SerializeField]
    private float _killDistance = 2f;
    [SerializeField]
    private GameObject _finishPrompt;

    private void Awake()
    {
        if (!_animator)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        bool isRunning = !(Mathf.Approximately(x, Mathf.Epsilon)
                             && Mathf.Approximately(z, Mathf.Epsilon));
        _animator.SetBool("Running", isRunning);
        if (isRunning)
        {
            var velocity = new Vector3(x, 0, z);
            velocity = Camera.main.transform.TransformDirection(velocity);
            velocity = Vector3.ProjectOnPlane(velocity, Vector3.up);
            velocity = velocity.normalized * _speed;
            transform.position += velocity * Time.deltaTime;
            var targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
            transform.rotation =
             Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            var orientationDelta = transform.InverseTransformDirection(velocity.normalized);
            _animator.SetFloat("Move X", orientationDelta.x);
            _animator.SetFloat("Move Y", orientationDelta.z);
        }

        var distToEnemy = (_enemy.position - transform.position).sqrMagnitude;
        _finishPrompt.SetActive(distToEnemy<=_killDistance*_killDistance);
    }
}
