using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DefaultNamespace
{
    public class CrowdInputReliability
    {
        private IntPtr m_NativeObject = IntPtr.Zero;
        public int numberOfPlayers { get; }

        public CrowdInputReliability(
            int numberOfPlayers,
            int numberOfCommands,
            float reliabilityCoefficient,
            float agreementThreshold
        )
        {
            m_NativeObject = Internal_CreateCCrowdInputReliability(numberOfPlayers, numberOfCommands,reliabilityCoefficient, agreementThreshold);
            this.numberOfPlayers = numberOfPlayers;
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

        public List<float> GetPlayerReliabilities()
        {
            var ptr = Internal_GetPlayerReliabilities(m_NativeObject);
            var result = new float[numberOfPlayers];
            Marshal.Copy(ptr, result, 0, numberOfPlayers);
            return result.ToList();
        }

        [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "Internal_CreateCCrowdInputReliability")]
        private static extern IntPtr Internal_CreateCCrowdInputReliability(
            int numberOfPlayers,
            int numberOfCommands,
            float reliabilityCoefficient,
            float agreementThreshold
        );

        [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "Internal_DestroyCCrowdInputReliability")]
        private static extern void Internal_DestroyCCrowdInputReliability(IntPtr obj);

        [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "Internal_IssueCommands")]
        private static extern int Internal_IssueCommands(IntPtr obj, int[] commands);

        [DllImport("CROWDINPUTRELIABILITY", EntryPoint = "Internal_GetPlayerReliabilities")]
        private static extern IntPtr Internal_GetPlayerReliabilities(IntPtr obj);
    }
}