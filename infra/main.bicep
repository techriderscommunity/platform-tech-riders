@description('Backend App Service plan name in the target resource group.')
@minLength(1)
@maxLength(60)
param appServicePlanName string

@description('Backend Web App name in the target resource group.')
@minLength(2)
@maxLength(60)
param webAppName string

@description('Azure SQL Server name in the target resource group.')
@minLength(1)
@maxLength(63)
param sqlServerName string

@description('Primary Azure SQL database name.')
@minLength(1)
@maxLength(128)
param sqlDatabaseName string

@description('Log Analytics workspace name linked to Application Insights.')
@minLength(4)
@maxLength(63)
param logAnalyticsWorkspaceName string

@description('Application Insights component name linked to the backend Web App.')
@minLength(1)
@maxLength(260)
param applicationInsightsName string

@description('Storage account name present in the platform resource group.')
@minLength(3)
@maxLength(24)
param storageAccountName string

@description('Static Web App name used by the Angular frontend deployment.')
@minLength(1)
param staticWebAppName string

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' existing = {
  name: appServicePlanName
}

resource webApp 'Microsoft.Web/sites@2024-04-01' existing = {
  name: webAppName
}

resource sqlServer 'Microsoft.Sql/servers@2024-05-01-preview' existing = {
  name: sqlServerName
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2024-05-01-preview' existing = {
  name: sqlDatabaseName
  parent: sqlServer
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2023-09-01' existing = {
  name: logAnalyticsWorkspaceName
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightsName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2024-01-01' existing = {
  name: storageAccountName
}

resource staticWebApp 'Microsoft.Web/staticSites@2024-04-01' existing = {
  name: staticWebAppName
}

output appServicePlanResourceId string = appServicePlan.id
output appServiceUrl string = 'https://${webApp.properties.defaultHostName}'
output sqlServerFullyQualifiedDomainName string = sqlServer.properties.fullyQualifiedDomainName
output sqlDatabaseResourceId string = sqlDatabase.id
output applicationInsightsConnectionString string = applicationInsights.properties.ConnectionString
output logAnalyticsWorkspaceResourceId string = logAnalyticsWorkspace.id
output storageAccountResourceId string = storageAccount.id
output staticWebAppDefaultHostname string = staticWebApp.properties.defaultHostname
