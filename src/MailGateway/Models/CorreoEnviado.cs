namespace MailGateway.Models
{
    public class CorreoEnviado
    {
        
            public int Id { get; set; }
            public string Para { get; set; }
            public string Asunto { get; set; }
            public string Contenido { get; set; }
            public string MessageId { get; set; }
            public DateTime FechaEnvio { get; set; }
            public bool FueRespondido { get; set; } = false;
        

    }
}
