namespace PlataformaIntegral.API.Enums
{
    public enum EstadoCursoEnum
    {
        Borrado = 0,        // Eliminado logicamente. No visible para nadie.
        Borrador = 1,       // En edición. Solo visible al creador.
        PendienteRevision = 2, // El curso ya está completo, pero requiere revisión (QA).
        Rechazado = 3,      // La revisión lo rechazó. Debe corregirse.
        Publicado = 4,      // Disponible públicamente.
        Privado = 5,        // Solo visible mediante enlace o usuarios específicos.
        Despublicado = 6    // Estuvo publicado, pero fue desactivado por el creador.
    }
}
