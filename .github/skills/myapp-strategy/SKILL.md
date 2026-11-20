---
name: myapp-strategy
description: Añade tipos de tarea y estrategias al ejemplo .NET, conectándolos con la cola Channel y la aplicación de consola.
---

# myapp-strategy

Úsalo cuando se solicite agregar un tipo de tarea o una estrategia al proyecto Tasks.

## Información necesaria

- Tipo de tarea y comportamiento esperado.
- Datos de entrada y resultado que determinan el éxito.
- Destino y formato, si la estrategia crea o modifica recursos.
- Reglas de error y opciones que debe ofrecer la consola.

Si un dato esencial no está especificado y no puede deducirse del proyecto, pregunta antes de implementarlo. Inspecciona primero los contratos, el enum, `TaskProcessQueue` y el registro de dependencias.

## Pasos

1. Agrega el miembro al enum existente e implementa `ITaskStrategy`, asignando `Type` y encapsulando el comportamiento en `ExecuteAsync`.
2. Haz que `TaskProcessQueue` despache por `Type` usando solo el contrato de estrategia; conserva el `Channel` y el guardado existente.
3. Registra la estrategia en el punto de composición y agrega la selección de tipo en la consola cuando corresponda.
4. Valida entradas, comunica fallos explícitamente y ejecuta la compilación o pruebas pertinentes.

## Ejemplo de uso

Pide: «Usa `myapp-strategy` para agregar una estrategia `File` que guarde `TaskItem.Data` en `<TaskItem.Name>.txt` dentro del directorio de trabajo y permita elegirla al crear una tarea».
