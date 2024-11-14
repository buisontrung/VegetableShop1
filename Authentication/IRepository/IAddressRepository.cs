using Authentication.Model;
using Authentication.ModelDto;
using System.Threading.Tasks;

namespace Authentication.IRepository
{
	public interface IAddressRepository
	{
		Task<IEnumerable<AddressDTO?>> GetAllAddressesAsync();
		Task<AddressDTO?> GetAddressByIdAsync(int id);
		Task<IEnumerable<AddressDTO?>> GetAddressesByUserIdAsync(string userId);
		Task<AddressDTO?> AddAddressAsync(AddressDTO address);
		Task UpdateAddressAsync(AddressDTO address);
		Task<AddressDTO?> PrimaryAddressUserAsync(string userId);
		Task<AddressDTO?> UpdatePrimaryAddressUserAsync(string userId, int Id);
		Task DeleteAddressAsync(int id);
		
	}
}
