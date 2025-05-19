using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkInputRelay : MonoBehaviourBase
{
    [Tooltip("Input Action Asset reference.")]
    public InputActionReference moveAction;

    public NetworkIdentity Identity;
    public CommandQueue CommandQueue;

    protected override void OnEnable()
    {
      base.OnEnable();
        if (moveAction != null)
    {
      moveAction.action.Enable();
    }
    }

    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.action.Disable();
        }
    }

    private void Update()
    {
        if (Identity == null || !Identity.IsLocal)
        {
            return;
        }

        Vector2 input = moveAction.action.ReadValue<Vector2>();

        if (input != Vector2.zero)
        {
            CommandQueue.Enqueue(Identity.Id, new MoveCommand { Direction = input });
        }
    }
}
