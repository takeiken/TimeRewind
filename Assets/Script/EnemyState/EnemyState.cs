using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public bool isCompleted { get; protected set; }
    protected float startTime;
    public float time => Time.time - startTime;

    protected Rigidbody2D body;
    protected Animator animator;
    protected EnemyMovement enemyInput;

    public virtual void Enter() { }
    public virtual void Do() { }
    public virtual void FixedDo() { }
    public virtual void Exit() { }

    public void Setup(Rigidbody2D _body, EnemyMovement _input, Animator _animator = null)
    {
        body = _body;
        animator = _animator;
        enemyInput = _input;
    }

    public void Initialize()
    {
        isCompleted = false;
        startTime = Time.time;
    }
}
