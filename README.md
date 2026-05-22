# Helix Jump Clone

Un clon optimizado y fiel del clásico juego arcade móvil **Helix Jump**, desarrollado en **Unity** con **C#**. El objetivo es guiar una pelota en su descenso a través de una torre de plataformas giratorias, esquivando obstáculos y zonas de peligro generadas dinámicamente.

##  Características del Proyecto

* **Generación Procedimental de Niveles:** Los escenarios no están pre-construidos; se generan en tiempo real al iniciar el nivel usando configuraciones basadas en `ScriptableObjects`.
* **Mecánica de Combo (Super Speed):** Si la pelota pasa de largo por 3 o más huecos sin tocar ninguna plataforma (`perfectPass`), entra en modo "fuego" (súper velocidad) y destruye la primera plataforma que toca.
* **Control Fluido (New Input System):** Implementado con el nuevo sistema de inputs de Unity, asegurando un comportamiento responsivo tanto con el mouse en PC como con gestos táctiles en móviles.
* **Persistencia de Datos:** Guardado local del puntaje máximo (`Best Score`) mediante `PlayerPrefs`.
* **Efectos Visuales Dinámicos (VFX):** Sistema de *splashes* de pintura que se instancian al colisionar y se acoplan como hijos de la plataforma para girar con ella de forma realista.
* **Zonas de Muerte Aleatorias:** Cada nivel calcula dinámicamente qué sectores se convierten en obstáculos mortales (`DeathPart`) según los parámetros de dificultad.

---

##  Stack Técnico

* **Motor:** Unity (2022.3 LTS o superior).
* **Lenguaje:** C# (.NET).
* **Inputs:** Unity New Input System.
* **UI:** TextMeshPro para el renderizado de fuentes.

---

##  Arquitectura y Estructura del Código

El proyecto está estructurado de forma modular dividiendo las responsabilidades:

### Core & UI
* **`GameManager.cs`**: Controlador central del loop de juego (Singleton). Maneja los estados de pausa, reinicios de nivel, transiciones de escenas y el flujo del puntaje.
* **`UImanager.cs`**: Actualiza la interfaz de usuario en tiempo real (score actual, récord y la barra de progreso calculando la distancia vertical restante hacia la meta).

### Físicas y Gameplay
* **`BallController.cs`**: Controla el comportamiento físico de la bola (`Rigidbody`), los impulsos de salto, el detector de combos para la súper velocidad y la instanciación de los *splashes* de pintura.
* **`CamController.cs`**: Sigue a la pelota de manera suave únicamente en el eje Y mediante un *offset* calculado en el `Start`.

### Generación y Niveles
* **`HelixController.cs`**: El motor del nivel. Lee el *drag* del usuario para rotar el eje central. Además, se encarga de spawnear los prefabs de las plataformas, desactivar piezas aleatorias para crear los huecos y setear las zonas de muerte.
* **`Stage.cs`**: Contenedor de datos (`ScriptableObject`) que define las paletas de colores del nivel y una lista con la configuración de dificultad por pisos (`partCount` y `deathPartCount`).
* **`DeathPart.cs`**: Script dinámico que se le inyecta a las piezas peligrosas para cambiar su comportamiento y pintarlas de rojo.
* **`PassScorePoint.cs`**: Trigger invisible en los huecos que detecta cuándo el jugador pasa limpiamente para sumar puntos y activar el combo.
* **`GoalController.cs`**: Trigger en la base del nivel que le avisa al `GameManager` que el escenario fue superado.

---

##  Un vistazo al código

### Lógica de Súper Velocidad (BallController)
Si el jugador acumula suficientes caídas limpias, la bola gana una fuerza descendente extra y la propiedad de destruir la plataforma en el siguiente impacto:
```csharp
if (isSuperSpeedActive && !collision.transform.GetComponent<GoalController>()) {
    Destroy(collision.transform.parent.gameObject, 0.2f);
}
