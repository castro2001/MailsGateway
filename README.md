# MailsGateway
# Paquetes instalado En el 
dotnet add MailGateway package Microsoft.EntityFrameworkCore.Design

# uso de migraciones 
 dotnet ef migrations add NombreDeTuMigracion --project Infrastructure --startup-project MailGateway
 ef migrations remove