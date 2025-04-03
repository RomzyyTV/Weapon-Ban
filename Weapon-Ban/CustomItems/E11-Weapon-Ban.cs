using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;
using System.Collections.Generic;
using System.Linq;
using Weapon_Ban;

namespace Site16Essentials.Items.Firearm
{
    [Exiled.API.Features.Attributes.CustomItem(ItemType.GunRevolver)]
    public class FuckYouGun : CustomWeapon
    {
        public override uint Id { get; set; } = Plugin.Singleton.Config.ID;
        public override string Name { get; set; } = Plugin.Singleton.Config.Name;
        public override string Description { get; set; } = Plugin.Singleton.Config.Description;
        public override float Weight { get; set; } = 2f;
        public override float Damage { get; set; } = 10f;

        public override AttachmentName[] Attachments { get; set; } = new[]
        {
            AttachmentName.Flashlight
        };

        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
        };

        public enum AvailableBanTimes
        {
            Kick,
            OneDayBan,
            OneWeekBan,
            PermBan,
        }

        private Dictionary<Player, AvailableBanTimes> _setMode = new();

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }

        public override void Give(Exiled.API.Features.Player player, bool displayMessage = true)
        {
            if (!Plugin.Singleton.Config.UserIdWhitelist.Contains(player.UserId))
            {
                player.Broadcast(10, "This weapon is dev only, try asking a dev like a good boy.");
                Item deniedItem = player.Items.FirstOrDefault(item => item.Type == ItemType.GunRevolver);
                player.RemoveItem(deniedItem);
                return;
            }

            if (!_setMode.ContainsKey(player))
            {
                _setMode.Add(player, AvailableBanTimes.Kick);
            }

            _setMode.TryGetValue(player, out AvailableBanTimes receivedMode);
            player.ShowHint($"\n\n\nGun mode changed to <B>{receivedMode}</B>", 5f);
            base.Give(player, displayMessage);
        }

        protected override void OnPickingUp(PickingUpItemEventArgs ev)
        {
            if (!ev.Player.RemoteAdminAccess)
            {
                ev.Player.ShowHint("You aren't allowed this weapon as you aren't a staff member.");
                ev.IsAllowed = false;
                return;
            }

            if (!_setMode.ContainsKey(ev.Player))
            {
                _setMode.Add(ev.Player, AvailableBanTimes.Kick);
            }

            _setMode.TryGetValue(ev.Player, out AvailableBanTimes receivedMode);
            ev.Player.ShowHint($"\n\n\nGun mode changed to <B>{receivedMode}</B>", 5f);
            base.OnPickingUp(ev);
        }

        protected override void OnDroppingItem(DroppingItemEventArgs ev)
        {
            if (ev.IsThrown)
            {
                _setMode.TryGetValue(ev.Player, out AvailableBanTimes previousMode);

                _setMode.Remove(ev.Player);

                AvailableBanTimes newMode = previousMode + 1;
                if (previousMode == AvailableBanTimes.PermBan)
                {
                    newMode = AvailableBanTimes.Kick;
                }

                _setMode.Add(ev.Player, newMode);

                ev.Player.ShowHint($"\n\n\nGun mode changed to <B>{newMode}</B>", 5f);

                ev.IsAllowed = false;
            }

            base.OnDroppingItem(ev);
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            if (ev.Target == null) return;
            if (!Check(ev.Player.CurrentItem)) return;

            if (Plugin.Singleton.Config.UserIdWhitelist.Contains(ev.Target.UserId))
            {
                ev.Player.ShowHint(
                    "You can't ban a dev, because they are too sigma. \nTherefore you have to suffer the consequences.");
                ev.Player.Vaporize();
                return;
            }

            _setMode.TryGetValue(ev.Player, out AvailableBanTimes receivedMode);

            switch (receivedMode)
            {
                case AvailableBanTimes.Kick:
                    ev.Target.Kick("Kicked from the server by the Ban Gun", ev.Player);
                    break;

                case AvailableBanTimes.OneDayBan:
                    ev.Target.Ban(86400, "Banned from the server by the Ban Gun", ev.Player);
                    break;

                case AvailableBanTimes.OneWeekBan:
                    ev.Target.Ban(604800, "Banned from the server by the Ban Gun", ev.Player);
                    break;

                case AvailableBanTimes.PermBan:
                    ev.Target.Ban(999999999, "Permanently banned from the server by the Ban Gun", ev.Player);
                    break;
            }
        }
    }
}