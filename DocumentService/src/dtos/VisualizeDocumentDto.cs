using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentService.src.dtos
{
    /// <summary>
    /// DTO para visualizar la información de un documento
    /// </summary>
    public class VisualizeDocumentDto
    {
        /// <summary>
        /// ID del documento
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// ID del espacio de trabajo
        /// </summary>
        public required string WorkspaceId { get; set; }

        /// <summary>
        /// Título del documento
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Ícono del documento
        /// </summary>
        public required string Icon { get; set; }

        /// <summary>
        /// Contenido del documento en formato JSON
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// ID del usuario creador
        /// </summary>
        public required string CreatedByUserId { get; set; }

        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Estado del documento
        /// </summary>
        public bool IsActive { get; set; }
    }
}