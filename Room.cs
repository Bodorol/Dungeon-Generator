using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ORDC_Map_Test
{
    
    class Room
    {
         private int xPos;
         private int yPos;
         private List<Room> connectedRooms;
         public int[,] roomMap;
         /* private static List<int[,]> roomMaps = new List<int[,]>() {
            new int[,] {{1, 1, -5, 1, 1},
                        {1, 1, 1, 1, 1},
                        {1, 1, -5, 1, 1}},
            new int[,] {{1, 1, -5, 1, 1},
                        {-5, 1, 1, 1, -5},
                        {1, 1, -5, 1, 1}},
            new int[,] {{1, 1, -5, 1, 1},
                        {-5, 1, 1, 1, -5},
                        {1, 1, -5, 1, 1}},
            new int[,] {{1, 1, 1, -5, 1},
                        {1, 1, 1, 1, -5},
                        {-5, 1, 1, 1, 1},
                        {1, -5, 1, 1, 1}},
         };
 */
        private static List<int[,]> roomMaps = new List<int[,]>() {
            new int[,] {{-7, -7, -5, -7, -7},
                        {-7, 1, 1, 1, -7},
                        {-7, -7, -5, -7, -7}},
            new int[,] {{-7, -7, -5, -7, -7},
                        {-5, 1, 1, 1, -5},
                        {-7, -7, -5, -7, -7}},
            new int[,] {{-7, -7, -5, -7, -7},
                        {-5, 1, 1, 1, -5},
                        {-7, -7, -5, -7, -7}},
            new int[,] {{-7, -7, -7, -5, -7},
                        {-7, 1, 1, 1, -5},
                        {-5, 1, 1, 1, -7},
                        {-7, -5, -7, -7, -7}},
            new int[,] {{-7, -7, -7, -5, -7},
                        {-7, 1, 1, 1, -7},
                        {-5, 1, 1, -7, 0},
                        {-7, -5, -7, -7, 0}},
            new int[,] {{-7, -7, -7, -5, -7},
                        {-7, 1, 1, 1, -5},
                        {-7, -7, -7, 1, -7},
                        {0, 0, -7, -5, -7}},
            new int[,] {{-7, -5, -7},
                        {-7, 1, -7},
                        {-7, 1, -7},
                        {-7, 1, -7},
                        {-7, -5, -7}},
            new int[,] {{-7, -7, -7, -7},
                        {-5, 1, 1, -5},
                        {-7, -7, -7, -7}},
            new int[,] {{-7, -7, -7, -5, -7, -7, -7},
                        {-7, 1, 1, 1, 1, 1, -7},
                        {-7, 1, 1, 1, 1, 1, -7},
                        {-7, 1, 1, 1, 1, 1, -7},
                        {-7, -7, -7, -5, -7, -7, -7}},
            new int[,] {{-7, -7, -7, -5, -7, -7, -7},
                        {-7, 1, 1, 1, 1, 1, -7},
                        {-7, 1, 1, 1, 1, 1, -5},
                        {-7, 1, 1, 1, 1, 1, -7},
                        {-7, -7, -7, -5, -7, -7, -7}}
         };
         public Room(int index) {
             roomMap = roomMaps[index];
             connectedRooms = new List<Room>();
         }

         public static Room randomRoom() {
             Random rand = new Random();
             return new Room(rand.Next(0, roomMaps.Count()));
         }

         public List<char> getDoorDirections() {
             List<char> doorDirections = new List<char>();
             for (int i = 0; i < roomMap.GetLength(1); i++) {
                 if (roomMap[0, i] == -5) {
                     doorDirections.Add('N');
                 }
                 if (roomMap[roomMap.GetLength(0) - 1, i] == -5) {
                     doorDirections.Add('S');
                 }
             }
             for (int i = 0; i < roomMap.GetLength(0); i++) {
                 if (roomMap[i, 0] == -5) {
                     doorDirections.Add('W');
                 }
                 if (roomMap[i, roomMap.GetLength(1) - 1] == -5) {
                     doorDirections.Add('E');
                 }
             }
             return doorDirections;
         }

         public int[] getDoorCoordinates(char direction) {
             int[] coords = new int[2];
             int start = 0;
             int stop = 0;
             switch(direction) {
                case('N'):
                    stop = roomMap.GetLength(1);
                    goto case('H');
                case('S'):
                    start = roomMap.GetLength(0) - 1;
                    stop = roomMap.GetLength(1);
                    goto case('H');
                case('E'):
                    start = roomMap.GetLength(1) - 1;
                    stop = roomMap.GetLength(0);
                    goto case('V');
                case('W'):
                    stop = roomMap.GetLength(0);
                    goto case('V'); 
                case('H'):
                    for (int i = 0; i < stop; i++) {
                        if (roomMap[start, i] == -5) {
                            coords[0] = start;
                            coords[1] = i;
                        }
                    }
                    break;
                case('V'):
                    for (int i = 0; i < stop; i++) {
                        if (roomMap[i, start] == -5) {
                            coords[1] = start;
                            coords[0] = i;
                        }
                    }
                    break;
             }
             return coords;
         }

         public void addConnection(Room connectedRoom) {
             connectedRooms.Add(connectedRoom);
         }

         public List<Room> getConnectedRooms() {
             return new List<Room>(connectedRooms);
         }

         public int getWidth() {
             return roomMap.GetLength(1);
         }

         public int getHeight() {
             return roomMap.GetLength(0);
         }

         public int getXPos() {
             return xPos;
         }

         public int getYPos() {
             return yPos;
         }

         public void setPos(int xPos, int yPos) {
             this.xPos = xPos;
             this.yPos = yPos;
         }
    }
}
