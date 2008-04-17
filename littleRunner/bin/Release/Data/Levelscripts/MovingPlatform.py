
import clr
clr.AddReference('littleRunner')
from littleRunner import GameDirection, GameElement, GameInstruction, InstructionType
from littleRunner.GameObjects import MoveType
import time


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
      self.ignore = False


   def __MGOmove(self, direction):
       doThen = GameInstruction(InstructionType.MoveElement, self.obj, self.direction, self.speed)
       self.MGO.Move(direction, self.speed, doThen)
       
       
   def __move(self, newpos):
      if self.direction == GameDirection.Right:
         self.__MGOmove(MoveType.goRight)
      elif self.direction == GameDirection.Left:
         self.__MGOmove(MoveType.goLeft)
   
      elif self.direction == GameDirection.Top:
         self.__MGOmove(MoveType.goTop)
         
      elif self.direction == GameDirection.Bottom:
         self.__MGOmove(MoveType.goBottom)
         
   
   def __flipdirection(self):
      if self.direction == GameDirection.Right:
         self.direction = GameDirection.Left
      elif self.direction == GameDirection.Left:
         self.direction = GameDirection.Right
      
      elif self.direction == GameDirection.Top:
         self.direction = GameDirection.Bottom
      elif self.direction == GameDirection.Bottom:
         self.direction = GameDirection.Top
		
				
   def Check(self, newpos):
      if self.flying:
         self.__move(newpos)
         self.curDistance += self.speed
         
         if self.curDistance > self.maxDistance:
			self.flying = False
			self.curDistance = 0
         
			self.__flipdirection()
			self.ignore = True


   def onOver(self, geventhandler, who, direction):
      if who == GameElement.MGO and direction == GameDirection.Top \
         and not self.flying and not self.ignore \
         and (self.moveBack or not self.flown):
         
         self.flying = True
         self.curDistance = 0
         self.flown = True
         
      elif self.ignore:
         self.ignore = False
