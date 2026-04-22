using UnityEngine;

namespace StickmanIo.Runtime.Input
{
    public static class CursorViewController 
    {
        public static void Enable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public static void Disable()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}