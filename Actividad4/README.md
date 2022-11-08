# Actividad 4

En esta actividad realizaremos pruebas con las diferentes modalidades de uso de eventos para enviar mensajes entre los objetos de la escena en Unity.

Actividades a realizar:

1. Crear un script para mover al objeto jugador con los ejes Horizontal y Vertical
    - Para mover el jugador en el eje vertical, lo que hacemos es coger el valor actual de dicho eje y multiplicarlo por el vector forward local del actor, para luego desplazarlo en coordenadas locales. Tenemos también una variable *SprintMultiplier* que aplicaremos a este movimiento en caso de que la tecla correspondiente (en nuestro caso *LeftShift*) esté pulsada. En el caso de la rotación, aplicaremos el input del eje horizontal al vector up local del actor para rotarlo en torno a ese eje.

    ```
    public class PlayerController : MonoBehaviour
    {
      public float MovementSpeed = 3.0f;
      public float RotationSpeed = 135.0f;

      void Update()
      {
        Vector3 movementDirection = Input.GetAxis("Vertical") * Vector3.forward;
        Vector3 rotationDirection = Input.GetAxis("Horizontal") * Vector3.up;
        transform.Translate(movementDirection * MovementSpeed * Time.deltaTime);
        transform.Rotate( rotationDirection * RotationSpeed * Time.deltaTime);
      }
    }
    ```

2. Implementar una UI que permita configurar con qué velocidad te moverás: turbo o normal. También debe mostrar la cantidad de objetos recolectados y si chocas con alguno especial restar fuerza.
    - Para este ejercicio lo primero será ajustar el script *PlayerController* para que el player pueda moverse con un sprint que podremos activar y desactivar desde un toggle que crearemos en la UI:

    ```
    public class PlayerController : MonoBehaviour
    {
      public float MovementSpeed = 3.0f;
      public float RotationSpeed = 135.0f;
      
      public float SprintMultiplier = 2.0f;
      
      private float MovementSpeedModifier = 1.0f;

      void Update()
      {
        float currentMovementSpeed = MovementSpeed * MovementSpeedModifier;
        Vector3 movementDirection = Input.GetAxis("Vertical") * Vector3.forward;
        Vector3 rotationDirection = Input.GetAxis("Horizontal") * Vector3.up;
        transform.Translate(movementDirection * currentMovementSpeed * Time.deltaTime);
        transform.Rotate( rotationDirection * RotationSpeed * Time.deltaTime);
      }

      public void ToggleSprint(Toggle toggle)
      {
        if (toggle.isOn)
        {
          ApplyMovementSpeedModifier(SprintMultiplier);
        }
        else
        {
          ApplyMovementSpeedModifier(1.0f / SprintMultiplier);
        }
      }

      public void ApplyMovementSpeedModifier(float modifier)
      {
        MovementSpeedModifier *= modifier;
      }
    }
    ```

      Tendremos una variable *SprintMultiplier*, que definirá la potencia del sprint, y una variable *MovementSpeedModifier* donde almacenaremos todos los modificadores de velocidad que reciba el personaje. También tendremos un método público que se llamará desde la UI (en concreto desde un elemento *Toggle*) para aplicar (multiplicar) y para retirar (dividir) el efecto del sprint en el modificador de velocidad. Más tarde, solo tendremos que multiplicar el valor del *MovementSpeedModifier* a la *MovementSpeed* para obtener la velocidad de movimiento que queremos usar.

      Necesitaremos también tanto una clase *Collectable*, que poseerán todos los objetos coleccionables; como una clase *Collecter*, que deberemos asignar a nuestro player:

    ```
    public class Collectable : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            Collecter collecter = other.gameObject.GetComponent<Collecter>();
            if (collecter)
            {
                collecter.Collect(this);
                Destroy(gameObject);
            }
        }
    }
    ```

    ```
    public class Collecter : MonoBehaviour
    {
        int NCollected = 0;
        public Text NCollectedUILabel;

        public void Collect(Collectable collectable)
        {
            ++NCollected;
            if (NCollectedUILabel)
            {
                NCollectedUILabel.text = "x " + NCollected;
            }
        }
    }
    ```

      Los objetos de tipo *Collectable* estarán a la escucha de solapamientos con posibles objetos de tipo *Collecter* y, en caso de que un solapamiento se produzca, notificará al *Collecter* solapado, que actualizará el contador de objetos que lleva recogidos y actualizará el texto correspondiente de la UI. Tras todo esto, el *Collectable* se destruirá.

      Para los objetos que deban restar fuerza al personaje, crearemos un script *Glue* que será una especie de losa de pegamento que estará colocada en el suelo y que, al ser pisada por el el player, este será ralentizado. Para eso, detectaremos solapamientos con un *PlayerController* y, en caso de encontrarlo, al comenzar el solapamiento aplicaremos un modificador de velocidad de movimiento correspondiente a la ralentización y, al terminar el solapamiento, retiraremos este modificador.

    ```
    public class Glue : MonoBehaviour
    {
        public float SlowingPower = 2.0f;
        
        void OnTriggerEnter(Collider other)
        {
            ApplyMovementSpeedModifierToPlayerController(other, 1.0f / SlowingPower);
        }
        
        void OnTriggerExit(Collider other)
        {
            ApplyMovementSpeedModifierToPlayerController(other, SlowingPower);
        }

        void ApplyMovementSpeedModifierToPlayerController(Collider other, float modifier)
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.ApplyMovementSpeedModifier(modifier);
            }
        }
    }
    ```

      Para el siguiente ejemplo, utilizaremos 3 objetos de tipo *Glue* con unos *SlowingPower* de 2, 4 y 8 respectivamente. Una vez reunidas todas las piezas previamente expuestas, este es el resultado:

    ![Gif de demostración 1](demo1.gif)

3. Agregar a tu escena un objeto que al ser recolectado por el jugador haga que otros objetos obstáculos se desplacen de su trayectoria.
    - Para este ejercicio simplemente crearemos un script *Obstacle*, así como un script *ObstacleMover*. El *ObstacleMover* detectará solapamientos con objetos de tipo *PlayerController*. En cuanto detecte uno, buscará todos los objetos de tipo *Obstacle* en la escena y les aplicará una fuerza de repulsión tomando como epicentro la posición del jugador, tras lo cual se destruirá.

    ```
    public class Obstacle : MonoBehaviour
    {
        public void Repel(Transform epicenter, float repelForce)
        {
            transform.Translate((transform.position - epicenter.position).normalized * repelForce);
        }
    }
    ```

    ```
    public class ObstacleMover : MonoBehaviour
    {
        public float repelForce = 1.0f;
        
        void OnTriggerEnter(Collider other)
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController)
            {
                foreach (Obstacle obstacle in GameObject.FindObjectsOfType<Obstacle>())
                {
                    obstacle.Repel(playerController.transform, repelForce);
                }
                Destroy(gameObject);
            }
        }
    }
    ```

      Para el siguiente ejemplo, utilizaremos 3 objetos de tipo *ObstacleMover* con unas *repelForce* de 1, 2 y 4 respectivamente. Una vez reunidas todas las piezas previamente expuestas, este es el resultado:

    ![Gif de demostración 2](demo2.gif)

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
        public float SlerpFactor = 0.5f;

        void Update()
        {
            Vector3 goalPositionAtSelfHeight = new Vector3(Goal.position.x, transform.position.y, Goal.position.z);
            if (Vector3.Distance(transform.position, goalPositionAtSelfHeight) > AcceptanceRadius)
            {
                Quaternion DesiredRotation = Quaternion.LookRotation(goalPositionAtSelfHeight - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, DesiredRotation, SlerpFactor);
                transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
            }
        }
    }
    ```

    Moveremos al perseguidor primero rotándolo suavemente hacia su objetivo y luego moviéndolo en su dirección frontal. Solamente lo haremos cuando la distancia entre él y su objetivo sea mayor que su *AcceptanceRadius*, a fin de evitar el efecto conocido como *jittering*, con el cual el perseguidor se quedaría oscilando indefinidamente alrededor de su objetivo porque nunca se daría el caso de que su posición fuera exactamente la misma que la de su objetivo. También ignoramos por completo la coordenada vertical Y en el momento de hacer el movimiento porque por ahora nos interesa que los actores mantengan siempre la misma posición vertical. Una vez lista la script, crearemos un par de cápsulas que se encarguen de perseguir cada una a su objetivo. La cápsula de un gris más oscuro perseguirá una especie de banderín de meta que crearemos en la escena con un cilindro y un plano y la cápsula de un gris más clarito perseguirá al player. Este es el resultado con un *SlerpFactor* de 0,01:

    !["Gif de demostración 2"](demo2.gif)