# Actividad 9

El objetivo de la actividad es experimentar con diferentes técnicas. Concretamente, hay que desarrollar ejemplos que implementen los siguientes casos:

1. Scroll con movimiento del fondo.

En realidad este ejercicio es una versión un pelín más elaborada del siguiente. Para este ejercicio hemos creado un GameObject vacío con un componente que hemos llamado *AutoScroll*. Este componente tiene referencias a la cámara principal de la escena y a dos *SpriteRenderer* (que corresponden al fondo del juego) idénticos y colocados en escena uno junto al otro y alineados con la cámara. Lo que hace el componente *AutoScroll* es desplazar pasivamente los dos fondos en el eje horizontal a la velocidad que le indiquemos y, cuando uno de los fondos se salga del rango de visión de la cámara, lo coloque al otro lado del otro fondo para que vuelva a estar dentro del mismo. Este es el código resultante:

```
public class AutoScroll : MonoBehaviour
{
    public Camera Camera;
    
    public SpriteRenderer Background1;
    public SpriteRenderer Background2;

    public float BackgroundSpeed = -1.0f;
    
    void Update()
    {
        if (!Background1 || !Background2 || !Camera)
        {
            return;
        }

        TranslateAndKeepBackgroundInSight(Background1);
        TranslateAndKeepBackgroundInSight(Background2);
    }

    void TranslateAndKeepBackgroundInSight(SpriteRenderer background)
    {
        background.transform.Translate(BackgroundSpeed * Time.deltaTime * Vector2.right);
        float backgroundWidth = background.bounds.size.x;
        if (background.transform.position.x + backgroundWidth < Camera.transform.position.x)
        {
            background.transform.Translate(2.0f * backgroundWidth * Vector2.right);
        }
        if (background.transform.position.x - backgroundWidth > Camera.transform.position.x)
        {
            background.transform.Translate(-2.0f * backgroundWidth * Vector2.right);
        }
    }
}
```

Este es el resultado:

![Gif de demostración 1](demo1.gif)

2. Scroll con movimiento del personaje.

En este ejercicio teníamos que hacer exactamente lo mismo pero sin todo lo relacionado con el desplazamiento pasivo. Este es el script *ScrollOnPlayerMovement* resultante:

```
public class ScrollOnPlayerMovement : MonoBehaviour
{
    public Camera Camera;
    
    public SpriteRenderer Background1;
    public SpriteRenderer Background2;
    
    void Update()
    {
        if (!Background1 || !Background2 || !Camera)
        {
            return;
        }

        KeepBackgroundInSight(Background1);
        KeepBackgroundInSight(Background2);
    }

    void KeepBackgroundInSight(SpriteRenderer background)
    {
        float backgroundWidth = background.bounds.size.x;
        if (background.transform.position.x + backgroundWidth < Camera.transform.position.x)
        {
            background.transform.Translate(2.0f * backgroundWidth * Vector2.right);
        }
        if (background.transform.position.x - backgroundWidth > Camera.transform.position.x)
        {
            background.transform.Translate(-2.0f * backgroundWidth * Vector2.right);
        }
    }
}
```

![Gif de demostración 2](demo2.gif)

3. Fondo con efecto parallax. El efecto empieza cuando el jugador empieza a moverse, esto se debe comunicar mediante eventos.

A nivel de escena, hemos creado un GameObject vacío con un componente *Parallax* que hemos creado y lo hemos hecho hijo de la cámara principal del juego. Nos hemos bajado un fondo parallax de internet (referenciado al final de este documento) que consistía en una serie de 5 capas: suelo, árboles, montañas, nubes y cielo. En la web donde se ofrecían, se ordenaban las capas en orden ascendiente de más lejanas a más cercanas a la cámara, pero nosotros hemos decidido ordenarlas de más cercanas a más lejanas porque queríamos tener un mayor control sobre la velocidad que iba a tener la capa más cercana, la de suelo, y luego la velocidad que fuera a tener la última, de cielo, ya se vería. Al GameObject con el componente *Parallax* le añadimos como hijos un quad por cada capa del parallax, es decir, 5. Los hemos escalado para que tuvieran un tamaño ligeramente mayor que el rango de la cámara, manteniendo las proporciones de los sprites originales. Los sprites, al importarlos en Unity, tuvimos que indicar que su `Wrap Mode` fuera de tipo `Repeat` para que la textura se pudiese *tilear*, es decir, que si desplazamos la textura lo que veamos sea la misma textura repetida desde el principio, en vez de ver el último píxel de la misma repetido hasta el infinito. Para cada sprite hubo que crear un material con un shader de tipo `Unlit/Transparent` para poder asignarle cada textura a su respectivo quad de los que habíamos creado con anterioridad.

Una vez hecho todo lo anterior y con cada quad con su respectivo material y correctamente situado en la escena, los hemos referenciado desde el componente *Parallax* en orden, como hemos comentado antes, de más cercano a la cámara a más lejano. Lo que haremos entonces será, cada vez que se produzca un movimiento del jugador, desplazar todas las capas de parallax que haya referenciadas en función de este movimiento, desplazando las primeras capas en la cola a más velocidad que las siguientes.

Para estar al tanto de cuándo se mueve el personaje, en la misma clase *PlayerController* que llevamos arrastrando de varias actividades atrás hemos creado un delegado *OnMove* que ejecutaremos cada vez que se produzca un desplazamiento. El componente *Parallax* entonces solo deberá suscribirse a ese evento y ya estará al tanto de cada vez que el jugador se mueva.

Este es el código que hemos añadido a la clase *PlayerController*:

```
public class PlayerController : MonoBehaviour
{
    [...]

    public delegate void Movement(Vector3 dx);
    public event Movement OnMovement;
    
    [...]

    void Update()
    {
        [...]

        // Manage movement
        Vector3 movement = HorizontalInput * MovementSpeed * Vector3.right * Time.deltaTime;
        transform.Translate(movement);
        if (OnMovement != null && movement.magnitude > 0.0f)
        {
            OnMovement(movement);
        }

        [...]
    }
    
    [...]
}
```

Y este es el código resultante en la clase *Parallax*:

```
public class Parallax : MonoBehaviour
{
    public Renderer[] Layers;
    public float BaseSpeed = 0.0525f;
    
    public PlayerController PlayerController;

    void Start()
    {
        PlayerController.OnMovement += UpdateLayers;
    }

    private void UpdateLayers(Vector3 movement)
    {
        for (int i = 0; i < Layers.Length; ++i)
        {
            Material m = Layers[i].material;
            Vector2 movement2D = new Vector2(movement.x, movement.y);
            m.SetTextureOffset("_MainTex", m.GetTextureOffset("_MainTex") + (BaseSpeed * movement2D / (i + 1.0f)));
        }
    }
}
```

La variable *BaseSpeed* hace referencia a la velocidad a la que se moverá la primera capa, es decir, la de suelo, y las siguientes capas se moverán a menor velocidad dependiendo de qué índice de capa les corresponda: a mayor índice, menor velocidad.

Finalmente, este es el resultado:

![Gif de demostración 3](demo3.gif)

4. Utilizar la técnica de pool de objetos para ir creando elementos en el juego que el jugador irá recolectando.

Para este ejercicio, hemos creado dos componentes: un *CoinManager* que, regularmente, va instanciando monedas en el juego que podremos recoger; y un *Coin* que será la propia moneda que podremos recoger.

La moneda simplemente tendrá un delegado *OnPicked* que ejecutará en caso de que detecte una colisión con el jugador. Como veremos más adelante, el *CoinManager* será quien cace este evento. Este es el script *Coin*:

```
public class Coin : MonoBehaviour
{
    public delegate void Picked(Coin coin);
    public event Picked OnPicked;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (OnPicked != null && collision.gameObject.GetComponent<PlayerController>())
        {
            OnPicked(this);
        }
    }
}
```

Como hemos comentado, periódicamente el *CoinManager* instanciará monedas en el juego que podremos recoger. Sin embargo, podemos optimizar el proceso si cada vez que una de ellas es recogida, en vez de destruirla, nos la guardamos en una especie de recámara, que en nuestro caso será un *Stack*, y la esconderemos en la escena para que, cuando volvamos a necesitar hacer aparecer una moneda en escena, en vez de crearla de 0 podamos simplemente reutilizar una previamente creada, lo cual optimizará el proceso. Como hemos comentado, para representar la *coin pool* hemos utilizado un *Stack* porque es una estructura de datos que está especialmente diseñada para estar constantemente sacando y metiendo nuevos datos. En nuestra situación era indiferente utilizar un *Stack* o una *Queue*.

Una vez definido todo lo previamente comentado, dentro del *CoinManager* lo que haremos será, dentro del método *DropCoin* que se estará ejecutando periódicamente, comprobar si en la *coin pool* hay algún elemento. En caso afirmativo, simplemente lo colocaremos en la parte de la escena que queramos y lo haremos visible. En caso de que esté vacía, crearemos un objeto nuevo y lo colocaremos igualmente en la parte de la escena que queramos. Además, cuando se detecte que una moneda ha sido recogida mediante el delegado que hemos comentado previamente, simplemente la esconderemos y la meteremos dentro de la *coin pool*. Crearemos las monedas a partir de un prefab que hemos creado a partir de de un `GameObject -> 2D Object -> Sprites -> Circle` con un *CircleCollider2D*, un *RigidBody2D* y un *Coin*. Este es el script *CoinManager* resultante:

```
public class CoinManager : MonoBehaviour
{
    public Object CoinPrefab;
    
    public float CoinDropRate = 1.0f;
    public float CoinDropHeight = 7.0f;
    public float CoinDropHorizontalRange = 7.0f;
    
    public Stack<Coin> _coinPool;
    
    void Start()
    {
        _coinPool = new Stack<Coin>();
        InvokeRepeating("DropCoin", CoinDropRate, CoinDropRate);
    }

    void DropCoin()
    {
        Debug.Log("Coin Pool item count: " + _coinPool.Count);
        if (_coinPool.Count == 0 && CoinPrefab != null)
        {
            Debug.Log("Coin Pool empty. Instantiating a new coin...");
            Object instCoinObject = Instantiate(CoinPrefab, GetRandomLocationForCoin(), new Quaternion());
            Coin instCoin = instCoinObject.GetComponent<Coin>();
            if (instCoin != null)
            {
                instCoin.OnPicked += HideCoin;
            }
        }
        else
        {
            Debug.Log("Coin Pool not empty. Reusing a coin from the Coin Pool...");
            Coin popCoin = _coinPool.Pop();
            popCoin.transform.position = GetRandomLocationForCoin();
            popCoin.gameObject.SetActive(true);
        }
        
    }

    void HideCoin(Coin coin)
    {
        coin.gameObject.SetActive(false);
        _coinPool.Push(coin);
    }

    Vector3 GetRandomLocationForCoin()
    {
        return new Vector3
        (
            transform.position.x + Random.Range(-CoinDropHorizontalRange, CoinDropHorizontalRange),
            CoinDropHeight,
            transform.position.z
        );
    }
}
```

Este es el resultado:

![Gif de demostración 4](demo4.gif)

# Referencias Assets

- Fondo background scroll: https://www.behance.net/gallery/108797191/Seamless-background-for-2d-games
- Fondo con capas Parallax: https://opengameart.org/content/3-parallax-backgrounds