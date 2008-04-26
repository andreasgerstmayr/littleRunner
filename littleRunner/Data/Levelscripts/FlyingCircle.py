
from littleRunner import GameDirection
import math


class CreateFlyingCircle(object):

   def __init__(self, obj, handler, radius, speed=5, direction=GameDirection.Right):
      handler[obj.Name].Check = self.__Check
      self.obj = obj
      self.r = radius
      self.ym = obj.Top
      self.xm = obj.Left

      self.deg = 270
      if direction == GameDirection.Left:
         self.add = (-1) * speed
      elif direction == GameDirection.Right:
         self.add = speed
      else:
         self.add = 0


   def __getCoords(self):
      phi = math.radians(self.deg)
      return self.xm + self.r * math.cos(phi), \
             self.ym + self.r * math.sin(phi)


   def __Check(self, newpos):
      x, y = self.__getCoords()
      self.obj.Top = y
      self.obj.Left = x
      
      self.deg += self.add
      if self.deg < 0:
         self.deg = 360
      elif self.deg > 360:
         self.deg = 0
