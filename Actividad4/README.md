# Actividad 4

En esta actividad realizaremos pruebas con las diferentes modalidades de uso de eventos para enviar mensajes entre los objetos de la escena en Unity.

Actividades a realizar:

1. Crear un script para mover al objeto jugador con los ejes Horizontal y Vertical
    - Para mover el jugador en el eje vertical, lo que hacemos es coger el valor actual de dicho eje y multiplicarlo por el vector forward local del actor, para luego desplazarlo en coordenadas locales. Tenemos también una variable *SprintMultiplier* que aplicaremos a este movimiento en caso de que la tecla correspondiente (en nuestro caso *LeftShift*) esté pulsada. En el caso de la rotación, aplicaremos el input del eje horizontal al vector up local del actor para rotarlo en torno a ese eje.

    ```
    public class PlayerController : MonoBehaviour
    {
      public float MovementSpeed = 3.0f;
      public float SprintMultiplier = 2.0f;
      public float RotationSpeed = 135.0f;

      void Update()
      {
        Vector3 movementDirection = Input.GetAxisRaw("Vertical") * Vector3.forward;
        float sprintValue = Input.GetKey(KeyCode.LeftShift) ? SprintMultiplier : 1.0f;
        Vector3 rotationDirection = Input.GetAxis("Horizontal") * Vector3.up;
        transform.Translate(movementDirection * MovementSpeed * Time.deltaTime * sprintValue);
        transform.Rotate(rotationDirection * RotationSpeed * Time.deltaTime);
      }
    }
    ```

4. Agrega un objeto que te teletransporte a otra zona de la escena.
    - Para este ejercicio hemos creado una script *Portal* que, cuando detecte solapamiento con algún componente de colisión, automáticamente teletransportará al gameObject dueño de dicha colisión a la posición de otro portal objetivo. Hemos usado la flag *IsTeleporting* para que, en el momento de hacer la teletransportación, cuando el portal destino detecte un nuevo solapamiento, lo ignore; y así evitar que se produzca un bucle infinito en que los dos portales se pasen la patata caliente indefinidamente el uno al otro hasta el fin de los tiempos.

    ```
    public class Portal : MonoBehaviour
    {
        public Portal Goal;
        private bool IsTeleporting = false;
        
        void OnTriggerEnter(Collider other)
        {
            if (IsTeleporting)
            {
                return;
            }

            Goal.IsTeleporting = true;
            other.gameObject.transform.position = new Vector3(Goal.transform.position.x, other.gameObject.transform.position.y, Goal.transform.position.z);
        }
        
        void OnTriggerExit(Collider other)
        {
            IsTeleporting = false;
        }
    }
    ```
    
    Una vez lista la script, crearemos en la escena un par de portales para probarla. Los haremos a partir de una esfera y la estiraremos para que tenga más o menos forma de portal. Como al hacerlo el *Sphere Collider* dejará de encajar bien con la figura, lo cambiaremos por un *Box Collider*, que encajará mucho mejor. Será imprescindible marcar este colisionador como *isTrigger*. Para no tener que repetir todo el proceso, guardaremos el gameObject como prefab y colocaremos otro en la escena. Para distinguirlos, le asignaremos a uno un material de color azul y al otro uno de color naranja. Por último, referenciaremos cada uno de los portales en la script del otro recíprocamente dentro de la variable `public Portal Goal`. Listo, vamos a probarlo:

    ![Gif de demostración 1](demo1.gif)
5. Agrega un personaje que se dirija hacia un objetivo estático en la escena.
6. Agrega un personaje que siga el movimiento del jugador.
    - Realizaremos estos dos últimos ejercicios conjuntamente porque creemos que una sola solución les puede valer a ambos. Crearemos un script que lo que haga sea perseguir el transform de un gameObject en la escena, ya sea este un objeto estático o el player:

    ```
    public class Chaser : MonoBehaviour
    {
        public Transform Goal;
        public float MovementSpeed = 1.0f;
        public float AcceptanceRadius = 1.0f;

        void Update()
        {
            Vector2 transformXZ = new Vector2(transform.position.x, transform.position.z);
            Vector2 goalXZ = new Vector2(Goal.position.x, Goal.position.z);
            Vector2 transformToGoalXZ = goalXZ - transformXZ;
            float transformToGoalDistance = transformToGoalXZ.magnitude;
            if (transformToGoalDistance > AcceptanceRadius)
            {
                Vector3 movementDirection = new Vector3(transformToGoalXZ.x, 0.0f, transformToGoalXZ.y) / transformToGoalDistance;
                transform.Translate(movementDirection * MovementSpeed * Time.deltaTime);
            }
        }
    }
    ```

    Lo único a destacar es que moveremos al perseguidor solamente cuando la distancia entre él y su objetivo sea mayor que su *AcceptanceRadius*, a fin de evitar el efecto conocido como *jittering*, con el cual el perseguidor se quedaría oscilando indefinidamente alrededor de su objetivo porque nunca se daría el caso de que su posición fuera exactamente la misma que la de su objetivo. También ignoramos por completo la coordenada vertical Y en el momento de hacer el movimiento porque por ahora nos interesa que los actores mantengan siempre la misma posición vertical. Una vez lista la script, crearemos un par de cápsulas que se encarguen de perseguir cada una a su objetivo. La cápsula de un gris más oscuro perseguirá una especie de banderín de meta que crearemos en la escena con un cilindro y un plano y la cápsula de un gris más clarito perseguirá al player. Este es el resultado:

    !["Gif de demostración 2"](demo2.gif)