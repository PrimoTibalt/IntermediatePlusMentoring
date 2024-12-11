using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace RegisterServicesSourceGenerator
{
	[Generator(LanguageNames.CSharp)]
	public class RegisterServicesGenerator : IIncrementalGenerator
	{
		private const string AttributeMetadataName = "RegisterServicesSourceGenerator.RegisterServiceAttribute`1";
		private const string AttributeShortName = "RegisterServiceAttribute`1";
		private string assemblyName;

		private record struct Declaration(string InterfaceName, string LifeTime, string ImplementationName);
		public void Initialize(IncrementalGeneratorInitializationContext context)
		{
			var provider = context.SyntaxProvider.ForAttributeWithMetadataName(AttributeMetadataName, 
				static (node, _) => node is ClassDeclarationSyntax,
				(ctx, _) =>
				{
					var attribute = ctx.Attributes.Single(a => a.AttributeClass.MetadataName.Contains(AttributeShortName));
					var interfaceName = attribute.AttributeClass.TypeArguments.Single().ToDisplayString();
					var lifeTime = (LifeTime)(int)attribute.ConstructorArguments
											.Single()
											.Value;
					var implementationName = ctx.SemanticModel.GetDeclaredSymbol(ctx.TargetNode).ToDisplayString();
					assemblyName = ctx.SemanticModel.Compilation.AssemblyName;
					return new Declaration(interfaceName, lifeTime.ToString(), implementationName);
				}).Collect();

			context.RegisterSourceOutput(provider, Generate);
		}

		private void Generate(SourceProductionContext context, ImmutableArray<Declaration> source)
		{
			if (source.Length == 0) return;

			var builder = new StringBuilder();
			builder.Append($$"""
			using Microsoft.Extensions.DependencyInjection;
			using Microsoft.Extensions.DependencyInjection.Extensions;

			namespace {{assemblyName}}
			{
			public static class Injector
			{
			public static void Register(this IServiceCollection services) {
			""");
			foreach (var declaration in source)
			{
				builder.AppendLine($"services.TryAdd{declaration.LifeTime}<{declaration.InterfaceName},{declaration.ImplementationName}>();");
			}

			builder.Append("}}}");

			context.AddSource("GeneratedCode.g.cs", builder.ToString());
		}
	}
}
