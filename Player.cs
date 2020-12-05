using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ORDC_Map_Test
{
    
    class Player
    {
         private int xPos;
         private int yPos;

         public Player() {
             xPos = 50;
             yPos = 50;
         }

         public int getXPos() {
             return xPos;
         }

         public int getYPos() {
             return yPos;
         }

         public void moveUp() {
             yPos++;
         }

         public void moveDown() {
             yPos--;
         }

         public void moveRight() {
             xPos++;
         }

         public void moveLeft() {
             xPos--;
         }
    }
}