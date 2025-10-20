using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Entities
{
    public class Collectible
    {
        // Properties
        public GamePiece Piece { get; }
        public string Type { get; }

        // Constructor
        public Collectible(GamePiece piece, string type)
        {
            Piece = piece;
            Type = type;
        }
    }
}
