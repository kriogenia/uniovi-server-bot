# -*- coding: utf-8 -*-
"""
Created on Fri Feb  8 15:19:35 2019
@author: Resonancia Ceniza
"""

#Cargamos nuestro paquete matemático.
import numpy as np
#Cargamos el paquete para las gráficas.
import matplotlib.pyplot as plt

#Definimos nuestra función:
#Nuestra nombre de variable, nombre de función, variables, y comportamiento de función.
f=lambda x:np.exp(x)

#Para representar funcines en intervalos:
#Primero definimos los intervalos acotados mediante sus extremos.
a=-1;b=1;   ##Ojo como hemos definido todo en la misma línea mediante la separación por ";"

#Y ahora calculamos la partición, por ejemplo una malla del número de elementos:
p=500

##Y creamos nuestra malla, con los extremos y el tamaño.
x=np.linspace(a,b,p)    #El último parámetro son las divisiones a crear, en este caso:p

##Ahora calculamos la coordenada y de los respectivos puntos:
y=f(x)

##Imprimimos la gráfica
#plt.plot(x,y) #Coordenada X coordenada Y.

##Si solo queremos los puntos lo hacemos explícitamente mediante el tercer parámetro.
#plt.plot(x,y,'*')    ##También podemos darles valores entre 0 y 5 por ejemplo

##Para asignar leyenda tenemos que asignar etiquetas y ACTIVARLAS después de la creación:

#Para especificar el color usamos la primera letra del nombre del color en ingles:
plt.plot(x,y,'skyblue',label="Exponencial(x)")

##Ej: Gráfica del Coseno:
plt.plot(x,np.cos(x),'lightgreen', label="Coseno(x)")

##Para dibujar el eje OX:
plt.plot(x,0*x,'black',label='EJE OX')
##Si no queremos que se muestren en la misma gráfica usamos show(), que imprime 
##las órdenes anteriores a ella en una gráfica específica.
plt.show()
##Ej: La tangente:
plt.plot(x,np.tan(x),'r--',label="Tangenge(x)") ##Linea discontinua añadiendo --

##Activamos las leyendas:
plt.legend()