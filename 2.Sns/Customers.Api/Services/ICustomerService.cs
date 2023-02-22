using Customers.Api.Domain;

namespace Customers.Api.Services;

public interface ICustomerService
{
    Task<bool> CreateAsync(Customer customer, CancellationToken cancellationToken = default);

    Task<Customer?> GetAsync(Guid id);

    Task<IEnumerable<Customer>> GetAllAsync();

    Task<bool> UpdateAsync(Customer customer, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
