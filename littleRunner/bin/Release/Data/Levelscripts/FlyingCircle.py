
from littleRunner import GameDirection
import math


class FlyingCircle(object):

   def __init__(self, lr, obj, radius, speed=230, dockObj=None, direction=GameDirection.Right):
      self.lr = lr
      lr.Handler[obj.Name].Check += self.__Check
      self.obj = obj
      self.dockObj = dockObj
      
      if dockObj != None:
         self.dockX = obj.Left-dockObj.Left
         self.dockY = obj.Top-dockObj.Top
      else:
         self.xm = obj.Left
         self.ym = obj.Top
      
      self.r = radius
      self.direction = direction
      self.speed = speed
      
      if direction == GameDirection.Right:
         self.deg = 270
      else:
         self.deg = 90


   def __Check(self, newpos):
      if self.direction == GameDirection.Left:
         self.deg *= -1
      
      if self.dockObj == None:
         self.obj.Left = self.xm + self.r * math.cos(math.radians(self.deg))
         self.obj.Top = self.ym + self.r * math.sin(math.radians(self.deg))
      else:
         self.obj.Left = self.dockObj.Left + self.dockX + self.r * math.cos(math.radians(self.deg))
         self.obj.Top = self.dockObj.Top + self.dockY + self.r * math.sin(math.radians(self.deg))

      if self.direction == GameDirection.Left:
         self.deg *= -1


      if self.deg > 360:
         self.deg = 0
      else:
         self.deg += self.speed * self.lr.FrameFactor
