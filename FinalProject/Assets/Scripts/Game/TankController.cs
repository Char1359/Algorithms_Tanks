using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TankController : MonoBehaviour
{
    private Tank tank;


    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Tank>();
    }

    public void OnForwardMovement(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            tank.ForwardMovement = context.ReadValue<float>();
        }
    }

    public void OnTankRotation(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            tank.TankRotation = context.ReadValue<float>();
        }
    }

    public void OnTurretRotation(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            tank.TurretRotation = context.ReadValue<float>();
        }
    }

    public void OnFireProjectile(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            if (context.ReadValue<float>() > 0)
            {
                tank.FireProjectile();
            }
        }
    }
}
