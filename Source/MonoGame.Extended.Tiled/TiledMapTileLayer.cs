#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#endregion

namespace MonoGame.Extended.Tiled
{
    public class TiledMapTileLayer : TiledMapLayer
    {
        // immutable
        public int Width { get; }
        public int Height { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }
        public IReadOnlyList<TiledMapTile> Tiles { get; }

        internal TiledMapTileLayer(ContentReader input, TiledMap map) 
            : base(input)
        {
            Width = input.ReadInt32();
            Height = input.ReadInt32();
            TileWidth = map.TileWidth;
            TileHeight = map.TileHeight;

            var tileCount = input.ReadInt32();
            var tiles = new TiledMapTile[tileCount];
            Tiles = new ReadOnlyCollection<TiledMapTile>(tiles);

            for (var i = 0; i < tileCount; i++)
            {
                var globalTileIdentifierWithFlags = input.ReadUInt32();
                var x = input.ReadUInt16();
                var y = input.ReadUInt16();
                tiles[i] = new TiledMapTile(globalTileIdentifierWithFlags, x, y);
            }
        }

        public bool TryGetTile(int x, int y, out TiledMapTile? tile)
        {
            var index = x + y * Width;
            if ((index < 0) || (index >= Tiles.Count))
            {
                tile = null;
                return false;
            }

            tile = Tiles[index];
            return true;
        }
    }
}