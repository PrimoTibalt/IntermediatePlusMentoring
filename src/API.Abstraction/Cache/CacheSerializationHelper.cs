using ProtoBuf;
using System.Reflection;

namespace API.Abstraction.Cache
{
	public static class CacheSerializationHelper
	{
		public static byte[] GetBytes<T>(T entity)
		{
			if (entity is null) return null;

			ValidateType<T>();
			using var memoryStream = new MemoryStream();
			Serializer.Serialize(memoryStream, entity);
			return memoryStream.ToArray();
		}

		public static bool TryGetFromBytes<T>(byte[] bytes, out T entity)
		{
			entity = default;
			if (bytes is null) return false;
			try
			{
				ValidateType<T>();
				using var memoryStream = new MemoryStream(bytes);
				var result = Serializer.Deserialize<T>(memoryStream);
				entity = result;
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static void ValidateType<T>()
		{
			if (typeof(T).IsGenericType)
			{
				var types = typeof(T).GetGenericArguments();
				if (types.Any(type => type.GetCustomAttribute<ProtoContractAttribute>() == null))
					throw new NotImplementedException();

				return;
			}

			var hasContract = typeof(T).GetCustomAttribute<ProtoContractAttribute>() != null;
			if (!hasContract)
				throw new NotImplementedException();
		}
	}
}
