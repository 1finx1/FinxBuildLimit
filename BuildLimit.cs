using FinxBuildLimit;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinxStructureLimiter
{
    public class FinxStructureLimiter : RocketPlugin<Config>
    {
        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"place_denied", "You cannot place a structure above the specified height ({0}m), you are attempting to place a structure at ({1}m)!"},
        };

        protected override void Load()
        {
            StructureManager.onDeployStructureRequested += OnDeployStructureRequested;
        }

        protected override void Unload()
        {
            StructureManager.onDeployStructureRequested -= OnDeployStructureRequested;
        }





        private void OnDeployStructureRequested(Structure structure, ItemStructureAsset asset, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            // Find the player associated with the owner's CSteamID
            UnturnedPlayer player = UnturnedPlayer.FromCSteamID(new CSteamID(owner));



            if (asset == null)
            {
                // Debug: Log that asset is null
                Debug.LogWarning("Asset is null.");
                return;
            }

            float maxHeight = Configuration.Instance.MaxHeight; // Use a configuration value for the max height

            if (maxHeight <= 0f)
            {
                // Debug: Log that maxHeight is not set
                Debug.LogWarning("MaxHeight is not set.");
                return;
            }

            // Create a raycast from the structure placement point downward
            if (Physics.Raycast(point, Vector3.down, out RaycastHit hit, Mathf.Infinity, RayMasks.GROUND))
            {
                float distanceToGround = hit.distance;

                // Debug: Log the distance to the ground
                Debug.Log($"Distance to ground: {distanceToGround}");

                if (distanceToGround > maxHeight)
                {
                    if (player != null)
                    {
                        string translatedMessage = Translate("place_denied", Configuration.Instance.MaxHeight, distanceToGround.ToString("F2"));
                        ChatManager.serverSendMessage(translatedMessage, Color.red, null, player.SteamPlayer(), EChatMode.SAY);

                        // Debug: Log that placement is denied
                        Debug.LogWarning("Placement denied due to height limit.");

                        shouldAllow = false; // Prevent the structure placement
                    }
                    else
                    {
                        // Debug: Log that player is null
                        Debug.LogWarning("Player is null.");
                    }
                }
            }
        }
    }
}
