using System;
using Eiram;
using Events;
using Items;
using Registers;
using UnityEngine;
using Worlds;
using static Eiram.Handles;

namespace Players
{
    public class AttackIndicator : MonoBehaviour
    {
        private bool attackedThisFrame = false;
        private Option<AttackStatus> CurrentAttack = None<AttackStatus>();

        public void Awake()
        {
            EiramEvents.PlayerAttackEvent += PlayerClicking;
        }

        public void LateUpdate()
        {
            // Debug.Log("INDICATOR:: checked last click");
            if (attackedThisFrame)
            {
                attackedThisFrame = false;
            }
            else
            {
                StopAttack();
            }
        }

        public void OnDestroy()
        {
            EiramEvents.PlayerAttackEvent -= PlayerClicking;
        }


        public void PlayerClicking(Vector3Int worldPosition, ItemStack inHand, Player player)
        {
            if(!World.Current.GetTileData(worldPosition).IsSome(out var tileData))
                return;
            
            if(tileData.TileId == TileId.AIR || tileData.TileId == TileId.BEDROCK) return;

            var tile = Register.GetTileByTileId(tileData.TileId);
            
            var item = Register.GetItemByItemId(inHand.ItemId);
            
            attackedThisFrame = true;

            // if we have an active attack and it is in the same position increase percentage else reset
            if (CurrentAttack.IsSome(out var status) && status.WorldPosition == worldPosition)
            {
                // TODO: check for required tool type
                float damage = 1.0f;

                if (item.IsToolItem(out var toolItem, out var _) && tile.RequiredToolType() == toolItem.ToolType && tile.RequiredToolLevel() >= toolItem.ToolLevel)
                {
                    damage *= toolItem.AttackMultipler;
                }
                
                status.Percentage += damage;
                if (!(status.Percentage >= 100.0f)) return;
                
                World.Current.RemoveTileAtAsPlayer(worldPosition, inHand, player);
                StopAttack();
                return;
            }
            
            RestAttackTo(worldPosition);
        }
        
        private void RestAttackTo(Vector3Int worldPosition)
        {
            CurrentAttack = Some(new AttackStatus()
                { Percentage = 0.0f, WorldPosition = worldPosition });

            transform.position = worldPosition + new Vector3(0.5f, 0.5f, 0); // offset to tile grid
        }

        private void StopAttack()
        {
            CurrentAttack = None<AttackStatus>();
            transform.position = new Vector3(0, -100, 0); // offset to tile grid
        }

        class AttackStatus
        {
            public float Percentage;
            public Vector3Int WorldPosition;
        }
    }
}