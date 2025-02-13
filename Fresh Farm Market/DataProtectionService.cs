using Microsoft.AspNetCore.DataProtection;

namespace Fresh_Farm_Market
{
	
	public class DataProtectionService
	{
		private readonly IDataProtector _protector;

		public DataProtectionService(IDataProtectionProvider dataProtectionProvider)
		{
			_protector = dataProtectionProvider.CreateProtector("FreshFarmMarket.DataProtection");
		}

		public string Protect(string input)
		{
			return _protector.Protect(input);
		}

		public string Unprotect(string protectedInput)
		{
			return _protector.Unprotect(protectedInput);
		}
	}

}
