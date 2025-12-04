namespace PlataformaIntegral.API.Enums
{
    /// <summary>
    /// Tipos de recursos que puede contener un curso.
    /// </summary>
    public enum TipoRecursoEnum
    {
        Documento = 0,   // Archivos PDF, Word, etc.
        Video = 1,       // Contenido audiovisual
        Cuestionario = 2,// Evaluaciones interactivas
        Examen = 3,      // Pruebas formales
        Certificado = 4  // Certificación emitida
    }
}
