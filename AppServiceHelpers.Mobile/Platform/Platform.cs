using System;
using System.Globalization;
using System.Reflection;

namespace AppServiceHelpers
{
	public static class Platform
	{
		static IPlatform current;

		public static string PlatformAssemblyName = "AppServiceHelpers.Ext";
		public static string PlatformTypeFullName = "AppServiceHelpers.CurrentPlatform";

		public static IPlatform Instance
		{
			get
			{
				if (current == null)
				{
					var provider = typeof(IPlatform);
					var asm = new AssemblyName(provider.GetTypeInfo().Assembly.FullName);
					asm.Name = PlatformAssemblyName;
					var name = PlatformTypeFullName + ", " + asm.FullName;

					var type = Type.GetType(name, false);
					if (type != null)
					{
						current = (IPlatform)Activator.CreateInstance(type);
					}
					else
					{
						ThrowForMissingPlatformAssembly();
					}
				}

				return current;
			}
			set
			{
				current = value;
			}
		}

		private static void ThrowForMissingPlatformAssembly()
		{
			AssemblyName portable = new AssemblyName(typeof(Platform).GetTypeInfo().Assembly.FullName);

			throw new InvalidOperationException(
				string.Format(CultureInfo.InvariantCulture,
							  "A Microsoft Azure Mobile Services assembly for the current platform was not found. Ensure that the current project references both {0} and the following platform-specific assembly: {1}.",
							portable.Name,
							PlatformAssemblyName));
		}
	}
}

