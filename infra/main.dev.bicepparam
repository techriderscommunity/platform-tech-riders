using './main.bicep'

param location = 'spaincentral'
param workload = 'techriders'
param environment = 'dev'

param tags = {
  Owner: 'tech-riders'
  CostCenter: 'platform'
  ManagedBy: 'bicep-avm'
}

// CAF naming conventions + service constraints (global uniqueness where required).
param logAnalyticsWorkspaceName = 'log-techriders-dev'
param applicationInsightsName = 'appi-techriders-dev'
param appServicePlanName = 'asp-techriders-dev'
param webAppName = 'app-techriders-dev'
param sqlServerName = 'sql-techriders-dev'
param sqlDatabaseName = 'sqldb-techriders-dev'
param keyVaultName = 'kv-techriders-dev'

// Replace with real values before deployment.
param sqlAdministratorLogin = 'sqladmintr'
param sqlAdministratorPassword = 'ReplaceMe_123456789!'

param appServicePlanSkuName = 'B1'
param sqlConnectionStringSecretName = 'SqlConnectionString'
param sqlAdminPasswordSecretName = 'SqlAdminPassword'
