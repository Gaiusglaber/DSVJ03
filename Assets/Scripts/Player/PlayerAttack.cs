using UnityEngine;
public class PlayerAttack : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("Attacking",true);
        }

        if (_animator.GetBool("Attacking") && _animator.GetCurrentAnimatorStateInfo(0).IsName("SpinEnd"))
        {
            _animator.SetBool("Attacking", false);
        }
    }
}