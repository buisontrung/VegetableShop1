using Authentication.IRepository;
using Authentication.Model;
using Authentication.ModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AddressController : ControllerBase
	{
		private readonly IAddressRepository _addressRepository;

		public AddressController(IAddressRepository addressRepository)
		{
			_addressRepository = addressRepository;
		}

		[HttpGet("getall")]
		public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAllAddresses()
		{
			var addresses = await _addressRepository.GetAllAddressesAsync();
			return Ok(addresses);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<AddressDTO>> GetAddressById(int id)
		{
			var address = await _addressRepository.GetAddressByIdAsync(id);
			if (address == null) return NotFound();
			return Ok(address);
		}

		[HttpGet("user/{userId}")]
		public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAddressesByUserId(string userId)
		{
			var addresses = await _addressRepository.GetAddressesByUserIdAsync(userId);
			return Ok(addresses);
		}
		[HttpGet("userprimary/{userId}")]
		public async Task<ActionResult<AddressDTO>> GetAddressesPrimaryByUserId(string userId)
		{
			var addresses = await _addressRepository.PrimaryAddressUserAsync(userId);
			return Ok(addresses);
		}
		[HttpPut("updateprimary/userId={userId}&Id={Id}")]
		public async Task<ActionResult<AddressDTO>> GetAddressesPrimaryByUserId(string userId,int Id)
		{
			var addresses = await _addressRepository.UpdatePrimaryAddressUserAsync(userId,Id);
			return Ok(addresses);
		}

		[HttpPost("add")]
		public async Task<ActionResult<AddressDTO?>> AddAddress(AddressDTO address)
		{
			var address1 = await _addressRepository.AddAddressAsync(address);

			return address1;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAddress( AddressDTO address)
		{
			

			await _addressRepository.UpdateAddressAsync(address);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAddress(int id)
		{
			await _addressRepository.DeleteAddressAsync(id);

			return NoContent();
		}
	}
}
