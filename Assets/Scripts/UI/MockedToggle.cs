using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class MockedToggle : MonoBehaviour
{
    [SerializeField] private CrowdConfig crowdConfig;

    private void Start()
    {
        GetComponent<Toggle>().isOn = crowdConfig.mockedCrowdConfig;
    }

    public void Toggle(bool mocked)
    {
        crowdConfig.mockedCrowdConfig = mocked;
    }
}
