---
name: myapp-solid
description: Evalúa componentes C# para detectar violaciones de los principios SOLID y recomendar mejoras de diseño.
---

# Evaluador de principios SOLID

## Objetivo
Analizar un componente de software e identificar posibles problemas relacionados con los principios SOLID.

## Información necesaria
- Nombre del componente a evaluar.

## Pasos
1. Leer el componente seleccionado buscándolo como una biblioteca de clases en la solución.
2. Identificar sus responsabilidades y dependencias.
3. Analizar el contexto necesario, incluyendo
   interfaces, clases base y consumidores.
4. Consultar `references/solid-criteria.md` y evaluar el componente según los criterios definidos.
5. Identificar violaciones respaldadas por código.
6. Proponer mejoras justificadas.
7. Generar un reporte de evaluación utilizando la plantilla `assets/report-template.md`. 

## Ejecución del script
1. Localizar `scripts/count-classes.cs` dentro del skill.
2. Obtener la ruta absoluta del script.
3. Ejecutarlo exclusivamente mediante la terminal,
   utilizando el SDK de .NET 10 o superior.
4. No abrir el archivo con ninguna aplicación.
5. Ejecutar el comando:

   dotnet run "<ruta-absoluta-del-script>" -- "<ruta-del-componente>"

6. Capturar la salida del comando.
7. Incluir el resultado en la sección "Archivos encontrados"
   del reporte.


## Reglas
- No modificar código nunca.
- Solo vas a evaluar, no modificar código nunca.
- No asumir que toda clase necesita interfaces.
- No recomendar patrones innecesarios.
- Priorizar simplicidad y mantenibilidad.
- Distinguir violaciones confirmadas de riesgos.
- Indicar cuando falta contexto para evaluar.
- Respetar la estructura de la plantilla del reporte.
- No modificar el archivo original de la plantilla.
- Mostrar el reporte en la respuesta, sin crear ni modificar archivos del proyecto.

## Formato de respuesta

Para cada hallazgo mostrar:

- Principio afectado.
- Componente y ubicación.
- Evidencia encontrada.
- Explicación del problema.
- Recomendación de mejora.
- Clasificación de problema: bajo, medio o alto.

Finalizar con un resumen de los principios evaluados.