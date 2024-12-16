//using Microsoft.EntityFrameworkCore;
//using ProductAPI.Data;
//using ProductAPI.IRepository;
//using ProductAPI.Model; // Import namespace chứa các entity của bạn
//using ProductAPI.ModelDto;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ProductAPI.Repository
//{
//	public class SupplierRepository : ISupplierRepository
//	{
//		private readonly ApplicationDbContext _context; // Đổi DbContext thành tên cụ thể của bạn, ví dụ: ProductDbContext

//		public SupplierRepository(ApplicationDbContext context)
//		{
//			_context = context;
//		}

//		// Lấy tất cả Suppliers
//		public async Task<List<SupplierDTO>> GetAllSuppliersAsync()
//		{
//			return await _context.Suppliers
//				.Include(s => s.ProductInventorySuppliers) // Bao gồm các ProductInventorySupplierDTO
//				.Select(s => new SupplierDTO
//				{
//					SupplierId = s.SupplierId,
//					SupplierName = s.SupplierName,
//					ContactInfo = s.ContactInfo,
//					PhoneNumber = s.PhoneNumber,
//					Suppliers = s.ProductInventorySuppliers.Select(ps => new ProductInventorySupplierDTO
//					{
//						// Mapping các thuộc tính trong ProductInventorySupplierDTO
//						Id = ps.Id,
//						ProductVariantId = ps.ProductVariantId,
//						Quantity = ps.Quantity,
//						// Add other necessary fields if available
//					}).ToList()
//				})
//				.ToListAsync();
//		}

//		// Lấy Supplier theo Id
//		public async Task<SupplierDTO?> GetSupplierByIdAsync(int id)
//		{
//			return await _context.Suppliers
//				.Where(s => s.SupplierId == id)
//				.Include(s => s.ProductInventorySuppliers)
//				.Select(s => new SupplierDTO
//				{
//					SupplierId = s.SupplierId,
//					SupplierName = s.SupplierName,
//					ContactInfo = s.ContactInfo,
//					PhoneNumber = s.PhoneNumber,
//					Suppliers = s.ProductInventorySuppliers.Select(ps => new ProductInventorySupplierDTO
//					{
//						Id = ps.Id,
//						ProductVariantId = ps.ProductVariantId,
//						Quantity = ps.Quantity
//					}).ToList()
//				})
//				.FirstOrDefaultAsync();
//		}

//		// Tạo mới Supplier
//		public async Task<SupplierDTO> CreateSupplierAsync(SupplierDTO supplierDto)
//		{
//			var supplier = new Supplier
//			{
//				SupplierName = supplierDto.SupplierName,
//				ContactInfo = supplierDto.ContactInfo,
//				PhoneNumber = supplierDto.PhoneNumber
//			};

//			_context.Suppliers.Add(supplier);
//			await _context.SaveChangesAsync();

//			supplierDto.SupplierId = supplier.SupplierId;
//			return supplierDto;
//		}

//		// Cập nhật Supplier
//		public async Task<bool> UpdateSupplierAsync(SupplierDTO supplierDto)
//		{
//			var supplier = await _context.Set<Supplier>().FindAsync(supplierDto.SupplierId);

//			if (supplier == null)
//			{
//				return false;
//			}

//			supplier.SupplierName = supplierDto.SupplierName;
//			supplier.ContactInfo = supplierDto.ContactInfo;
//			supplier.PhoneNumber = supplierDto.PhoneNumber;

//			_context.Suppliers.Update(supplier);
//			await _context.SaveChangesAsync();
//			return true;
//		}

//		// Xóa Supplier
//		public async Task<bool> DeleteSupplierAsync(int id)
//		{
//			var supplier = await _context.Suppliers.FindAsync(id);

//			if (supplier == null)
//			{
//				return false;
//			}

//			_context.Suppliers.Remove(supplier);
//			await _context.SaveChangesAsync();
//			return true;
//		}
//	}
//}
