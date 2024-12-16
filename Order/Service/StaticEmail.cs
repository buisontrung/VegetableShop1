using Order.Model;
using Order.ModelDto;

namespace Order.Service
{
	public static class StaticEmail
	{
		public static string GenerateEmailBody(string name, List<OrderDetailDTO> orderDetails, string code)
		{
			string emailbody = "<div style='width:100%;background-color:grey; padding: 20px; font-family: Arial, sans-serif;'>";

			// Header
			emailbody += $"<h1>Hi {name}, Cảm ơn vì đã đặt hàng</h1>";

			emailbody += $"<h2>Mã đơn hàng: {code}</h2>";

			// Order details


			// Table header
			emailbody += @"
            <thead>
                <tr style='background-color: #f2f2f2; text-align: left;'>
                    <th style='border: 1px solid #ddd; padding: 8px;'>Product Name</th>
                    <th style='border: 1px solid #ddd; padding: 8px;'>Quantity</th>
                    <th style='border: 1px solid #ddd; padding: 8px;'>Price</th>
                </tr>
            </thead>";

			emailbody += "<tbody>";
			foreach (var detail in orderDetails)
			{
				emailbody += $@"
                <tr>
                    <td style='border: 1px solid #ddd; padding: 8px;'>{detail.product.ProductName}</td>
                    <td style='border: 1px solid #ddd; padding: 8px;'>{detail.Quantity}</td>
                    <td style='border: 1px solid #ddd; padding: 8px;'>${detail.Price:F2}</td>
                </tr>";
			}
			emailbody += "</tbody>";
			emailbody += "</table>";

			// Footer
			emailbody += "<p style='margin-top: 20px;'>Thank you for your order!</p>";
			emailbody += "</div>";

			return emailbody;
		}
	}
}
