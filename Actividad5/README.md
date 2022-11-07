# Actividad 5

En esta actividad realizaremos pruebas con el Sistema de Waypoints de Unity.

Pasos a seguir:

1. Guardar en nuestra librería, descargar e importar los scripts *WaypointCircuit.cs* y *WaypointProgressTracker.cs* de los *Standard Assets* en nuestro proyecto.
2. Crear un GameObject vacío en nuestra escena que tenga como hijos otros GameObjects vacíos repartidos por la escena que serán puntos que compondrán nuestro circuito y asignar el script *WaypointCircuit.cs* al GameObject vacío padre. Dentro de dicho script, asignaremos todos los GameObjects hijos como parte del circuito de Waypoints.
3. Crear un GameObject de esfera, al que quitaremos la colisión, que actuará como guía para el actor que irá siguiendo el circuito.
4. Crear un GameObject de cápsula, al que también quitaremos la colisión, y asignarle tanto el script de *WaypointProgressTracker.cs* como el que habíamos estado trabajando durante las prácticas anteriores que persigue un objetivo y que en nuestro caso llamamos *Chaser.cs*. En el componente *WaypointProgressTracker* deberemos referenciar el GameObject de circuito que hemos creado antes y el Target, que es la esfera que hemos dicho que actuaría como guía. Por otro lado, en el componente *Chaser* deberemos añadir también la esfera como Goal.

Una vez seguidos todos los pasos anteriores, este es el resultado:

!["Gif de demostración 1"](demo1.gif)