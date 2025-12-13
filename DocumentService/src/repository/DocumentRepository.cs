using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentService.src.dtos;
using DocumentService.src.helper;
using DocumentService.src.Interface;
using DocumentsService.src.Model;

namespace DocumentService.src.repository
{
    /// <summary>
    /// Implementación del repositorio de documentos usando almacenamiento en memoria
    /// Los datos se almacenan en una lista estática que se reinicia al redesplegar el servicio
    /// </summary>
    public class DocumentRepository : IDocumentRepository
    {
        // Lista estática para almacenar documentos en memoria
        private static readonly List<Document> _documents = new();
        private static readonly object _lock = new();

        /// <summary>
        /// Constructor del repositorio
        /// Inicializa los datos de ejemplo si la lista está vacía
        /// </summary>
        public DocumentRepository()
        {
            lock (_lock)
            {
                if (_documents.Count == 0)
                {
                    InitializeSampleData();
                }
            }
        }

        /// <summary>
        /// Crea un nuevo documento en memoria
        /// </summary>
        /// <param name="createDocumentDto">DTO con la información del documento</param>
        /// <returns>El documento creado</returns>
        public Task<Document> CreateDocument(CreateDocumentDto createDocumentDto)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(createDocumentDto.WorkspaceId))
            {
                throw new ArgumentException("Workspace ID is required");
            }

            if (string.IsNullOrWhiteSpace(createDocumentDto.Title))
            {
                throw new ArgumentException("Title is required");
            }

            if (string.IsNullOrWhiteSpace(createDocumentDto.CreatedByUserId))
            {
                throw new ArgumentException("Created by user ID is required");
            }

            // Validar y formatear el contenido JSON
            string content = createDocumentDto.Content;
            if (string.IsNullOrWhiteSpace(content))
            {
                content = JsonValidator.GetEmptyDocumentContent();
            }
            else if (!JsonValidator.IsValidJson(content))
            {
                throw new ArgumentException("Invalid JSON content");
            }
            else
            {
                content = JsonValidator.FormatJson(content);
            }

            // Generar un ID único (UUID V4)
            string newId = Guid.NewGuid().ToString();

            // Crear el documento
            var document = new Document
            {
                Id = newId,
                WorkspaceId = createDocumentDto.WorkspaceId,
                Title = createDocumentDto.Title,
                Icon = createDocumentDto.Icon,
                Content = content,
                CreatedByUserId = createDocumentDto.CreatedByUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // Agregar a la lista en memoria
            lock (_lock)
            {
                _documents.Add(document);
            }

            return Task.FromResult(document);
        }

        /// <summary>
        /// Obtiene un documento por su ID
        /// </summary>
        /// <param name="id">ID del documento</param>
        /// <returns>El documento o null si no existe</returns>
        public Task<Document?> GetDocument(string id)
        {
            lock (_lock)
            {
                var document = _documents.FirstOrDefault(d => d.Id == id && d.IsActive);
                return Task.FromResult(document);
            }
        }

        /// <summary>
        /// Obtiene todos los documentos activos
        /// </summary>
        /// <returns>Lista de todos los documentos activos</returns>
        public Task<List<Document>> GetAllDocuments()
        {
            lock (_lock)
            {
                var documents = _documents
                    .OrderByDescending(d => d.UpdatedAt)
                    .ToList();
                return Task.FromResult(documents);
            }
        }

        /// <summary>
        /// Obtiene todos los documentos de un espacio de trabajo
        /// </summary>
        /// <param name="workspaceId">ID del espacio de trabajo</param>
        /// <returns>Lista de documentos activos del workspace</returns>
        public Task<List<Document>> GetDocumentsByWorkspace(string workspaceId)
        {
            lock (_lock)
            {
                var documents = _documents
                    .Where(d => d.WorkspaceId == workspaceId && d.IsActive)
                    .OrderByDescending(d => d.UpdatedAt)
                    .ToList();
                return Task.FromResult(documents);
            }
        }

        /// <summary>
        /// Actualiza un documento existente
        /// </summary>
        /// <param name="id">ID del documento</param>
        /// <param name="updateDocumentDto">DTO con los datos a actualizar</param>
        /// <returns>El documento actualizado o null</returns>
        public Task<Document?> UpdateDocument(string id, UpdateDocumentDto updateDocumentDto)
        {
            lock (_lock)
            {
                var document = _documents.FirstOrDefault(d => d.Id == id && d.IsActive);

                if (document == null)
                {
                    return Task.FromResult<Document?>(null);
                }

                // Actualizar título si se proporciona
                if (!string.IsNullOrWhiteSpace(updateDocumentDto.Title))
                {
                    document.Title = updateDocumentDto.Title;
                }

                // Actualizar ícono si se proporciona
                if (!string.IsNullOrWhiteSpace(updateDocumentDto.Icon))
                {
                    document.Icon = updateDocumentDto.Icon;
                }

                // Actualizar contenido si se proporciona
                if (!string.IsNullOrWhiteSpace(updateDocumentDto.Content))
                {
                    if (!JsonValidator.IsValidJson(updateDocumentDto.Content))
                    {
                        throw new ArgumentException("Invalid JSON content");
                    }
                    document.Content = JsonValidator.FormatJson(updateDocumentDto.Content);
                }

                // Actualizar timestamp
                document.UpdatedAt = DateTime.UtcNow;

                return Task.FromResult<Document?>(document);
            }
        }

        /// <summary>
        /// Elimina lógicamente un documento (soft delete)
        /// </summary>
        /// <param name="id">ID del documento</param>
        /// <returns>True si se eliminó correctamente</returns>
        public Task<bool> DeleteDocument(string id)
        {
            lock (_lock)
            {
                var document = _documents.FirstOrDefault(d => d.Id == id && d.IsActive);

                if (document == null)
                {
                    return Task.FromResult(false);
                }

                // Soft delete
                document.IsActive = false;
                document.DeletedAt = DateTime.UtcNow;

                return Task.FromResult(true);
            }
        }

        /// <summary>
        /// Inicializa datos de ejemplo en memoria
        /// </summary>
        private void InitializeSampleData()
        {
            // IDs de ejemplo para workspaces y usuarios
            var workspaceId1 = "workspace-example-1";
            var workspaceId2 = "workspace-example-2";
            var userId1 = "user-example-1";
            var userId2 = "user-example-2";

            var sampleDocuments = new List<Document>
            {
                new Document
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkspaceId = workspaceId1,
                    Title = "Documento de Bienvenida",
                    Icon = "👋",
                    Content = JsonValidator.GetEmptyDocumentContent(),
                    CreatedByUserId = userId1,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10),
                    IsActive = true
                },
                new Document
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkspaceId = workspaceId1,
                    Title = "Notas de Reunión",
                    Icon = "📝",
                    Content = @"{""blocks"":[{""type"":""paragraph"",""content"":""Notas de la reunión del equipo""}]}",
                    CreatedByUserId = userId1,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2),
                    IsActive = true
                },
                new Document
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkspaceId = workspaceId1,
                    Title = "Ideas del Proyecto",
                    Icon = "💡",
                    Content = @"{""blocks"":[{""type"":""heading"",""content"":""Brainstorming""},{""type"":""paragraph"",""content"":""Lista de ideas...""}]}",
                    CreatedByUserId = userId2,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1),
                    IsActive = true
                },
                new Document
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkspaceId = workspaceId2,
                    Title = "Documentación Técnica",
                    Icon = "📚",
                    Content = JsonValidator.GetEmptyDocumentContent(),
                    CreatedByUserId = userId1,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-7),
                    IsActive = true
                },
                new Document
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkspaceId = workspaceId2,
                    Title = "Plan de Marketing",
                    Icon = "📊",
                    Content = @"{""blocks"":[{""type"":""heading"",""content"":""Estrategia Q1""},{""type"":""paragraph"",""content"":""Objetivos y métricas...""}]}",
                    CreatedByUserId = userId2,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            };

            _documents.AddRange(sampleDocuments);
        }
    }
}