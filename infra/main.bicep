@description('Deployment location for all resources.')
param location string = resourceGroup().location

@description('Short workload identifier used in resource names.')
@minLength(2)
@maxLength(12)
param workload string

@description('Environment identifier (dev, qa, stage, prod).')
@allowed([
  'dev'
  'qa'
  'stage'
  'prod'
])
param environment string = 'dev'

@description('Tag values applied to all resources.')
param tags object = {}

@description('Log Analytics workspace name (CAF abbreviation: log).')
param logAnalyticsWorkspaceName string

@description('Application Insights name (CAF abbreviation: appi).')
param applicationInsightsName string

@description('App Service plan name (CAF abbreviation: asp).')
param appServicePlanName string

@description('Web App name (CAF abbreviation: app). Must be globally unique.')
param webAppName string

@description('Azure SQL Server name (CAF abbreviation: sql). Must be globally unique.')
param sqlServerName string

@description('Primary application database name (CAF abbreviation: sqldb).')
param sqlDatabaseName string

@description('SQL administrator login.')
@minLength(4)
@maxLength(32)
param sqlAdministratorLogin string

@secure()
@description('SQL administrator password.')
@minLength(12)
param sqlAdministratorPassword string

@description('Key Vault name (CAF abbreviation: kv). Must be globally unique.')
param keyVaultName string

@description('App Service plan SKU name for dev workloads.')
param appServicePlanSkuName string = 'B1'

@description('Name for the SQL connection string secret in Key Vault.')
param sqlConnectionStringSecretName string = 'SqlConnectionString'

@description('Name for the SQL admin password secret in Key Vault.')
param sqlAdminPasswordSecretName string = 'SqlAdminPassword'

var normalizedTags = union(tags, {
  Environment: environment
  Workload: workload
})

module logAnalyticsWorkspace 'br/public:avm/res/operational-insights/workspace:0.16.1' = {
  name: 'logAnalyticsWorkspaceDeployment'
  params: {
    name: logAnalyticsWorkspaceName
    location: location
    tags: normalizedTags
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

module applicationInsights 'br/public:avm/res/insights/component:0.8.0' = {
  name: 'applicationInsightsDeployment'
  params: {
    name: applicationInsightsName
    location: location
    workspaceResourceId: logAnalyticsWorkspace.outputs.resourceId
    tags: normalizedTags
  }
}

module keyVault 'br/public:avm/res/key-vault/vault:0.14.0' = {
  name: 'keyVaultDeployment'
  params: {
    name: keyVaultName
    location: location
    tags: normalizedTags
  }
}

module appServicePlan 'br/public:avm/res/web/serverfarm:0.7.0' = {
  name: 'appServicePlanDeployment'
  params: {
    name: appServicePlanName
    location: location
    skuName: appServicePlanSkuName
    tags: normalizedTags
  }
}

module sqlServer 'br/public:avm/res/sql/server:0.22.0' = {
  name: 'sqlServerDeployment'
  params: {
    name: sqlServerName
    location: location
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorPassword
    databases: [
      {
        availabilityZone: -1
        name: sqlDatabaseName
      }
    ]
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    secretsExportConfiguration: {
      keyVaultResourceId: keyVault.outputs.resourceId
      sqlAdminPasswordSecretName: sqlAdminPasswordSecretName
      sqlAzureConnectionStringSecretName: sqlConnectionStringSecretName
    }
    tags: normalizedTags
  }
}

module webApp 'br/public:avm/res/web/site:0.24.0' = {
  name: 'webAppDeployment'
  params: {
    name: webAppName
    location: location
    serverFarmResourceId: appServicePlan.outputs.resourceId
    httpsOnly: true
    kind: 'app'
    tags: normalizedTags
  }
}

output appServiceUrl string = 'https://${webAppName}.azurewebsites.net'
output applicationInsightsConnectionString string = applicationInsights.outputs.connectionString
output logAnalyticsWorkspaceResourceId string = logAnalyticsWorkspace.outputs.resourceId
output sqlServerFullyQualifiedDomainName string = sqlServer.outputs.fullyQualifiedDomainName
output keyVaultResourceId string = keyVault.outputs.resourceId
