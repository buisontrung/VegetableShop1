using Authentication.Data;
using Authentication.IRepository;
using Authentication.Model;
using Authentication.ModelDto;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Repository
{
	public class AddressRepository : IAddressRepository
	{
		private readonly ApplicationDbContext _context;

		public AddressRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		// Lấy tất cả các địa chỉ và trả về AddressDTO
		public async Task<IEnumerable<AddressDTO?>> GetAllAddressesAsync()
		{
			return await _context.Addresses
				.Select(address => new AddressDTO
				{
					Id = address.Id,
					UserNameAddress = address.UserNameAddress,
					PhoneNumberAddress = address.PhoneNumberAddress,
					City = address.City,
					District = address.District,
					WardsCommunes = address.WardsCommunes,
					AddressName = address.AddressName,
					IsPrimary = address.IsPrimary,
					UserId = address.UserId,
				})
				.ToListAsync();
		}

		// Lấy địa chỉ theo ID và trả về AddressDTO
		public async Task<AddressDTO?> GetAddressByIdAsync(int id)
		{
			var address = await _context.Addresses
				.Where(a => a.Id == id)
				.Select(a => new AddressDTO
				{
					Id = a.Id,
					UserNameAddress = a.UserNameAddress,
					PhoneNumberAddress = a.PhoneNumberAddress,
					City = a.City,
					District = a.District,
					WardsCommunes = a.WardsCommunes,
					AddressName = a.AddressName,
					IsPrimary = a.IsPrimary
				})
				.FirstOrDefaultAsync();

			return address;
		}

		// Lấy địa chỉ của người dùng theo UserId và trả về AddressDTO
		public async Task<IEnumerable<AddressDTO?>> GetAddressesByUserIdAsync(string userId)
		{
			return await _context.Addresses
				.Where(address => address.UserId == userId)
				.Select(address => new AddressDTO
				{
					Id = address.Id,
					UserNameAddress = address.UserNameAddress,
					PhoneNumberAddress = address.PhoneNumberAddress,
					City = address.City,
					District = address.District,
					WardsCommunes = address.WardsCommunes,
					AddressName = address.AddressName,
					IsPrimary = address.IsPrimary
				})
				.ToListAsync();
		}

		// Thêm một địa chỉ mới
		public async Task<AddressDTO?> AddAddressAsync(AddressDTO address)
		{
			// Ánh xạ từ AddressDTO sang Address (entity)
			
			var addressModel = new Address
			{
				UserNameAddress = address.UserNameAddress,
				PhoneNumberAddress = address.PhoneNumberAddress,
				City = address.City,
				District = address.District,
				WardsCommunes = address.WardsCommunes,
				AddressName = address.AddressName,
				UserId = address.UserId, // UserId từ DTO
				IsPrimary = address.IsPrimary
			};
			var currentPrimaryAddress = await _context.Addresses
			.FirstOrDefaultAsync(a => a.UserId == address.UserId && a.IsPrimary == true);
			if (currentPrimaryAddress != null && address.IsPrimary==true)
			{
				currentPrimaryAddress.IsPrimary = false;
				_context.Addresses.Update(currentPrimaryAddress);
				await _context.SaveChangesAsync();
			}
			_context.Addresses.Add(addressModel);
			await _context.SaveChangesAsync();
			address.Id = addressModel.Id;
			return address;
		}

		// Cập nhật địa chỉ
		public async Task UpdateAddressAsync(AddressDTO address)
		{
			var addressModel = new Address
			{
				UserNameAddress = address.UserNameAddress,
				PhoneNumberAddress = address.PhoneNumberAddress,
				City = address.City,
				District = address.District,
				WardsCommunes = address.WardsCommunes,
				AddressName = address.AddressName,
				UserId = address.UserId, // UserId từ DTO
				IsPrimary = address.IsPrimary
			};
			_context.Addresses.Update(addressModel);
			await _context.SaveChangesAsync();
		}

		// Xóa địa chỉ theo ID
		public async Task DeleteAddressAsync(int id)
		{
			var address = await GetAddressByIdAsync(id);
			if (address != null)
			{
				_context.Addresses.Remove(new Address { Id = id });
				await _context.SaveChangesAsync();
			}
		}
		public async Task<AddressDTO?> PrimaryAddressUserAsync(string userId)
		{
			var address = await _context.Addresses.Where(a => a.UserId == userId && a.IsPrimary).Select(a => new AddressDTO
			{
				Id = a.Id,
				UserNameAddress = a.UserNameAddress,
				PhoneNumberAddress = a.PhoneNumberAddress,
				City = a.City,
				District = a.District,
				WardsCommunes = a.WardsCommunes,
				AddressName = a.AddressName,
				IsPrimary = a.IsPrimary
			}).FirstOrDefaultAsync();
			
			return address;

		}
		public async Task<AddressDTO?> UpdatePrimaryAddressUserAsync(string userId,int Id)
		{
			var address = await _context.Addresses.Where(a => a.Id == Id).FirstOrDefaultAsync();
			if (address == null)
			{
				return null;
			}
			var currentPrimaryAddress = await _context.Addresses
			.FirstOrDefaultAsync(a => a.UserId == userId && a.IsPrimary == true);
			if (currentPrimaryAddress != null)
			{
				currentPrimaryAddress.IsPrimary = false;
				_context.Addresses.Update(currentPrimaryAddress);
			}

			address.IsPrimary = true;
			_context.Addresses.Update(address);
			await _context.SaveChangesAsync();
			return new AddressDTO
			{
				Id = address.Id,
				UserNameAddress = address.UserNameAddress,
				PhoneNumberAddress = address.PhoneNumberAddress,
				City = address.City,
				District = address.District,
				WardsCommunes = address.WardsCommunes,
				AddressName = address.AddressName,
				IsPrimary = address.IsPrimary
			};

		}


	}
}
