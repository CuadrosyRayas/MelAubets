# PEC3

En esta PEC se ha realizado un juego de artillería ampliando el juego de plataformas que se desarrolló en la PEC anterior.

El objetivo del juego es alcanzar el diamante evitando ser eliminado por los enemigos, el jugador dispone de tres vidas en las que puede ser derrotado por un enemigo, pero si cae al vacío será eliminado directamente y deberá reiniciar el nivel con todos los enemigos en él. Se busca que sera un nivel sencillo sin un gran nivel de dificultad ni estrés.

Se han utilizado los assets presentes en la PEC2, junto a un pack de música llamado [Platformer Game Music Pack](https://opengameart.org/content/platformer-game-music-pack) creado por [CodeManu](https://opengameart.org/users/codemanu), también se ha utilizado el pack de assets [2D Pixel Spaceship](https://assetstore.unity.com/packages/2d/characters/2d-pixel-spaceship-two-small-ships-131545) para el proyectil.

Respecto a la codificación del codigo se han añadido dos scripts, el **BulletController**, que nos sirve para controlar la bala e indicar que debe herir a los enemigos y destruirse al colisionar, y un script llamado **Extension Methods** que nos servirá para convertir un Vector3 en un Vector2. Además hemos modificado el script **Enemy** para añadir la función _Hurt_, que realizará la animación correspondiente a la muerte del enemigo y lo destruirá. También se ha añadido la función _FixedUpdate_ que hace que el enemigo gire al encontrar un obstaculo o el vacío, de manera que no desaparezcan si no los destruye el jugador.

El script **CharacterController** también ha sido modificado para añadir la salud del jugador, es decir, los tres corazones, y la función que detectará cuando el jugador está tocando al enemigo desde encima, para así eliminarlo. Para ello se han modificado las funciones _OnCollisionEnter2D_ y _Dead_.

Al jugador también se le ha añadido un sistema de partículas que se genera cada vez que este salta o cambia de dirección, para ello solo se ha tenido que definir la función _CreateDust_ y llamarla desde las funciones correspondientes.

Para realizar esta PEC se han utilizado los siguientes tutoriales:

- [Unity Platformer Tutorial - Part 4 - Enemy Movement - Devin Curry](https://www.youtube.com/watch?v=LPNSh9mwT4w&t=1254s&ab_channel=DevinCurry)
- [Unity Platformer Tutorial - Part 5a - Combat Player Health - Devin Curry](https://www.youtube.com/watch?v=kRsBoo2p6uw&t=1120s&ab_channel=DevinCurry)
- [Unity Platformer Tutorial Part 5b Combat Enemy Death - Devin Curry](https://www.youtube.com/watch?v=e8DXLqGPosI&t=350s&ab_channel=DevinCurry)
- [Dust Effect when Running & Jumping in Unity (Particle Effect) - Press Start](https://www.youtube.com/watch?v=1CXVbCbqKyg&ab_channel=PressStart)
- [COMO DISPARAR EN UNITY 2D - pt 1 - Tutoriales Dingo](https://www.youtube.com/watch?v=MQnKjIWy2qI&t=627s&ab_channel=TutorialesDingo)
