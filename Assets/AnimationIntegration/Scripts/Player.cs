using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f, _finisherApproachSpeed = 10f;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _rotationSpeed = 180f, _finisherRotationSpeed = 720f;
    [SerializeField]
    private Transform _enemy;
    [SerializeField]
    private float _finisherPromptDistance = 2f;
    [SerializeField]
    private GameObject _finisherPrompt;
    [SerializeField]
    private float _finisherDistance = 1f, _finisherAngleError = 5f;
    public bool FinisherRunning;
    private MouseLook _mouseLook;
    [SerializeField]
    private GameObject _gun, _sword;

    private void Awake()
    {
        if (!_animator)
        {
            _animator = GetComponentInChildren<Animator>();
        }
        _mouseLook = GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FinisherRunning)
        {
            return;
        }
        var distToEnemy = (_enemy.position - transform.position).sqrMagnitude;
        bool finisherAvailable = distToEnemy <= _finisherPromptDistance * _finisherPromptDistance;
        _finisherPrompt.SetActive(finisherAvailable);
        if (finisherAvailable && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Finisher());
        }

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
    }

    private IEnumerator Finisher()
    {
        FinisherRunning = true;
        _finisherPrompt.SetActive(false);
        _mouseLook.FollowMouse = false;
        Vector3 distVector = _enemy.position - transform.position;
        float dist = distVector.magnitude;
        
        bool mustApproach = dist > _finisherDistance;
        float angleDelta = Vector3.Angle(distVector, transform.forward);
        var velocity = distVector.normalized * _finisherApproachSpeed;
        if(!mustApproach){
            velocity *= -1f;
        }

        //if starts outside the finisher radius, come closer
        //if starts inside the finisher radius, step back
        while ((mustApproach && dist > _finisherDistance)
         || (!mustApproach && dist < _finisherDistance)
         || angleDelta > _finisherAngleError)
        {
            _animator.SetBool("Running", true);
            if ((mustApproach&& dist > _finisherDistance)
            ||(!mustApproach && dist < _finisherDistance))
            {
                transform.position += velocity * Time.deltaTime;
            }
            var targetRotation = Quaternion.LookRotation(distVector, Vector3.up);
            transform.rotation =
             Quaternion.RotateTowards(transform.rotation, targetRotation, _finisherRotationSpeed * Time.deltaTime);
            var orientationDelta = transform.InverseTransformDirection(velocity.normalized);
            _animator.SetFloat("Move X", orientationDelta.x);
            _animator.SetFloat("Move Y", orientationDelta.z);
            distVector = _enemy.position - transform.position;
            dist = distVector.magnitude;
            angleDelta = Vector3.Angle(distVector, transform.forward);
            yield return null;
        }
        _animator.SetBool("Running", false);
        _gun.SetActive(false);
        _sword.SetActive(true);
        _animator.SetTrigger("Finish");
        while(FinisherRunning){
            yield return null;
        }
        Enemy enemy = _enemy.GetComponent<Enemy>();
        enemy.Die();
        EnemyManager.Instance.Respawn(enemy);
        _gun.SetActive(true);
        _sword.SetActive(false);
        _mouseLook.FollowMouse = true;
    }
}
