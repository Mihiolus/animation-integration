using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    private Rigidbody[] _ragdoll;
    public bool IsAlive = true;

    private void Awake()
    {
        if (!_animator)
        {
            _animator = GetComponentInChildren<Animator>();
        }
        _ragdoll = _animator.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in _ragdoll)
        {
            rb.isKinematic = true;
        }
    }

    public void Die()
    {
        _animator.enabled = false;
        foreach (var rb in _ragdoll)
        {
            rb.isKinematic = false;
        }
        IsAlive = false;
    }

    public void Respawn()
    {
        _animator.enabled = true;
        foreach (var rb in _ragdoll)
        {
            rb.isKinematic = true;
        }
        IsAlive = true;
    }
}
