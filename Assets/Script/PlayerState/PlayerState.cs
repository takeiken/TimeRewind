using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    public bool isCompleted { get; protected set; }
    protected float startTime;
    public float time => Time.time - startTime;

    protected Rigidbody2D body;
    protected Animator animator;
    protected CharacterMovement movementInput;

    public virtual void Enter() { }
    public virtual void Do() { }
    public virtual void FixedDo() { }
    public virtual void Exit() { }

    public void Setup(Rigidbody2D _body, Animator _animator, CharacterMovement _input)
    {
        body = _body;
        animator = _animator;
        movementInput = _input;
    }

    public void Initialize()
    {
        isCompleted = false;
        startTime = Time.time;
    }
}
