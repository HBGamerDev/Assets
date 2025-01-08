using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RebindDisplay : MonoBehaviour
{
    public InputActionReference rebind;
    public Player controls;
    public Text bindingDisplayText;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebinding()
    {
        bindingDisplayText.text = "Press any key";
        
        controls.stats.input.SwitchCurrentActionMap("Cursor");

        rebindingOperation = rebind.action.PerformInteractiveRebinding()
        .WithControlsExcluding("Mouse").OnComplete(operation => RebindComplete()).Start();
    }

    private void RebindComplete()
    {
        int bindIndex = rebind.action.GetBindingIndexForControl(rebind.action.controls[0]);

        bindingDisplayText.text = InputControlPath.ToHumanReadableString(rebind.action.bindings[bindIndex].effectivePath,
        InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        controls.stats.input.SwitchCurrentActionMap("Player");
    }
}
