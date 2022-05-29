using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum InputType
{
    POWER,
    SPIN,
    RIGHT,
    LEFT,
    UPPER_RIGHT,
    UPPER_LEFT,
    TOGGLE_SPIN
}

public struct InputEventStruct
{
    public InputType Type;
    public bool IsDown;
}

public enum Direction
{
    RIGHT = 1,
    LEFT = 2
}

public class InputService : MonoBehaviour
{
    public static InputService Instance;

    public bool IsPowerDown = false;
    public bool IsRightDown = false;
    public bool IsLeftDown = false;
    public bool IsUpperRightDown = false;
    public bool IsUpperLeftDown = false;

    public bool IsSpinDown = false;

    public Direction SpinDirection;

    public bool IsToggleSpinDown = false;

    public UnityEvent<InputEventStruct> InputEvent;

    private void Awake()
    {
        Instance = this;
        InputEvent = new UnityEvent<InputEventStruct>();
        SpinDirection = Direction.LEFT;
    }

    public void PrimaryDown()
    {
        IsPowerDown = true;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.POWER,
            IsDown = true
        });
    }

    public void PrimaryUp()
    {
        IsPowerDown = false;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.POWER,
            IsDown = false
        });
    }

    public void SecondaryDown()
    {
        IsSpinDown = true;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.SPIN,
            IsDown = true
        });
    }

    public void SecondaryUp()
    {
        IsSpinDown = false;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.SPIN,
            IsDown = false
        });
    }

    public void RightDown()
    {
        IsRightDown = true;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.RIGHT,
            IsDown = true
        });
    }

    public void RightUp()
    {
        IsRightDown = false;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.RIGHT,
            IsDown = false
        });
    }

    public void Left(bool isDown)
    {
        IsLeftDown = isDown;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.RIGHT,
            IsDown = isDown
        });
    }

    public void UpperRight(bool isDown)
    {
        IsUpperRightDown = isDown;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.UPPER_RIGHT,
            IsDown = isDown
        });
    }

    public void UpperLeft(bool isDown)
    {
        IsUpperLeftDown = isDown;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.UPPER_LEFT,
            IsDown = isDown
        });
    }

    public void ToggleSpin(bool isDown)
    {
        if (isDown)
        {
            SpinDirection = SpinDirection == Direction.RIGHT ? Direction.LEFT : Direction.RIGHT;
        }
        IsToggleSpinDown = isDown;
        InputEvent.Invoke(new InputEventStruct
        {
            Type = InputType.TOGGLE_SPIN,
            IsDown = isDown
        });
    }

    public void ClearAll()
    {
        if (IsRightDown)
        {
            RightUp();
        }
        if (IsLeftDown)
        {
            Left(false);
        }
        if (IsPowerDown)
        {
            PrimaryUp();
        }
        if (IsSpinDown)
        {
            SecondaryUp();
        }
        if (IsUpperLeftDown)
        {
            UpperLeft(false);
        }
        if (IsUpperRightDown)
        {
            UpperRight(false);
        }
    }


}
