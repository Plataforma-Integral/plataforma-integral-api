namespace PlataformaIntegral.API.Enums
{
    /// <summary>
    /// Representa el ciclo de vida de un curso dentro de la plataforma.
    /// </summary>
    public enum EstadoCursoEnum
    {
        Eliminado = 0,          // Eliminado lógicamente. No visible para nadie.
        Borrador = 1,           // En edición. Solo visible al creador.
        PendienteRevision = 2,  // Curso completo, requiere revisión/QA antes de publicarse.
        Rechazado = 3,          // La revisión lo rechazó. Debe corregirse.
        Publicado = 4,          // Disponible públicamente para todos los usuarios.
        Privado = 5,            // Visible solo mediante enlace o usuarios autorizados.
        Despublicado = 6        // Estuvo publicado, pero fue desactivado por el creador/admin.
    }
}
