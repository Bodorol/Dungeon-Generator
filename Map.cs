using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ORDC_Map_Test
{
    class Map
    {
        private int[,] map;
        private List<Room> rooms;
        private Player player;

        public Map() {
            map = new int[120, 120];
            player = new Player();
            rooms = new List<Room>();
        }

        private bool detectCollisionsForEntireMap() {
            return detectCollisionsForArea(0, 0, map.GetLength(1), map.GetLength(0));
        }

        private bool detectCollisionsForArea(int startX, int startY, int stopX, int stopY) {
            bool hasCollisions = false;
            for (int i = startY; i < stopY; i++) {
                for (int j = startX; j < stopX; j++) {
                    if (map[i, j] >= 2 || map[i, j] <= -10) {
                        hasCollisions = true;
                    }
                }
            }
            return hasCollisions;
        }

        private void placeRoomAtDoorway(Room roomToPlace, Room roomFrom, char doorDirection) {
            Dictionary<char, char> oppositeDirections = new Dictionary<char, char>(){
                {'N', 'S'},
                {'S', 'N'},
                {'E', 'W'},
                {'W', 'E'}
            };
            int[] doorFromCoords = roomFrom.getDoorCoordinates(doorDirection);
            int[] doorToCoords = roomToPlace.getDoorCoordinates(oppositeDirections[doorDirection]);
            if (doorDirection == 'N') {
                placeRoom(roomToPlace, roomFrom.getXPos() + doorFromCoords[1] - doorToCoords[1], roomFrom.getYPos() - roomToPlace.getHeight());
            } else if (doorDirection == 'S') {
                placeRoom(roomToPlace, roomFrom.getXPos() + doorFromCoords[1] -  doorToCoords[1], roomFrom.getYPos() + roomFrom.getHeight());
            } else if (doorDirection == 'E') {
                placeRoom(roomToPlace, roomFrom.getXPos() + roomToPlace.getWidth(), roomFrom.getYPos() + doorFromCoords[0] - doorToCoords[0]);
            } else {
                placeRoom(roomToPlace, roomFrom.getXPos() - roomToPlace.getWidth(), roomFrom.getYPos() + doorFromCoords[0] - doorToCoords[0]);
            }
        }

        private void placeRoom(Room roomToPlace, int xPos, int yPos) {
            for (int i = 0; i < roomToPlace.roomMap.GetLength(0); i++) {
                for (int j = 0; j < roomToPlace.roomMap.GetLength(1); j++) {
                    map[i + yPos, j + xPos] += roomToPlace.roomMap[i, j];
                }
            }
            roomToPlace.setPos(xPos, yPos);
            rooms.Add(roomToPlace);
        }

        private void removeRoom(Room roomToRemove) {
            for (int i = 0; i < roomToRemove.roomMap.GetLength(0); i++) {
                for (int j = 0; j < roomToRemove.roomMap.GetLength(1); j++) {
                    map[i + roomToRemove.getYPos(), j + roomToRemove.getXPos()] -= roomToRemove.roomMap[i, j];
                }
            }
            rooms.Remove(roomToRemove);
        }

        private bool checkIfDoorConnected(Room room, char doorDirection) {
            bool isConnected = false;
            int[] doorCoords = room.getDoorCoordinates(doorDirection);
            switch (doorDirection) {
                case('N'):
                    if (map[room.getYPos() - 1, room.getXPos() + doorCoords[1]] != 0) {
                        isConnected = true;
                    }
                    break;
                case('S'):
                    if (map[room.getYPos() + room.getHeight() + 1, room.getXPos() + doorCoords[1]] != 0) {
                        isConnected = true;
                    }
                    break;
                case('E'):
                    if (map[room.getYPos() + doorCoords[0], room.getXPos() + room.getWidth() + 1] != 0) {
                        isConnected = true;
                    }
                    break;
                case('W'):
                    if (map[room.getYPos() + doorCoords[0], room.getXPos() - 1] != 0) {
                        isConnected = true;
                    }
                    break;
            }
            return isConnected;
        }

        private void generateMapStart(int roomsForward) {
            Room startRoom = Room.randomRoom();
            placeRoom(startRoom, 50 - startRoom.getWidth()/2, 50 - startRoom.getHeight()/2);
            generateMap(startRoom, roomsForward);
        }

        private void generateMap(Room baseRoom, int roomsForward) {
            if (roomsForward <= 0) {
                return;
            }
            List<char> doors = baseRoom.getDoorDirections();
            Dictionary<char, char> oppositeDirections = new Dictionary<char, char>(){
                {'N', 'S'},
                {'S', 'N'},
                {'E', 'W'},
                {'W', 'E'}
            };
            for (int i = 0; i < doors.Count(); i++) {
                Room newRoom;
                int iterations = 0;
                if (checkIfDoorConnected(baseRoom, baseRoom.getDoorDirections()[i])) {
                    continue;
                }
                while (true) {
                    if (iterations > 50) {
                        return;
                    }
                    newRoom = Room.randomRoom();
                    while (!newRoom.getDoorDirections().Contains(oppositeDirections[doors[i]])) {
                        newRoom = Room.randomRoom();
                    }
                    placeRoomAtDoorway(newRoom, baseRoom, doors[i]);
                    if (!detectCollisionsForEntireMap()) {
                        break;
                    } else {
                        removeRoom(newRoom);
                        iterations++;
                    }
                }                    
                newRoom.addConnection(baseRoom);
                baseRoom.addConnection(newRoom);
            }
            for (int i = 1; i < baseRoom.getConnectedRooms().Count(); i++) {
                generateMap(baseRoom.getConnectedRooms()[i], roomsForward - 1);
            }
        }

        public void drawWholeMap() {
            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(1); j++) {
                    if (i == player.getXPos() && j == player.getYPos()) {
                        Console.Write("C ");
                    } else if (map[i, j] == -7) {
                        Console.Write("* ");
                    } else {
                        Console.Write(Math.Abs(map[i, j]) + " ");
                    }
                }
                Console.WriteLine();
            } 
        }

        public void drawPlayerArea() {
            for (int i = Math.Max(player.getYPos() - 4, 0); i < Math.Min(player.getYPos() + 5, 100); i++) {
                for (int j = Math.Max(player.getYPos() - 4, 0); j < Math.Min(player.getYPos() + 5, 100); j++) {
                    if (i == player.getXPos() && j == player.getYPos()) {
                        Console.Write("C ");
                    } else {
                        Console.Write(Math.Abs(map[i, j]) + " ");
                    }
                }
                Console.WriteLine();
            }
        }

        public static void Main(String[] args) {
            Map map = new Map();
            map.generateMapStart(7);
            //Room room1 = new Room(1);
            //map.placeRoom(room1, 50 - room1.getWidth()/2, 50 - room1.getHeight()/2);
            //map.drawPlayerArea();
            //Room room2 = new Room(3);
            //map.placeRoomAtDoorway(room2, room1, 'E');
            //map.drawPlayerArea();
            map.drawWholeMap();
            //Console.WriteLine(room.getDoorDirections()[0]);
            //Console.WriteLine(room.getDoorDirections()[1]);
            //Console.WriteLine(room.getDoorCoordinates('S')[0]);
            //Console.WriteLine(room.getDoorCoordinates('S')[1]);
        }
    }
}
