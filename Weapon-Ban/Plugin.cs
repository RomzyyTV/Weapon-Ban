using System;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;

namespace Weapon_Ban;
public class Plugin : Plugin<Config>
{
    public override string Author => "RomzyyTV";
    public override string Name => "Weapon Ban Plugin";
    public override Version Version => new(1, 0,0);
    public override Version RequiredExiledVersion => new(Exiled.Loader.Loader.Version.Major, Exiled.Loader.Loader.Version.Minor, Exiled.Loader.Loader.Version.Build);
    public static Plugin Singleton;
    
    public override void OnEnabled()
    {
        CustomItem.RegisterItems();
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        CustomItem.UnregisterItems();
        base.OnDisabled();
    }
}