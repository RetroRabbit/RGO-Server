using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces
{
    /// <summary>
    /// Defines a set of methods for managing client projects.
    /// </summary>
    public interface IClientProjectService
    {
        /// <summary>
        /// Retrieves all client projects from the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="ClientProjectsDto"/>.</returns>
        Task<List<ClientProjectsDto>> GetAllClientProject();

        /// <summary>
        /// Retrieves a single client project by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the client project to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="ClientProjectsDto"/> corresponding to the specified id,
        /// or null if no project with such an id exists.
        /// </returns>
        Task<ClientProjectsDto?> GetClientProject(int id);

        /// <summary>
        /// Creates a new client project in the database.
        /// </summary>
        /// <param name="clientProject">The <see cref="ClientProject"/> object to create.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="ClientProjectsDto"/> that was created.
        /// </returns>
        Task<ClientProjectsDto> CreateClientProject(ClientProjectsDto clientProjectsDto);

        /// <summary>
        /// Updates an existing client project in the database.
        /// </summary>
        /// <param name="clientProject">The <see cref="ClientProject"/> object to update.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the updated <see cref="ClientProjectsDto"/>.
        /// </returns>
        Task<ClientProjectsDto> UpdateClientProject(ClientProjectsDto clientProjectsDto);

        /// <summary>
        /// Deletes a client project from the database based on its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the client project to be deleted.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="ClientProjectsDto"/> that was deleted.
        /// </returns>
        Task<ClientProjectsDto> DeleteClientProject(int id);
    }
}
