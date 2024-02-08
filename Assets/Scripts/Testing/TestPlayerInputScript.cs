using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerInputScript : MonoBehaviour
{
    public void OnLateral(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"{gameObject.name} performed a lateral input of {context.ReadValue<Vector2>()}");
        }
    }

    public void OnButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"{gameObject.name} performed a button input of {context.ReadValue<Vector2>()}");
        }
    }
}
