namespace GbxExplorer.Client.Services;

public class BaseAddressService : IBaseAddressService
{
	private readonly IConfiguration _config;

	public BaseAddressService(IConfiguration config)
	{
		_config = config;
	}

	public string GetRoot()
	{
		var baseAddress = _config["BaseAddress"];
		return string.IsNullOrWhiteSpace(baseAddress) ? "/" : baseAddress;
	}
}
