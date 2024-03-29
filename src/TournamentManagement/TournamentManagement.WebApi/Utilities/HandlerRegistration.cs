﻿using DomainDesign.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TournamentManagement.Application.Decorators;

namespace TournamentManagement.WebApi.Utilities
{
	public static class HandlerRegistration
	{
		public static void AddHandlers(this IServiceCollection services)
		{
			List<Type> commandHandlers = Assembly.Load("TournamentManagement.Application")
				.GetTypes()
				.Where(x => x.GetInterfaces().Any(y => IsHandlerInterface(y)))
				.Where(x => x.Name.EndsWith("Handler"))
				.ToList();

			foreach (Type type in commandHandlers)
			{
				AddHandler(services, type);
			}

			List<Type> queryHandlers = Assembly.Load("TournamentManagement.Query")
				.GetTypes()
				.Where(x => x.GetInterfaces().Any(y => IsHandlerInterface(y)))
				.Where(x => x.Name.EndsWith("Handler"))
				.ToList();

			foreach (Type type in queryHandlers)
			{
				AddHandler(services, type);
			}
		}

		private static void AddHandler(IServiceCollection services, Type type)
		{
			object[] attributes = type.GetCustomAttributes(false);

			Type interfaceType = type.GetInterfaces().Single(y => IsHandlerInterface(y));
			var count = interfaceType.GenericTypeArguments.Length;

			List<Type> pipeline = attributes
				.Select(x => ToDecorator(x, count))
				.Concat(new[] { type })
				.Reverse()
				.ToList();

			Func<IServiceProvider, object> factory = BuildPipeline(pipeline, interfaceType);

			services.AddTransient(interfaceType, factory);
		}

		private static Func<IServiceProvider, object> BuildPipeline(List<Type> pipeline, Type interfaceType)
		{
			List<ConstructorInfo> ctors = pipeline
				.Select(x =>
				{
					Type type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
					return type.GetConstructors().Single();
				})
				.ToList();

			Func<IServiceProvider, object> func = provider =>
			{
				object current = null;

				foreach (ConstructorInfo ctor in ctors)
				{
					List<ParameterInfo> parameterInfos = ctor.GetParameters().ToList();

					object[] parameters = GetParameters(parameterInfos, current, provider);

					current = ctor.Invoke(parameters);
				}

				return current;
			};

			return func;
		}

		private static object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider provider)
		{
			var result = new object[parameterInfos.Count];

			for (int i = 0; i < parameterInfos.Count; i++)
			{
				result[i] = GetParameter(parameterInfos[i], current, provider);
			}

			return result;
		}

		private static object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider provider)
		{
			Type parameterType = parameterInfo.ParameterType;

			if (IsHandlerInterface(parameterType))
				return current;

			object service = provider.GetService(parameterType);
			if (service != null)
				return service;

			throw new ArgumentException($"Type {parameterType} not found");
		}

		private static Type ToDecorator(object attribute, int count)
		{
			Type type = attribute.GetType();

			if (type == typeof(AuditCommandAttribute))
			{
				return count == 1
					? typeof(AuditCommandDecorator<>)
					: typeof(AuditCommandDecorator<,>);
			}

			if (type == typeof(PassthroughAttribute))
			{
				return count == 1
					? typeof(PassthroughDecorator<>)
					: typeof(PassthroughDecorator<,>);
			}

			// other attributes go here

			throw new ArgumentException(attribute.ToString());
		}

		private static bool IsHandlerInterface(Type type)
		{
			if (!type.IsGenericType)
				return false;

			Type typeDefinition = type.GetGenericTypeDefinition();

			return typeDefinition == typeof(ICommandHandler<>) ||
				typeDefinition == typeof(ICommandHandler<,>) ||
				typeDefinition == typeof(IQueryHandler<,>);
		}
	}
}
