using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ORDC_Map_Test
{
    class Node {
        public Node parent;
        public int xPos;
        public int yPos;
        public int g;
        public int h;
        public int f;

        public Node(Node parent, int xPos, int yPos) {
            this.parent = parent;
            this.xPos = xPos;
            this.yPos = yPos;
        }

        public Node (Node parent, int[] coords) : this(parent, coords[1], coords[0]) {}
    }
    class AStar
    {
        private static List<int[,]> testMatrices = new List<int[,]>() {
            new int[,] {{'7', '7', '2', '7', '7'},
                        {'7', '1', '1', '1', '7'},
                        {'7', '7', '7', '3', '7'}}
        };
        private char source;
        private char goal;
        private char blocked;
        private int[,] testMatrix;

        public AStar(char source, char goal, char blocked) {
            this.source = source;
            this.goal = goal;
            this.blocked = blocked;
            Random rand = new Random();
            testMatrix = testMatrices[rand.Next(0, testMatrices.Count())];
        }

        public List<int[]> search() {
            List<int[]> path = new List<int[]>();
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();
            Node start = new Node(null, findSource());
            start.f = 0;
            openList.Add(start);
            while (openList.Count > 0) {
                Node q = findLeastF(openList);
                openList.Remove(q);
                List<Node> successors = new List<Node>();
                int i = -1;
                do {
                    Node node1 = new Node(q, q.xPos + i, q.yPos);
                    Node node2 = new Node(q, q.xPos, q.yPos + i);
                    if (isNodeValid(node1)) {
                        successors.Add(node1);
                    }
                    if (isNodeValid(node2)) {
                        successors.Add(node2);
                    }
                    i *= -1;
                } while (i != -1);
                foreach (Node successor in successors) {
                    if (isNodeGoal(successor)) {
                        path = returnPath(successor);
                    }
                    successor.g = getDistance(q, successor);
                    successor.h = getDistance(start, successor);
                    successor.f = successor.g + successor.h;
                    bool skipFlag = false;
                    foreach (Node openNode in openList) {
                        if (openNode.xPos == successor.xPos && openNode.yPos == successor.yPos && openNode.f <= successor.f) {
                            skipFlag = true;
                            break;
                        }
                    }
                    foreach (Node closedNode in closedList) {
                        if (closedNode.xPos == successor.xPos && closedNode.yPos == successor.yPos && closedNode.f <= successor.f) {
                            skipFlag = true;
                            break;
                        }
                    }
                    if (skipFlag) {
                        continue;
                    }
                    openList.Add(successor);
                }
                closedList.Add(q);
            }
            return path;
        }

        private int[] findChar(char charToFind) {
            for (int i = 0; i < testMatrix.GetLength(0); i++) {
                for (int j = 0; j < testMatrix.GetLength(1); j++) {
                    if (testMatrix[i, j] == charToFind) {
                        return new int[]{i, j};
                    }
                }
            }
            return new int[]{-1, -1};
        }

        private int[] findSource() {
            return findChar(source);
        }

        private int[] findGoal() {
            return findChar(goal);
        }

        private int getDistance(Node node1, Node node2) {
            return Math.Abs(node1.xPos - node2.xPos) + Math.Abs(node1.yPos - node2.yPos);
        }

        private bool isNodeValid(Node node) {
            return node.xPos >= 0 && node.xPos < testMatrix.GetLength(1) && node.yPos >= 0 && node.yPos < testMatrix.GetLength(0) && testMatrix[node.yPos, node.xPos] != blocked;
        }

        private bool isNodeGoal(Node node) {
            return testMatrix[node.yPos, node.xPos] == goal;
        }

        private Node findLeastF(List<Node> nodes) {
            Node minNode = nodes[0];
            foreach (Node node in nodes) {
                if (node.f < minNode.f) {
                    minNode = node;
                }
            }
            return minNode;
        }

        private void returnPath(List<int[]> path, Node node) {
            if (node.parent is null) {
                path.Add(new int[]{node.yPos, node.xPos});
            } else {
                returnPath(path, node.parent);
                path.Add(new int[]{node.yPos, node.xPos});
            }
        }

        private List<int[]> returnPath(Node node) {
            List<int[]> path = new List<int[]>();
            returnPath(path, node.parent);
            path.Add(new int[]{node.yPos, node.xPos});
            return path;
        }
    }
}