# Examen Introducción 3D

Este examen consiste en desarrollar una escena 3D que cumpla con los siguientes requsitos:

1. Incorporar un samurai, un grupo de arañas y sus huevos de la asset store.
2. Crear un script para mover el samurai por teclado.
  - Para este ejercicio hemos reutilizado el script *PlayerController* de prácticas pasadas.
3. Crear un script que haga mover a una araña entre dos posiciones fijas.
  - Para este ejercicio hemos reutilizado el script *Chaser* de prácticas pasadas, adaptándolo para tener una lista de objetivos sobre la que se va iterando a medida que se van alcanzando y añadiéndole un *MovementSpeedModifier* similar al que añadimos en el *PlayerController* en la Actividad 4.
4. Crear un script que determine si el samurai está a una distancia de una araña menor que un valor fijo, el resto de arañas deben dirigirse a sus huevos para protegerlos y la araña que se mueve entre dos posiciones fijas duplicar la velocidad.
  - Para este ejercicio hemos creado un script *Watcher* que tendrá un atributo de tipo Transform "Treasure" para los huevos y otro "Threat" para el jugador, y estará pendiente de si nos acercamos demasiado, en cuyo caso buscará todos los *Guardian* y *Patrol* que haya en la escena. Los *Guardian* tendrán un componente *Chaser* al que le añadiremos y quitaremos un Goal que será el "Treasure" según si el player se acerca demasiado a los huevos. Los *Patrol* aplicarán y retirarán un modificador de x2 en su de velocidad de movimiento.

Este es el resultado:

![Gif de demostración](demo.gif)