using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICurrency
{
    public string Name { get; }
    public void Gain();
    public Transform Tooltip { get; }
}

public class CurrencyMass : ICurrency
{
    public double amount;

    public string Name => "mass";
    public Transform Tooltip => throw new System.NotImplementedException();

    public void Gain()
    {
        
    }
}

public class GameController : SingleBehaviour<GameController>
{
    public ScriptableData overrideData;
    public Data data = new Data();

    protected override void InternalAwake() {}

    protected override void InternalOnDestroy() {}

    private void OnValidate()
    {
        if (overrideData != null)
            overrideData.data.SetInstance();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Controller.CloseApplication();
    }
}

