
import clr
clr.AddReference('littleRunner')
from littleRunner import GameDirection, GameElement, GameInstruction, InstructionType
from littleRunner.GameObjects import MoveType
from littleRunner.GamePhysics import SimpleCrashDetection
import time



class CreateMovingPlatform(object):

	def __init__ (self, obj, handler, MGO, maxDistance, speed=4, startDirection=GameDirection.Right, moveBack=True, startOnOver=True):
		self.handler = handler
		self.startOnOver = startOnOver
      
		self.handler[obj.Name].onOver = self.__onOver
		if not startOnOver:
			self.handler[obj.Name].Check = self.__Check
         
		self.obj = obj
		self.mgoOnTop = False
		self.MGO = MGO
		self.speed = speed
		self.curDistance = 0
		self.maxDistance = maxDistance
		self.direction = startDirection
		self.moveBack = moveBack
		self.flown = False
		self.releaseMGOtime = 0
      

	def __move(self):
		doThen = GameInstruction(InstructionType.MoveElement, self.obj, self.direction, self.speed)
		centripetalForce = self.releaseMGOtime != 0 and time.time()-0.3 < self.releaseMGOtime
		
		if self.mgoOnTop or centripetalForce:
			self.MGO.Move(self.__moveDirection(), self.speed, doThen)
		else:
			doThen.Do()
			
		if self.releaseMGOtime != 0 and not centripetalForce:
			self.releaseMGOtime = 0
       
       
	def __moveDirection(self):
		if self.direction == GameDirection.Right:
			return MoveType.goRight
		elif self.direction == GameDirection.Left:
			return MoveType.goLeft
   
		elif self.direction == GameDirection.Top:
			return MoveType.goTop
		elif self.direction == GameDirection.Bottom:
			return MoveType.goBottom
         
   
	def __flipDirection(self):
		if self.direction == GameDirection.Right:
			self.direction = GameDirection.Left
		elif self.direction == GameDirection.Left:
			self.direction = GameDirection.Right
      
		elif self.direction == GameDirection.Top:
			self.direction = GameDirection.Bottom
		elif self.direction == GameDirection.Bottom:
			self.direction = GameDirection.Top


	def __jumpDirection(self):
		if self.direction == GameDirection.Left:
			return MoveType.jumpLeft
		elif self.direction == GameDirection.Right:
			return MoveType.jumpRight
   
		return None
		

	def __Check(self, newpos):
		self.__move()
		self.curDistance += self.speed
         
		if self.curDistance > self.maxDistance:
			if self.startOnOver:
				del self.handler[self.obj.Name].Check

			self.curDistance = 0
			self.__flipDirection()
		
		
		newMgoOnTop = SimpleCrashDetection(self.MGO, self.obj, 5, 0)
		if self.mgoOnTop and not newMgoOnTop and self.releaseMGOtime == 0: # jump off the platform
			self.releaseMGOtime = time.time()

		self.mgoOnTop = newMgoOnTop


	def __onOver(self, geventhandler, who, direction):
		if who == GameElement.MGO and direction == GameDirection.Top \
			and (self.moveBack or not self.flown):

			self.mgoOnTop = True
			self.handler[self.obj.Name].Check = self.__Check
			self.flown = True
