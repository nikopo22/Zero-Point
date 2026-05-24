using Microsoft.Xna.Framework.Input;
using ZeroPoint.Entities;

namespace ZeroPoint.Controllers;

public class PlayerController
{
    private KeyboardState _previousKeyboardState;

    public PlayerController()
    {
        _previousKeyboardState = Keyboard.GetState();
    }

    public void Update(Player player, KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.A))
            player.MoveLeft();
        else if (keyboardState.IsKeyDown(Keys.D))
            player.MoveRight();
        else
            player.StopMoving();

        if (keyboardState.IsKeyDown(Keys.W) && _previousKeyboardState.IsKeyUp(Keys.W))
            player.Jump();

        if (keyboardState.IsKeyDown(Keys.LeftShift))
            player.ActivateMagnet();
        else
            player.DeactivateMagnet();

        if (keyboardState.IsKeyDown(Keys.E) && _previousKeyboardState.IsKeyUp(Keys.E))
        {
            if (player.ScanAbility.IsActive)
                player.DeactivateScan();
            else
                player.ActivateScan();
        }

        _previousKeyboardState = keyboardState;
    }
}
