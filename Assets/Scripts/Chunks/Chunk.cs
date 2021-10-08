using System.Linq;
using Eiram;
using Registers;
using Tilemaps;
using UnityEngine;

namespace Chunks
{
    public class Chunk
    {
        private TileId[,] tileIds = new TileId[EiramTypes.CHUNK_WIDTH, EiramTypes.CHUNK_HEIGHT];

        public Chunk()
        {
            for (int i = 0; i < tileIds.GetLength(0); i++)
            {
                for (int j = 0; j < tileIds.GetLength(1); j++)
                {
                    tileIds[i, j] = TileId.DIRT;
                }
            }
            Render();
        }

        public void Render()
        {
            for (int i = 0; i < tileIds.GetLength(0); i++)
            {
                for (int j = 0; j < tileIds.GetLength(1); j++)
                {
                    EiramTilemap.Instance.SetTile(
                        new Vector3Int(i, j, 0),
                        tileIds[i, j]);
                }
            }
        }
    }
}