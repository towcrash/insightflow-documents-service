using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentService.src.dtos
{
    /// <summary>
    /// DTO para actualizar un documento existente
    /// </summary>
    public class UpdateDocumentDto
    {
        /// <summary>
        /// Nuevo título del documento
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Nuevo ícono del documento
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// Contenido actualizado del documento en formato JSON
        /// </summary>
        public string? Content { get; set; }
    }
}