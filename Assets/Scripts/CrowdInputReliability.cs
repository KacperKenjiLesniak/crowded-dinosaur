using System;
using System.Runtime.InteropServices;

namespace DefaultNamespace
{
    public class CrowdInputReliability
    {
        private IntPtr m_NativeObject = IntPtr.Zero;
        
        public CrowdInputReliability(int numberOfPlayers, int numberOfCommands)
        {
            m_NativeObject = Internal_CreateCCrowdInputReliability(numberOfPlayers, numberOfCommands);
        }
        
        ~CrowdInputReliability()
        { 
            Destroy();
        }
        
        public void Destroy()
        {
            if (m_NativeObject != IntPtr.Zero)
            {
                Internal_DestroyCCrowdInputReliability(m_NativeObject);
                m_NativeObject = IntPtr.Zero;
            }
        }
        
        public int IssueCommands(int[] commands)
        {
            if (m_NativeObject == IntPtr.Zero)
                throw new Exception("No native object");
            return Internal_IssueCommands(m_NativeObject, commands);
        }
        
        [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "Internal_CreateCCrowdInputReliability")]
        private static extern IntPtr Internal_CreateCCrowdInputReliability(int numberOfPlayers, int numberOfCommands);
        
        [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "Internal_DestroyCCrowdInputReliability")]
        private static extern void Internal_DestroyCCrowdInputReliability(IntPtr obj);

        [DllImport("CROWDINPUTRELIABILITY",  EntryPoint = "Internal_IssueCommands")]
        private static extern int Internal_IssueCommands(IntPtr obj, int[] commands);
    }
}