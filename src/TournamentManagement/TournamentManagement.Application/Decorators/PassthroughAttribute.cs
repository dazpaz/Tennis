using System;

namespace TournamentManagement.Application.Decorators
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	public sealed class PassthroughAttribute : Attribute
	{
		public PassthroughAttribute()
		{
		}
	}
}
