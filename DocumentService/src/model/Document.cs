using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DocumentsService.src.Model
{
    /// <summary>
    /// Representa un documento dentro de un espacio de trabajo
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Identificador único del documento (UUID V4)
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Identificador del espacio de trabajo al que pertenece
        /// </summary>
        public required string WorkspaceId { get; set; }

        /// <summary>
        /// Título del documento
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Ícono del documento (emoji o URL)
        /// </summary>
        public required string Icon { get; set; }

        /// <summary>
        /// Contenido del documento en formato JSON
        /// </summary>
        public string Content { get; set; } = "{}";

        /// <summary>
        /// ID del usuario que creó el documento
        /// </summary>
        public required string CreatedByUserId { get; set; }

        /// <summary>
        /// Fecha y hora de creación
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha y hora de última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Indica si el documento está activo (no eliminado)
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Fecha de eliminación (soft delete)
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
