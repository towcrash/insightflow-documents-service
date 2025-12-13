using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentService.src.dtos;
using DocumentsService.src.Model;

namespace DocumentService.src.Interface
{
    /// <summary>
    /// Interfaz del repositorio de documentos
    /// Define las operaciones CRUD para gestionar documentos en memoria
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Crea un nuevo documento
        /// </summary>
        /// <param name="createDocumentDto">DTO con la información del documento</param>
        /// <returns>El documento creado</returns>
        Task<Document> CreateDocument(CreateDocumentDto createDocumentDto);

        /// <summary>
        /// Obtiene un documento por su ID
        /// </summary>
        /// <param name="id">ID del documento</param>
        /// <returns>El documento encontrado o null</returns>
        Task<Document?> GetDocument(string id);

        /// <summary>
        /// Obtiene todos los documentos activos
        /// </summary>
        /// <returns>Lista de todos los documentos activos</returns>
        Task<List<Document>> GetAllDocuments();

        /// <summary>
        /// Obtiene todos los documentos de un espacio de trabajo
        /// </summary>
        /// <param name="workspaceId">ID del espacio de trabajo</param>
        /// <returns>Lista de documentos del workspace</returns>
        Task<List<Document>> GetDocumentsByWorkspace(string workspaceId);

        /// <summary>
        /// Actualiza un documento existente
        /// </summary>
        /// <param name="id">ID del documento a actualizar</param>
        /// <param name="updateDocumentDto">DTO con los datos a actualizar</param>
        /// <returns>El documento actualizado o null si no existe</returns>
        Task<Document?> UpdateDocument(string id, UpdateDocumentDto updateDocumentDto);

        /// <summary>
        /// Elimina lógicamente un documento (soft delete)
        /// </summary>
        /// <param name="id">ID del documento a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> DeleteDocument(string id);
    }
}