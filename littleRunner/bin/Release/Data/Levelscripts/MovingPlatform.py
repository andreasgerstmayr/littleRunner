
import clr
clr.AddReference('littleRunner')
from littleRunner import GameDirection, GameElement


class CreateMovingPlatform(object):

   def __init__ (self, obj, handler, MGO, maxDistance, speed=4, startDirection=GameDirection.Right, moveBack=True):
      handler[obj.Name].Check = self.Check
      handler[obj.Name].onOver = self.onOver
      self.obj = obj
      self.flying = False
      self.MGO = MGO
      self.speed = speed
      self.curDistance = 0
      self.maxDistance = maxDistance
      self.direction = startDirection
      self.moveBack = moveBack
      self.flown = False


   def Check(self, newpos):
      if self.curDistance > self.maxDistance:
         self.flying = False
         self.curDistance = 0
         
         if self.direction == GameDirection.Right:
            self.direction = GameDirection.Left
            
         elif self.direction == GameDirection.Left:
            self.direction = GameDirection.Right
         
         
      if self.flying:
      
         if self.direction == GameDirection.Right:
            self.MGO.Left += self.speed
            newpos['left'] += self.speed
         else:
            self.MGO.Left -= self.speed
            newpos['left'] -= self.speed
            
         self.curDistance += self.speed


   def onOver(self, geventhandler, who, direction):
      if who == GameElement.MGO and direction == GameDirection.Top and not self.flying and (self.moveBack or not self.flown):
         self.flying = True
         self.curDistance = 0
         self.flown = True
