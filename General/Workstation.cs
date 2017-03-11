using System.Runtime.InteropServices;

namespace NoPassword.General
{
    public class Workstation : IWorkstation
    {
        [DllImport("user32")]
        private static extern void LockWorkStation();

        public void Lock()
        {
            LockWorkStation();
        }
    }
}