using System;

namespace RegisterServicesSourceGenerator
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class RegisterServiceAttribute<TInterface> : Attribute
	{
		private readonly LifeTime lifeTime;

		public RegisterServiceAttribute(LifeTime lifeTime)
		{
			this.lifeTime = lifeTime;
		}
	}
}
