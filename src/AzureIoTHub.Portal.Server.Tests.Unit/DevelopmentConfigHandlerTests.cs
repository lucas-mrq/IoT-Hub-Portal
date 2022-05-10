// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AzureIoTHub.Portal.Server.Tests.Unit
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using AzureIoTHub.Portal.Server;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DevelopmentConfigHandlerTests
    {
        private MockRepository mockRepository;

        private Mock<IConfiguration> mockConfiguration;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
        }

        private DevelopmentConfigHandler CreateDevelopmentConfigHandler()
        {
            return new DevelopmentConfigHandler(this.mockConfiguration.Object);
        }

        [TestCase(ConfigHandler.PortalNameKey, nameof(ConfigHandler.PortalName))]
        [TestCase(ConfigHandler.DPSServiceEndpointKey, nameof(ConfigHandler.DPSEndpoint))]
        [TestCase(ConfigHandler.DPSIDScopeKey, nameof(ConfigHandler.DPSScopeID))]
        [TestCase(ConfigHandler.OIDCScopeKey, nameof(ConfigHandler.OIDCScope))]
        [TestCase(ConfigHandler.OIDCAuthorityKey, nameof(ConfigHandler.OIDCAuthority))]
        [TestCase(ConfigHandler.OIDCMetadataUrlKey, nameof(ConfigHandler.OIDCMetadataUrl))]
        [TestCase(ConfigHandler.OIDCClientIdKey, nameof(ConfigHandler.OIDCClientId))]
        [TestCase(ConfigHandler.OIDCApiClientIdKey, nameof(ConfigHandler.OIDCApiClientId))]
        [TestCase(ConfigHandler.StorageAccountBlobContainerNameKey, nameof(ConfigHandler.StorageAccountBlobContainerName))]
        [TestCase(ConfigHandler.StorageAccountBlobContainerPartitionKeyKey, nameof(ConfigHandler.StorageAccountBlobContainerPartitionKey))]
        [TestCase(ConfigHandler.LoRaKeyManagementUrlKey, nameof(ConfigHandler.LoRaKeyManagementUrl))]
        [TestCase(ConfigHandler.LoRaKeyManagementCodeKey, nameof(ConfigHandler.LoRaKeyManagementCode))]
        [TestCase(ConfigHandler.LoRaRegionRouterConfigUrlKey, nameof(ConfigHandler.LoRaRegionRouterConfigUrl))]
        [TestCase(ConfigHandler.IoTHubConnectionStringKey, nameof(ConfigHandler.IoTHubConnectionString))]
        [TestCase(ConfigHandler.DPSConnectionStringKey, nameof(ConfigHandler.DPSConnectionString))]
        [TestCase(ConfigHandler.StorageAccountConnectionStringKey, nameof(ConfigHandler.StorageAccountConnectionString))]
        public void SettingsShouldGetValueFromAppSettings(string configKey, string configPropertyName)
        {
            // Arrange
            var expected = Guid.NewGuid().ToString();
            var configHandler = this.CreateDevelopmentConfigHandler();

            _ = this.mockConfiguration.SetupGet(c => c[It.Is<string>(x => x == configKey)])
                .Returns(expected);

            // Act
            var result = configHandler
                                .GetType()
                                .GetProperty(configPropertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                                .GetValue(configHandler, null);

            // Assert
            Assert.AreEqual(expected, result);
            this.mockRepository.VerifyAll();
        }

        [TestCase(ConfigHandler.IsLoRaFeatureEnabledKey, nameof(ConfigHandler.IsLoRaEnabled))]
        public void SettingsShouldGetBoolFromAppSettings(string configKey, string configPropertyName)
        {
            // Arrange
            var expected = false;
            var productionConfigHandler = this.CreateDevelopmentConfigHandler();

            _ = this.mockConfiguration.SetupGet(c => c[It.Is<string>(x => x == configKey)])
                .Returns(Convert.ToString(expected, CultureInfo.InvariantCulture));

            // Act
            var result = productionConfigHandler
                                .GetType()
                                .GetProperty(configPropertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                                .GetValue(productionConfigHandler, null);

            // Assert
            Assert.AreEqual(expected, result);

            // Arrange
            expected = true;

            _ = this.mockConfiguration.SetupGet(c => c[It.Is<string>(x => x == configKey)])
                .Returns(Convert.ToString(expected, CultureInfo.InvariantCulture));

            // Act
            result = productionConfigHandler
                                .GetType()
                                .GetProperty(configPropertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                                .GetValue(productionConfigHandler, null);

            // Assert
            Assert.AreEqual(expected, result);
            this.mockRepository.VerifyAll();
        }
    }
}