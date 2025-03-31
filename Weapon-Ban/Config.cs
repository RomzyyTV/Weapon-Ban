using System.Collections.Generic;
using Exiled.API.Interfaces;

namespace Weapon_Ban;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; }
    public uint ID { get; set; } = 22;
    public string Name { get; set; } = "WeaponBan";
    public string Description { get; set; } = "Weapon ban";
    public float Weight { get; set; } = 0;
    public int TimeBan { get; set; } = 1000;
    public string Reason { get; set; } = "RIP BOZO";
    public string ShowHintIfIsNotAllowed { get; set; } = "You are not allowed to use this item";
    public float ShowHintDuration { get; set; } = 5f;

    public List<string> UserIdWhitelist { get; set; } = new List<string>()
    {
        "@steam"
    };
}