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
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
            foreach (var col in colliders)
            {
                if (col.gameObject.tag == "Enemy")
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
}