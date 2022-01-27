# PEC3

En esta PEC se ha realizado un juego de plataformas con artillería.

El objetivo del juego es alcanzar el diamante evitando ser eliminado por los enemigos, el jugador dispone de tres vidas en las que puede ser derrotado por un enemigo, pero si cae al vacío será eliminado directamente y deberá reiniciar el nivel con todos los enemigos en él. Se busca que sea un nivel sencillo sin un gran nivel de dificultad ni estrés.

Lo primero que se ha hecho ha sido pintar el escenario, para ello se ha usado el siguiente set de [asets](https://opengameart.org/content/jumpo-platformer-assets). Se ha importado como tilemap y se ha generado la paleta en cuestión, de esta manera ha sido mucho más sencillo completar todo el nivel.
Para los personajes se ha utilizado otro set llamado [Sunny Land](https://assetstore.unity.com/packages/2d/characters/sunny-land-103349). Este set es muy completo ya que contiene todos los sprites necesarios para las animaciones.
Para la música se ha utilizado el pack [Platformer Game Music Pack](https://opengameart.org/content/platformer-game-music-pack) creado por [CodeManu](https://opengameart.org/users/codemanu).
Finalmente se ha utilizado el pack de assets [2D Pixel Spaceship](https://assetstore.unity.com/packages/2d/characters/2d-pixel-spaceship-two-small-ships-131545) para el proyectil.

Una vez diseñado el escenario y escogido el personaje protagonista se han añadido los componentes _Rigidbody2D_ y _Colliders_, escogiendo en cada caso la forma más adecuada para el tipo de personaje u objeto, por ejemplo, tanto para el protagonista como para los enemigos se han usado dos tipos de collider, uno circular y otro rectangular, para conseguir una física más ajustada y realista. También se han añadido objetos vacíos con _Colliders_ marcados con _"IsTrigger"_ para los agujeros que harán que el personaje muera al caer.

Una vez decidido el diseño se ha realizado la codificación de los Scripts, lo primero que se ha codificado ha sido el movimiento del protagonista.
Para ello se han usado dos scripts, uno llamado **CharacterController2D** que controla los aspectos fundamentales del movimiento, por ejemplo, es el encargado de controlar si el protagonista está tocando al suelo, a un enemigo, a un trigger o al diamante final. También es el encargado de girar al personaje cuando se mueva hacia atrás y demás. En este script hay diversas variables marcadas como _"SerializeField"_, esta característica que nos permite modificar el valor de las variables con el juego en funcionamiento nos ha resultado muy útil para ajustar los parámetros de la forma más adecuada y no encontrarse con un protagonista que sale volando hasta el infinito o que resbala demasiado al moverse.
El otro script asociado al movimiento del personaje es el **PlayerMovement**, este solo controla los inputs y los elementos relacionados con la animación.

También se ha programado un script para la cámara que hace que esta siga al personaje sin moverse en el eje vertical, de esta forma la cámara es más parecida a la del juego original.

Para controlar la bala e indicar que debe herir a los enemigos y destruirse al colisionar se ha creado el script **BulletController**, y un script llamado **Extension Methods** que nos servirá para convertir un Vector3 en un Vector2. Además hemos modificado el script **Enemy** para añadir la función _Hurt_, que realizará la animación correspondiente a la muerte del enemigo y lo destruirá. También se ha añadido la función _FixedUpdate_ que hace que el enemigo gire al encontrar un obstaculo o el vacío, de manera que no desaparezcan si no los destruye el jugador.

El script **CharacterController** también ha sido modificado para añadir la salud del jugador, es decir, los tres corazones, y la función que detectará cuando el jugador está tocando al enemigo desde encima, para así eliminarlo. Para ello se han modificado las funciones _OnCollisionEnter2D_ y _Dead_.

Al jugador también se le ha añadido un sistema de partículas que se genera cada vez que este salta o cambia de dirección, para ello solo se ha tenido que definir la función _CreateDust_ y llamarla desde las funciones correspondientes.

Para realizar esta PEC se han utilizado los siguientes tutoriales:

- [Unity Platformer Tutorial - Part 4 - Enemy Movement - Devin Curry](https://www.youtube.com/watch?v=LPNSh9mwT4w&t=1254s&ab_channel=DevinCurry)
- [Unity Platformer Tutorial - Part 5a - Combat Player Health - Devin Curry](https://www.youtube.com/watch?v=kRsBoo2p6uw&t=1120s&ab_channel=DevinCurry)
- [Unity Platformer Tutorial Part 5b Combat Enemy Death - Devin Curry](https://www.youtube.com/watch?v=e8DXLqGPosI&t=350s&ab_channel=DevinCurry)
- [Dust Effect when Running & Jumping in Unity (Particle Effect) - Press Start](https://www.youtube.com/watch?v=1CXVbCbqKyg&ab_channel=PressStart)
- [COMO DISPARAR EN UNITY 2D - pt 1 - Tutoriales Dingo](https://www.youtube.com/watch?v=MQnKjIWy2qI&t=627s&ab_channel=TutorialesDingo)
