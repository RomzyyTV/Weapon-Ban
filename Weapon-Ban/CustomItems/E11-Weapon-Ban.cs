using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using LabApi.Events.Arguments.PlayerEvents;

namespace Weapon_Ban.CustomItems;

[CustomItem(ItemType.GunE11SR)]
public class E11_Weapon_Ban : CustomWeapon
{
    public override uint Id { get; set; } = Plugin.Singleton.Config.ID;
    public override string Name { get; set; } = Plugin.Singleton.Config.Name;
    public override string Description { get; set; } = Plugin.Singleton.Config.Description;
    public override float Weight { get; set; } = Plugin.Singleton.Config.Weight;
    public override SpawnProperties SpawnProperties { get; set; }
    public override float Damage { get; set; }

    protected override void OnShot(ShotEventArgs ev)
    {
        if (!Plugin.Singleton.Config.UserIdWhitelist.Contains(ev.Player.UserId))
        {
            ev.Item.Destroy();
            ev.Player.ShowHint(Plugin.Singleton.Config.ShowHintIfIsNotAllowed, Plugin.Singleton.Config.ShowHintDuration);
            return;
        }
        else
        {
            ev.Target.Ban(Plugin.Singleton.Config.TimeBan, Plugin.Singleton.Config.Reason);
        }
        base.OnShot(ev);
    }
}