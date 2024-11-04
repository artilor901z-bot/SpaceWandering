using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public Scrollbar energyScrollbar;
    private EnergyManager energyManager;

    void Start()
    {
        energyManager = FindObjectOfType<EnergyManager>();
    }

    void Update()
    {
        // 确保 value 在 0 到 1 之间
        energyScrollbar.size = Mathf.Clamp01(energyManager.currentEnergy / energyManager.maxEnergy);
    }
}
