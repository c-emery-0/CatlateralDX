using UnityEngine;
using Cinemachine;
 
/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class LockCameraZ : CinemachineExtension
{
    [Tooltip("Lock the camera's Z position to this value")]

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            if (pos.x < -22.68597) pos.x = -22.68597f;
            if (pos.x > -22.68597f + 49.8f - 0.07f) pos.x = -22.68597f + 49.8f - 0.07f;
            if (pos.y < -8.185834) pos.y = -8.185834f;
            if (pos.y > -8.185834 + 17.28) pos.y = -8.185834f + 17.28f;
            state.RawPosition = pos;
        }
    }
}
 