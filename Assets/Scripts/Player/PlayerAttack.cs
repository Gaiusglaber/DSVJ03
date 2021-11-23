using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("OnAttack");
        }
    }
}