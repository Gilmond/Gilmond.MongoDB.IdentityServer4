// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace IdentityServer4.MongoDB.IntegrationTests
{
	public class DatabaseProviderFixture : IDisposable
	{
		// TODO: Find a way to intialize MongoDB
		// Given we don't have the luxury of EF migrations, initialization, nor seeds, 
		// contemplating using some docker integration, such as https://github.com/mariotoffia/FluentDocker
		public void Dispose()
		{
		}
	}
}
