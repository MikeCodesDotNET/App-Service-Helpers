using System;

namespace AppServiceHelpers.Data
{
	public enum ConflictResolutionStrategy
	{
		LatestWins,
		ClientWins,
		ServerWins
	}
}