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

    public List<string> UserIdWhitelist { get; set; } = new List<string>()
    {
        "@steam"
    };
}