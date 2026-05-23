# StockMeal

StockMeal es una aplicación de escritorio WPF desarrollada en .NET 8 para gestionar el stock, recetas, compras, ventas, proveedores, clientes, empleados y producción de un negocio de alimentos.

## Descripción

Esta aplicación ofrece una interfaz moderna con MahApps.Metro y Material Design, conectada a una base de datos MySQL mediante Entity Framework Core.

### Funcionalidades principales

- Gestión de productos y recetas
- Control de ingredientes
- Administración de proveedores y clientes
- Registro de compras y ventas
- Manejo de usuarios, roles y permisos
- Control de caja y cierres de caja
- Planificación y registro de producción
- Login de usuarios con autenticación básica

## Arquitectura

El proyecto está dividido en dos capas principales:

- `Frontend/` contiene la UI en WPF, diálogos y controles de usuario.
- `Backend/` contiene la lógica de datos, repositorios, modelos y el contexto de EF Core.

Además, la aplicación utiliza inyección de dependencias para resolver servicios, repositorios y vistas desde `App.xaml.cs`.

## Requisitos

- Windows 10/11
- .NET 8 SDK
- Visual Studio 2022/2023 con soporte para WPF o Visual Studio Code con C# y soporte WPF
- MySQL Server

## Dependencias principales

- `MahApps.Metro` 2.4.11
- `MaterialDesignThemes` 5.3.0
- `Microsoft.EntityFrameworkCore` 8.0.20
- `MySql.EntityFrameworkCore` 8.0.11
- `NLog` 6.0.6

## Configuración de base de datos

El proyecto utiliza MySQL con la siguiente cadena de conexión por defecto, definida en `Backend/DBContext/StockMealContext.cs`:

```csharp
server=127.0.0.1;port=3306;database=gestion_stock;user=root;password=mysql;
```

Asegúrate de que MySQL esté instalado y activo. Crea la base de datos `gestion_stock` y, si es necesario, ajusta el usuario y la contraseña en el archivo `StockMealContext.cs`.

> Nota: Esta cadena de conexión está en código fuente. Para producción, considera moverla a un archivo de configuración o variable de entorno.

## Instalación y ejecución

1. Abre una terminal en la carpeta del proyecto:

```powershell
cd c:\Users\Mario\source\repos\StockMeal-WPF
```

2. Restaurar dependencias:

```powershell
dotnet restore
```

3. Compilar el proyecto:

```powershell
dotnet build
```

4. Ejecutar la aplicación:

```powershell
dotnet run --project StockMeal.csproj
```

También puedes abrir `StockMeal.slnx` en Visual Studio y ejecutar el proyecto desde el IDE.

## Estructura del proyecto

- `App.xaml` / `App.xaml.cs` - configuración de inicio y DI
- `MainWindow.xaml` / `MainWindow.xaml.cs` - ventana principal
- `Backend/DBContext` - contexto de EF Core y configuración de la base de datos
- `Backend/Modelos` - entidades del dominio
- `Backend/Repositorios` - repositorios de acceso a datos
- `Frontend/UserControls` - controles de usuario
- `Frontend/Dialogos` - ventanas emergentes y diálogos

## Uso

1. Inicia la aplicación.
2. Inicia sesión con un usuario existente de la base de datos.
3. Navega por el menú para administrar ingredientes, platos, proveedores, clientes, compras, ventas y producción.

## Ejemplos de uso

### Agregar un nuevo ingrediente

1. Ve a la sección de ingredientes.
2. Selecciona "Añadir ingrediente".
3. Completa el nombre, cantidad y precio del ingrediente.
4. Guarda para que el ingrediente se almacene en la base de datos.

### Crear una receta

1. Ve a la sección de recetas.
2. Selecciona "Añadir receta".
3. Asigna nombre al plato y agrega los ingredientes requeridos.
4. Guarda para generar la receta en el sistema.

### Registrar una compra

1. Ve a la sección de compras.
2. Selecciona "Añadir compra".
3. Completa proveedor, productos y cantidades.
4. Confirma para registrar el movimiento y actualizar el stock.

## Configuración avanzada

### Cambiar la conexión de base de datos

Abre `Backend/DBContext/StockMealContext.cs` y modifica la cadena de conexión en el método `OnConfiguring`.

Ejemplo:

```csharp
optionsBuilder.UseLazyLoadingProxies()
    .UseMySQL("server=localhost;port=3306;database=gestion_stock;user=tu_usuario;password=tu_contraseña;");
```

### Crear la base de datos MySQL

Ejecuta en MySQL:

```sql
CREATE DATABASE gestion_stock;
```

### Configurar el nivel de logging

La aplicación carga `Microsoft.Extensions.Logging.Console` desde `App.xaml.cs`. Si deseas ajustar el nivel, puedes cambiar la configuración de logging en `ConfigureServices`.

### Ejecutar desde Visual Studio

1. Abre `StockMeal.slnx`.
2. Establece `StockMeal` como proyecto de inicio.
3. Ejecuta con F5 o Ctrl+F5.

## Contribuciones

Si quieres contribuir:

1. Haz un fork del repositorio.
2. Crea una rama con tu mejora.
3. Abre un pull request describiendo los cambios.

## Notas adicionales

- El login actual compara la contraseña en texto plano en `EmpleadoRepository`.
- Si deseas mejorar la seguridad, implementa hashing de contraseñas y almacenamiento seguro.
- Revisa `Backend/DBContext/StockMealContext.cs` para adaptar la base de datos a tu entorno.
