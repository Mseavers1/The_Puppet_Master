using UnityEngine;

namespace Source.Game
{
    /*
     * By Nebula Coding
     * Edited and Tweaks for project -- Michael Seavers
     * https://www.youtube.com/watch?v=nADIYwgKHv4&t=259s
     */
    public class Room
    {
        public Vector2 GridPos;
        public int Type;
        public bool DoorTop, DoorBot, DoorLeft, DoorRight;
        public Room(Vector2 gridPos, int type){
            GridPos = gridPos;
            Type = type;
        }
    }
}
