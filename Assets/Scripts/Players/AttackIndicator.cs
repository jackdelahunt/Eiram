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
        private AudioSource audioSource;
        
        public const float BASE_DAMAGE = 10.0f;

        public void Awake()
        {
            EiramEvents.PlayerAttackEvent += PlayerClicking;
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
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
                float damage = BASE_DAMAGE * (1.0f / tile.Hardness());
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
                PlayBreakingSound();
                if (!(status.Percentage >= 100.0f)) return;
                
                // break the tile
                PlayBreakSound();
                StopAttack();
                World.Current.RemoveTileAtAsPlayer(worldPosition, inHand, player);
                return;
            }
            
            RestAttackTo(worldPosition);
        }

        private void PlayBreakingSound()
        {
            if(audioSource.isPlaying) return;
            if (CurrentAttack.IsSome(out var attackStatus))
            {
                if (World.Current.GetTileData(attackStatus.WorldPosition).IsSome(out var tileData))
                {
                    var audioClip = Register.GetTileByTileId(tileData.TileId).BreakingSound();
                    if (audioClip != null)
                    {
                        audioSource.clip = audioClip;
                        audioSource.Play();
                    }
                }
            }
        }
        
        private void PlayBreakSound()
        {
            audioSource.Stop();
            audioSource.clip = null;

            if (CurrentAttack.IsSome(out var attackStatus))
            {
                if (World.Current.GetTileData(attackStatus.WorldPosition).IsSome(out var tileData))
                {
                    var audioClip = Register.GetTileByTileId(tileData.TileId).BreakSound();
                    if (audioClip != null)
                    {
                        audioSource.PlayOneShot(audioClip);
                    }
                }
            }
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
            audioSource.loop = false;
        }

        private void StopAttack()
        {
            spriteRenderer.sprite = frames[0];
            CurrentAttack = None<AttackStatus>();
            transform.position = new Vector3(0, -100, 0); // offset to tile grid
            audioSource.loop = false;
        }

        class AttackStatus
        {
            public float Percentage;
            public Vector3Int WorldPosition;
        }
    }
}