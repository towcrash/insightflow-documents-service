using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentService.src.dtos;
using DocumentsService.src.Model;

namespace DocumentService.src.mappers
{
    /// <summary>
    /// Mapper para convertir entre entidades Document y DTOs
    /// </summary>
    public static class DocumentMapper
    {
        /// <summary>
        /// Convierte un Document en un VisualizeDocumentDto
        /// </summary>
        /// <param name="document">Entidad Document</param>
        /// <returns>VisualizeDocumentDto con la información del documento</returns>
        public static VisualizeDocumentDto ToVisualizeDocumentDto(this Document document)
        {
            return new VisualizeDocumentDto
            {
                Id = document.Id,
                WorkspaceId = document.WorkspaceId,
                Title = document.Title,
                Icon = document.Icon,
                Content = document.Content,
                CreatedByUserId = document.CreatedByUserId,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt,
                IsActive = document.IsActive
            };
        }

        /// <summary>
        /// Convierte una colección de Documents en una colección de VisualizeDocumentDtos
        /// </summary>
        /// <param name="documents">Colección de Documents</param>
        /// <returns>Colección de VisualizeDocumentDtos</returns>
        public static IEnumerable<VisualizeDocumentDto> ToDtoEnumerable(this IEnumerable<Document> documents)
        {
            return documents.Select(doc => doc.ToVisualizeDocumentDto());
        }
    }
}