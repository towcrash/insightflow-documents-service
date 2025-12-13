using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentService.src.dtos;
using DocumentService.src.Interface;
using DocumentService.src.mappers;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.src.Controller
{
    /// <summary>
    /// Controlador REST para la gestión de documentos
    /// Expone endpoints HTTP para operaciones CRUD
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<DocumentsController> _logger;

        /// <summary>
        /// Constructor del controlador
        /// </summary>
        public DocumentsController(
            IDocumentRepository documentRepository,
            ILogger<DocumentsController> logger)
        {
            _documentRepository = documentRepository;
            _logger = logger;
        }

        /// <summary>
        /// Crea un nuevo documento
        /// </summary>
        /// <param name="createDocumentDto">Datos del documento a crear</param>
        /// <returns>El documento creado</returns>
        /// <response code="201">Documento creado exitosamente</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPost]
        [ProducesResponseType(typeof(VisualizeDocumentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentDto createDocumentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var document = await _documentRepository.CreateDocument(createDocumentDto);
                var documentDto = document.ToVisualizeDocumentDto();

                return CreatedAtAction(
                    nameof(GetDocument),
                    new { id = document.Id },
                    documentDto
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error in CreateDocument");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating document");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Obtiene un documento por su ID
        /// </summary>
        /// <param name="id">ID del documento</param>
        /// <returns>El documento solicitado</returns>
        /// <response code="200">Documento encontrado</response>
        /// <response code="404">Documento no encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VisualizeDocumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDocument(string id)
        {
            try
            {
                var document = await _documentRepository.GetDocument(id);

                if (document == null)
                {
                    return NotFound(new { error = "Document not found" });
                }

                var documentDto = document.ToVisualizeDocumentDto();
                return Ok(documentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting document {DocumentId}", id);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Obtiene todos los documentos activos
        /// </summary>
        /// <returns>Lista de todos los documentos</returns>
        /// <response code="200">Lista de documentos</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VisualizeDocumentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDocuments()
        {
            try
            {
                var documents = await _documentRepository.GetAllDocuments();
                var documentsDto = documents.ToDtoEnumerable();
                return Ok(documentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all documents");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Obtiene todos los documentos de un workspace específico
        /// </summary>
        /// <param name="workspaceId">ID del workspace</param>
        /// <returns>Lista de documentos del workspace</returns>
        /// <response code="200">Lista de documentos del workspace</response>
        [HttpGet("workspace/{workspaceId}")]
        [ProducesResponseType(typeof(IEnumerable<VisualizeDocumentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocumentsByWorkspace(string workspaceId)
        {
            try
            {
                var documents = await _documentRepository.GetDocumentsByWorkspace(workspaceId);
                var documentsDto = documents.ToDtoEnumerable();
                return Ok(documentsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting documents for workspace {WorkspaceId}", workspaceId);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Actualiza un documento existente
        /// </summary>
        /// <param name="id">ID del documento a actualizar</param>
        /// <param name="updateDocumentDto">Datos a actualizar</param>
        /// <returns>El documento actualizado</returns>
        /// <response code="200">Documento actualizado exitosamente</response>
        /// <response code="404">Documento no encontrado</response>
        /// <response code="400">Datos inválidos</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(VisualizeDocumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDocument(
            string id,
            [FromBody] UpdateDocumentDto updateDocumentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var document = await _documentRepository.UpdateDocument(id, updateDocumentDto);

                if (document == null)
                {
                    return NotFound(new { error = "Document not found" });
                }

                var documentDto = document.ToVisualizeDocumentDto();
                return Ok(documentDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error in UpdateDocument");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating document {DocumentId}", id);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Elimina un documento (soft delete)
        /// </summary>
        /// <param name="id">ID del documento a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">Documento eliminado exitosamente</response>
        /// <response code="404">Documento no encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            try
            {
                var deleted = await _documentRepository.DeleteDocument(id);

                if (!deleted)
                {
                    return NotFound(new { error = "Document not found" });
                }

                return Ok(new { message = "Document deleted successfully", id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document {DocumentId}", id);
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}