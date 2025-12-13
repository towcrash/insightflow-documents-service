using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentService.src.dtos
{
    /// <summary>
    /// DTO para la creación de un nuevo documento
    /// </summary>
    public class CreateDocumentDto
    {
        /// <summary>
        /// ID del espacio de trabajo
        /// </summary>
        [Required(ErrorMessage = "Workspace ID is required")]
        public required string WorkspaceId { get; set; }

        /// <summary>
        /// Título del documento
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public required string Title { get; set; }

        /// <summary>
        /// Ícono del documento
        /// </summary>
        public required string Icon { get; set; }

        /// <summary>
        /// Contenido inicial del documento en formato JSON
        /// </summary>
        public string Content { get; set; } = "{}";

        /// <summary>
        /// ID del usuario que crea el documento
        /// </summary>
        [Required(ErrorMessage = "Created by user ID is required")]
        public required string CreatedByUserId { get; set; }
    }
}