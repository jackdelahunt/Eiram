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
        [SerializeField] private Sprite[] frames;
        private bool attackedThisFrame = false;
        private Option<AttackStatus> CurrentAttack = None<AttackStatus>();
        private SpriteRenderer spriteRenderer;
        
        public const float BASE_DAMAGE = 0.8f;

        public void Awake()
        {
            EiramEvents.PlayerAttackEvent += PlayerClicking;
            spriteRenderer = GetComponent<SpriteRenderer>();
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
            
            attackedThisFrame = true;

            // if we have an active attack and it is in the same position increase percentage else reset
            if (CurrentAttack.IsSome(out var status) && status.WorldPosition == worldPosition)
            {
                float damage = BASE_DAMAGE;
                if (inHand.ItemId != ItemId.UNKNOWN)
                {
                    var item = Register.GetItemByItemId(inHand.ItemId);


                    if (item.IsToolItem(out var toolItem, out var _) &&
                        tile.RequiredToolType() == toolItem.ToolType &&
                        tile.RequiredToolLevel() >= toolItem.ToolLevel)
                    {
                        damage *= toolItem.AttackMultipler;
                    }
                }

                status.Percentage += damage;
                TryChangeFrame(status.Percentage);
                if (!(status.Percentage >= 100.0f)) return;
                
                World.Current.RemoveTileAtAsPlayer(worldPosition, inHand, player);
                StopAttack();
                return;
            }
            
            RestAttackTo(worldPosition);
        }

        private void TryChangeFrame(float percentage)
        {
            int framePercentage = 100 / frames.Length + 1;
            
            int index = (int)percentage / (int)framePercentage;
            if (index == frames.Length) index = frames.Length - 1;
            
            spriteRenderer.sprite = frames[index];
        }

        private void RestAttackTo(Vector3Int worldPosition)
        {
            CurrentAttack = Some(new AttackStatus()
                { Percentage = 0.0f, WorldPosition = worldPosition });

            spriteRenderer.sprite = frames[0];
            transform.position = worldPosition + new Vector3(0.5f, 0.5f, 0); // offset to tile grid
        }

        private void StopAttack()
        {
            spriteRenderer.sprite = frames[0];
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