# MailsGateway
# Paquetes instalado En el 
dotnet add MailGateway package Microsoft.EntityFrameworkCore.Design

# uso de migraciones 
 dotnet ef migrations add Usuarios --project Infrastructure --startup-project MailGateway
 ef migrations 
 
 dotnet ef database update --project Infrastructure --startup-project MailGateway


 Claro, aquí tienes los comandos más usados de **Entity Framework Core** para manejar migraciones desde la consola (CLI):

-- Actualizar migracion
dotnet ef migrations add UsuariosValidateEmailLenht   --project Infrastructure  --startup-project MailGateway
-- Actualizar la base de datos 

 dotnet ef database update --project Infrastructure --startup-project MailGateway
---
dotnet ef migrations add UsuariosNotificationInboxMessageSendMessage   --project Infrastructure  --startup-project MailGateway
## Comandos básicos para migraciones EF Core

### 1. Crear una nueva migración

```bash
dotnet ef migrations add NombreDeLaMigracion
```

* Crea una nueva migración con el nombre que le pongas, detectando los cambios en el modelo.
* Ejemplo:

```bash
dotnet ef migrations add AgregarCampoPerfilUsuario
```

---

### 2. Aplicar migraciones pendientes a la base de datos

```bash
dotnet ef database update
```

* Aplica todas las migraciones pendientes a la base de datos configurada en tu `DbContext`.
* Equivalente a `context.Database.Migrate()` pero desde consola.

---

### 3. Eliminar la última migración (sin aplicar)

```bash
dotnet ef migrations remove
```

* Elimina la última migración creada si aún no se ha aplicado a la base de datos.
* Útil si creaste una migración por error o con cambios no deseados.

---

### 4. Listar migraciones aplicadas y pendientes

```bash
dotnet ef migrations list
```

* Muestra la lista de migraciones que existen en el proyecto, indicando cuáles ya están aplicadas y cuáles no.

---

### 5. Opcional: Especificar proyecto o startup

Si tu solución tiene varios proyectos, puedes especificar cuál usar:

```bash
dotnet ef migrations add NombreDeLaMigracion --project NombreProyecto --startup-project NombreProyectoInicio
```

---

## Resumen rápido

| Comando                       | Uso                                    |
| ----------------------------- | -------------------------------------- |
| `dotnet ef migrations add X`  | Crear migración nueva                  |
| `dotnet ef database update`   | Aplicar migraciones a la base de datos |
| `dotnet ef migrations remove` | Eliminar última migración no aplicada  |
| `dotnet ef migrations list`   | Mostrar migraciones existentes         |

---

Si quieres, puedo ayudarte con ejemplos paso a paso para crear y aplicar migraciones en tu proyecto. ¿Quieres?

## Borro la base de datos
dotnet ef database drop --project Infrastructure --startup-project MailGateway

 
 ## Aplica la migración:

bash
Copiar
Editar

dotnet ef database update --project Infrastructure --startup-project MailGateway

## Comando seed poblar datos en la base de datos por defecto
dotnet run --project MailGateway -- seed

