using System;
using System.Runtime.InteropServices;

namespace DefaultNamespace
{
    public class CrowdInputReliability
    {
        private IntPtr m_NativeObject = IntPtr.Zero;
        
        public CrowdInputReliability()
        {
            m_NativeObject = Internal_CreateCCrowdInputReliability();
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
        
        public int GetNumber(int SomeParameter)
        {
            if (m_NativeObject == IntPtr.Zero)
                throw new Exception("No native object");
            return Internal_GetNumber(m_NativeObject, SomeParameter);
        }
        
        [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "Internal_CreateCCrowdInputReliability")]
        private static extern IntPtr Internal_CreateCCrowdInputReliability();
        
        [DllImport("CROWDINPUTRELIABILITY")]
        private static extern void Internal_DestroyCCrowdInputReliability(IntPtr obj);
        
        [DllImport("CROWDINPUTRELIABILITY")]
        private static extern int Internal_GetNumber(IntPtr obj, int SomeParameter);
    }
}