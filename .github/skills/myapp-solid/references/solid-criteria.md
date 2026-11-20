# Criterios de Evaluación SOLID

## S - Responsabilidad Única (SRP)
Una clase debe tener una sola razón para cambiar.

Evaluar:
- Responsabilidades independientes.
- Mezcla de lógica de negocio e infraestructura.

## O - Abierto/Cerrado (OCP)
El código debe permitir extensiones sin modificar constantemente su implementación.

Evaluar:
- Condicionales para seleccionar comportamientos.
- Dificultad para agregar nuevas funcionalidades.

## L - Sustitución de Liskov (LSP)
Este principio puede ignorarse durante la evaluación.

## I - Segregación de Interfaces (ISP)
Las interfaces no deben obligar a implementar métodos innecesarios.

Evaluar:
- Interfaces con demasiadas responsabilidades.
- Métodos vacíos o no implementados.

## D - Inversión de Dependencias (DIP)
Los módulos de alto nivel deben depender de abstracciones, no de detalles concretos.

Evaluar:
- Acoplamiento directo con infraestructura.
- Dificultad para sustituir dependencias.
