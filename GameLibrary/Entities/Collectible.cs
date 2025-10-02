using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Entities
{
    public class Collectible
    {
        public GamePiece Piece { get; }
        public string Type { get; }

        public Collectible(GamePiece piece, string type)
        {
            Piece = piece;
            Type = type;
        }
    }
}
